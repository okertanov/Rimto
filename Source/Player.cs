using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using NAudio;
using NAudio.Wave;

namespace Rimto
{
    public class PlayerEventArgs : EventArgs
    {
        public object Payload { get; private set; }
        public object Extra { get; private set; }

        public PlayerEventArgs(object payload)
        {
            Payload = payload;
            Extra = null;
        }

        public PlayerEventArgs(object payload, object extra)
        {
            Payload = payload;
            Extra = extra;
        }
    }

    public delegate void PlayerEventHandler(object sender, PlayerEventArgs ev);

    public class Player : IPlayer
    {
        public event PlayerEventHandler PlayingStarted = delegate { };
        public event PlayerEventHandler PlayingDone = delegate { };
        public event PlayerEventHandler Stopped = delegate { };
        public event PlayerEventHandler Paused = delegate { };
        public event PlayerEventHandler Sought = delegate { };
        public event PlayerEventHandler VolumeChanged = delegate { };

        public IPlayListItem CurrentPlayListItem { get; protected set; }

        private float _volume;

        private readonly Object _syncObject = new Object();

        private IWavePlayer _wavePlayer;
        private WaveStream _waveStream;

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public Player()
        {
            CurrentPlayListItem = null;

            _volume = 0.0F;
        }

        public Player(string media)
            : this()
        {
            CurrentPlayListItem = new PlayListItem(media);
        }

        public Player(PlayListItem item)
            : this()
        {
            CurrentPlayListItem = item;
        }

        ~Player()
        {
            if (_wavePlayer != null)
            {
                _wavePlayer.Stop();
            }

            if (_waveStream != null)
            {
                _waveStream.Close();

                _waveStream = null;
            }

            if (_wavePlayer != null)
            {
                _wavePlayer = null ;
            }
        }

        public void Play()
        {
            Play(CurrentPlayListItem);
        }

        public void Play(string media)
        {
            Play(new PlayListItem(media));
        }

        public void Play(IPlayListItem item)
        {
            PlayImpl(item);
        }

        public void Pause()
        {
            if (_wavePlayer != null)
            {
                _wavePlayer.Pause();

                Paused(this, new PlayerEventArgs(CurrentPlayListItem));
            }
        }

        public void Stop()
        {
            if (_wavePlayer != null)
            {
                _wavePlayer.Stop();

                Stopped(this, new PlayerEventArgs(CurrentPlayListItem));
            }
        }

        public void Seek()
        {
            if (_waveStream != null)
            {
                _waveStream.Seek(0, SeekOrigin.Begin);

                Sought(this, new PlayerEventArgs(CurrentPlayListItem));
            }
        }

        public float GetVolume()
        {
            float volume = default(float);

            lock (_syncObject)
            {
                volume = _volume;
            }

            return volume;
        }

        public void SetVolume(float volume)
        {
            lock (_syncObject)
            {
                _volume = volume;

                if (_waveStream != null)
                {
                    var volumeStream = _waveStream as WaveChannel32;

                    if (volumeStream != null)
                    {
                        volumeStream.Volume = _volume;
                    }
                }
            }

            VolumeChanged(this, new PlayerEventArgs(CurrentPlayListItem));
        }

        private void PlayImpl(IPlayListItem item)
        {
            if (CurrentPlayListItem != null &&
                item.Guid == CurrentPlayListItem.Guid &&
                _wavePlayer != null && _waveStream != null &&
                _wavePlayer.PlaybackState == NAudio.Wave.PlaybackState.Paused)
            {
                // Continue from Pause
                _wavePlayer.Play();

                PlayingStarted(this, new PlayerEventArgs(CurrentPlayListItem));
            }
            else
            {
                lock (_syncObject)
                {
                    CurrentPlayListItem = item;
                }

                if (CurrentPlayListItem != null)
                {
                    if (File.Exists(CurrentPlayListItem.Path))
                    {
                        // Play new audio track
                        lock (_syncObject)
                        {
                            if (_wavePlayer != null)
                            {
                                _wavePlayer.Stop();

                                _wavePlayer = null;
                            }

                            if (_wavePlayer == null)
                            {
                                _wavePlayer = new WaveOut();
                            }
                        }

                        if (_wavePlayer != null)
                        {
                            lock (_syncObject)
                            {
                                _wavePlayer.Stop();

                                if (_waveStream != null)
                                {
                                    _waveStream.Close();
                                    _waveStream = null;
                                }

                                WaveStream mp3Reader = new Mp3FileReader(CurrentPlayListItem.Path);
                                _waveStream = new WaveChannel32(mp3Reader);

                                var volumeStream = _waveStream as WaveChannel32;

                                if (volumeStream != null)
                                {
                                    _volume = volumeStream.Volume;
                                }

                                _wavePlayer.Init(_waveStream);
                                _wavePlayer.PlaybackStopped += (s, e) =>
                                {
                                    PlayingDone(this, new PlayerEventArgs(item));
                                };
                                _wavePlayer.Play();
                            }

                            PlayingStarted(this, new PlayerEventArgs(CurrentPlayListItem));
                        }
                        else
                        {
                            throw new System.InvalidOperationException("Media Player is not accessible.");
                        }
                    }
                    else
                    {
                        throw new IOException("File not found: " + CurrentPlayListItem.Path);
                    }
                }
                else
                {
                    throw new ArgumentNullException("item");
                }
            }
        }
    }
}
