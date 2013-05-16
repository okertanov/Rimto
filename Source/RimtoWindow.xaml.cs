using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

/**
    @see WPF custom NC window styles:

        http://elysium.codeplex.com/

        http://habrahabr.ru/post/158561/

        http://blogs.msdn.com/b/wpfsdk/archive/2010/08/25/experiments-with-windowchrome.aspx

        http://msdn.microsoft.com/en-us/library/windows/desktop/bb688195(v=vs.85).aspx

        http://wpftutorial.net/RemoveIcon.html

        http://msdn.microsoft.com/en-us/library/windows/desktop/ms684247(v=vs.85).aspx

        http://msdn.microsoft.com/en-us/library/windows/desktop/ff468822(v=vs.85).aspx

        http://msdn.microsoft.com/en-us/library/vstudio/system.windows.window.sizetocontent(v=vs.90).aspx

        http://archive.msdn.microsoft.com/WPFShell

        http://blogs.msdn.com/b/wpfsdk/archive/2008/09/08/custom-window-chrome-in-wpf.aspx

        http://stackoverflow.com/questions/6032032/how-do-i-compute-the-non-client-window-size-in-wpf

        http://stackoverflow.com/questions/3254179/how-to-remove-the-non-client-area-of-a-wpf-window-without-using-allowtransparenc
*/

namespace Rimto
{
    public enum RimtoState
    {
        Stopped,
        Paused,
        Playing
    }

    public partial class RimtoWindow : Window
    {
        public RimtoState State = RimtoState.Stopped;

        private readonly int _normalWidth = (int)SystemParameters.MinimumWindowWidth * 3;
        private readonly int _expandedWidth = (int)SystemParameters.MinimumWindowWidth * 6;
        private readonly int _normalHeight = (int)SystemParameters.MinimumWindowHeight * 2;
        private readonly int _expandedHeight = (int)SystemParameters.MinimumWindowHeight * 8;

        static string _playListName = @"DefaultPlaylist.rimto";
        static string _playListFullPath =
            System.IO.Path.GetFullPath(
                System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                              _playListName));

        private PlayList _playlist = new PlayList();
        private Player _player = new Player();

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public RimtoWindow()
        {
            InitializeDataContext(this);

            InitializeComponent();

            Loaded += new RoutedEventHandler(Window_Loaded);
            Closing += new CancelEventHandler(Window_Closing);
        }

        private void InitializeDataContext(object dctx)
        {
            DataContext = dctx;
        }

