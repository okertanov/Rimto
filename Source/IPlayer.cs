using System;

namespace Rimto
{
    interface IPlayer
    {
        void Play();
        void Play(string media);
        void Play(IPlayListItem item);

        void Pause();

        void Seek();
        void Stop();

        float GetVolume();
        void SetVolume(float volume);
    }
}