        protected override void OnSourceInitialized(EventArgs args)
        {
            try
            {
                base.OnSourceInitialized(args);

                UiHelpers.ExtendFrameIntoClientArea(this);

                UiHelpers.SetWindowThemeAttribute(this, false, false, false);

                UiHelpers.AttachWndProcHook(this, this.MessageHandler);

                MaxWidth = Width = _normalWidth;
                Height = _normalHeight;
            }
            catch (Exception e)
            {
                Logger.Error("Exception: {0}", e.ToString());
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs args)
        {
            try
            {
                Logger.Info("Rimto main window loaded OK.");

                PlaylistListView.DataContext = this;
                PlaylistListView.Items.Clear();
                PlaylistListView.ItemsSource = _playlist;
                PlaylistListView.IsSynchronizedWithCurrentItem = true;
                

                Logger.Info("Restoring playlist from {0}", _playListFullPath);
                _playlist.Restore(_playListFullPath);
                
                PlaylistListView.Items.Refresh();
            }
            catch (Exception e)
            {
                Logger.Error("Exception: {0}", e.ToString());
            }
        }

        void Window_Closing(object sender, CancelEventArgs args)
        {
            try
            {
                Logger.Info("Rimto main window closing...");

                Logger.Info("Saving playlist to {0}", _playListFullPath);
                _playlist.Save(_playListFullPath);
            }
            catch (Exception e)
            {
                Logger.Error("Exception: {0}", e.ToString());
            }
        }

        private void Expander_Expanded(object sender, RoutedEventArgs args)
        {
            try
            {
                Width = _normalWidth;
                Height = _expandedHeight;

                PlaylistPanel.Visibility = Visibility.Visible;
            }
            catch (Exception e)
            {
                Logger.Error("Exception: {0}", e.ToString());
            }
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs args)
        {
            try
            {
                PlaylistPanel.Visibility = Visibility.Collapsed;

                Width = _normalWidth;
                Height = _normalHeight;
            }
            catch (Exception e)
            {
                Logger.Error("Exception: {0}", e.ToString());
            }
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs args)
        {
            try
            {
                HandlePlayPauseStopCommand();
            }
            catch (Exception e)
            {
                Logger.Error("Exception: {0}", e.ToString());

                throw e;
            }
        }

        private void PlaylistListView_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            try
            {
                if (PlaylistListView.Items.Count > 0)
                {
                    IPlayListItem item = PlaylistListView.SelectedItem as IPlayListItem;

                    if (item != null)
                    {
                        // Update UI

                        if (State == RimtoState.Playing && item.Guid == _player.CurrentPlayListItem.Guid)
                        {
                            PlayPauseImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/pause-32x32.png"));
                        }
                        else
                        {
                            PlayPauseImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/play-32x32.png"));
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Play List item not found.");
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error("Exception: {0}", e.ToString());

                throw e;
            }
        }

        private void HandlePlayPauseStopCommand(string command)
        {
            try
            {
                if (PlaylistListView.Items.Count > 0 && command != "Stop")
                {
                    IPlayListItem item = PlaylistListView.SelectedItem as IPlayListItem;

                    if (item == null)
                    {
                        PlaylistListView.SelectedIndex = 0;
                        item = PlaylistListView.SelectedItem as IPlayListItem;

                        if (item == null)
                        {
                            throw new InvalidOperationException("Play List item not found.");
                        }
                    }

                    if (State == RimtoState.Playing && item.Guid == _player.CurrentPlayListItem.Guid)
                    {
                        State = RimtoState.Paused;
                        _player.Pause();
                    }
                    else if (State == RimtoState.Playing && item.Guid != _player.CurrentPlayListItem.Guid)
                    {
                        State = RimtoState.Playing;
                        _player.Play(item);
                    }
                    else if (State == RimtoState.Paused && item.Guid == _player.CurrentPlayListItem.Guid)
                    {
                        State = RimtoState.Playing;
                        _player.Play();
                    }
                    else if (State == RimtoState.Paused && item.Guid != _player.CurrentPlayListItem.Guid)
                    {
                        State = RimtoState.Playing;
                        _player.Play(item);
                    }
                    else if (State == RimtoState.Stopped)
                    {
                        State = RimtoState.Playing;
                        _player.Play(item);
                    }
                }
                else
                {
                    State = RimtoState.Stopped;
                    _player.Stop();
                }
            }
            catch (Exception e)
            {
                Logger.Error("Exception: {0}", e.ToString());

                throw e;
            }
            finally
            {
                // Update UI
                switch (State)
                {
                    case RimtoState.Playing:
                        WindowCaptionLabel.Content = _player.CurrentPlayListItem.Title;
                        PlayPauseImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/pause-32x32.png"));
                        break;
                    case RimtoState.Paused:
                        WindowCaptionLabel.Content = _player.CurrentPlayListItem.Title;
                        PlayPauseImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/play-32x32.png"));
                        break;
                    case RimtoState.Stopped:
                        WindowCaptionLabel.Content = "";
                        PlayPauseImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/play-32x32.png"));
                        break;
                    default:
                        break;
                }
            }
        }

        private void HandlePlayPauseStopCommand()
        {
            HandlePlayPauseStopCommand(String.Empty);
        }

        private void HandleRemovePlayListItem()
        {
            try
            {
                if (PlaylistListView.Items.Count > 0)
                {
                    IPlayListItem item = PlaylistListView.SelectedItem as IPlayListItem;

                    if (item != null)
                    {
                        var old = PlaylistListView.SelectedIndex;
                        _playlist.Remove(item);
                        PlaylistListView.SelectedIndex = old;
                    }
                    else
                    {
                        throw new InvalidOperationException("Play List item not found.");
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error("Exception: {0}", e.ToString());

                throw e;
            }
        }

        // TODO: Skip to next
        void Player_PlayingDone(object sender, PlayerEventArgs ev)
        {
            try
            {
                PlaylistListView.SelectedIndex += 1;
                HandlePlayPauseStopCommand();
            }
            catch (Exception e)
            {
                Logger.Error("Exception: {0}", e.ToString());
            }
        }
    }
}
