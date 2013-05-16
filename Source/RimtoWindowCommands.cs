using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Rimto
{
    public partial class RimtoWindow
    {
        /**
            @brief ApplicationCommands.Help
            @note F1
        */
        private void ApplicationCommands_Help(object sender, ExecutedRoutedEventArgs args)
        {
            try
            {
                Logger.Info("Routed Command: [{0}] forom {1}", ((RoutedCommand)args.Command).Name, sender.ToString());
            }
            catch (Exception e)
            {
                Logger.Error("Exception: {0}", e.ToString());
            }
        }

        private void ApplicationCommands_Help_CanExecute(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = true;
        }

        /**
            @brief ApplicationCommands.New
            @note Control-N
        */
        private void ApplicationCommands_New(object sender, ExecutedRoutedEventArgs args)
        {
            try
            {
                Logger.Info("Routed Command: [{0}] forom {1}", ((RoutedCommand)args.Command).Name, sender.ToString());
            }
            catch(Exception e)
            {
                Logger.Error("Exception: {0}", e.ToString());
            }
        }

        private void ApplicationCommands_New_CanExecute(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = true;
        }

        /**
            @brief ApplicationCommands.Open
            @note Control-O
        */
        private void ApplicationCommands_Open(object sender, ExecutedRoutedEventArgs args)
        {
            try
            {
                Logger.Info("Routed Command: [{0}] forom {1}", ((RoutedCommand)args.Command).Name, sender.ToString());

                Microsoft.Win32.OpenFileDialog openDlg = new Microsoft.Win32.OpenFileDialog()
                {
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
                    ValidateNames = true,
                    Multiselect = true,
                    Filter = @"MP3 files (*.mp3)|*.mp3"
                };

                Nullable<bool> result = openDlg.ShowDialog();
                if (result == true)
                {
                    string[] selectedFileNames = openDlg.FileNames;

                    if (selectedFileNames.Length > 0)
                    {
                        _playlist.Add(selectedFileNames);
                    }
                }
            }
            catch(Exception e)
            {
                Logger.Error("Exception: {0}", e.ToString());
            }
        }

        private void ApplicationCommands_Open_CanExecute(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = true;
        }

        /**
            @brief ApplicationCommands.Save, ApplicationCommands.SaveAs
            @note Control-S, Control-Shift-S
        */
        private void ApplicationCommands_Save(object sender, ExecutedRoutedEventArgs args)
        {
            try
            {
                Logger.Info("Routed Command: [{0}] forom {1}", ((RoutedCommand)args.Command).Name, sender.ToString());
            }
            catch(Exception e)
            {
                Logger.Error("Exception: {0}", e.ToString());
            }
        }

        private void ApplicationCommands_Save_CanExecute(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = true;
        }

        /**
            @brief ApplicationCommands.Close
            @note Control-W
        */
        private void ApplicationCommands_Close(object sender, ExecutedRoutedEventArgs args)
        {
            try
            {
                Logger.Info("Routed Command: [{0}] forom {1}", ((RoutedCommand)args.Command).Name, sender.ToString());
            }
            catch(Exception e)
            {
                Logger.Error("Exception: {0}", e.ToString());
            }
        }

        private void ApplicationCommands_Close_CanExecute(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = true;
        }

        /**
            @brief ApplicationCommands.Properties
            @note Alt-Enter
        */
        private void ApplicationCommands_Properties(object sender, ExecutedRoutedEventArgs args)
        {
            try
            {
                Logger.Info("Routed Command: [{0}] forom {1}", ((RoutedCommand)args.Command).Name, sender.ToString());
            }
            catch(Exception e)
            {
                Logger.Error("Exception: {0}", e.ToString());
            }
        }

        private void ApplicationCommands_Properties_CanExecute(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = true;
        }

        /**
            @brief ApplicationCommands.Stop
            @note Escape key, Control-Break
        */
        private void ApplicationCommands_Stop(object sender, ExecutedRoutedEventArgs args)
        {
            try
            {
                Logger.Info("Routed Command: [{0}] forom {1}", ((RoutedCommand)args.Command).Name, sender.ToString());
            }
            catch(Exception e)
            {
                Logger.Error("Exception: {0}", e.ToString());
            }
        }

        private void ApplicationCommands_Stop_CanExecute(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = true;
        }

        /**
            @brief ApplicationCommands.Find, NavigationCommands.Search
            @note Control-F, F3
        */
        private void NavigationCommands_Search(object sender, ExecutedRoutedEventArgs args)
        {
            try
            {
                Logger.Info("Routed Command: [{0}] forom {1}", ((RoutedCommand)args.Command).Name, sender.ToString());
            }
            catch(Exception e)
            {
                Logger.Error("Exception: {0}", e.ToString());
            }
        }

        private void NavigationCommands_Search_CanExecute(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = true;
        }

        /**
            @brief NavigationCommands.Refresh
            @note F5
        */
        private void NavigationCommands_Refresh(object sender, ExecutedRoutedEventArgs args)
        {
            try
            {
                Logger.Info("Routed Command: [{0}] forom {1}", ((RoutedCommand)args.Command).Name, sender.ToString());
            }
            catch(Exception e)
            {
                Logger.Error("Exception: {0}", e.ToString());
            }
        }

        private void NavigationCommands_Refresh_CanExecute(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = true;
        }
        
        /**
            @brief MediaCommands.Play, MediaCommands.Pause
                   MediaCommands.TogglePlayPause, MediaCommands.Stop
            @note Control-P, Control-Shift-P
                  Space, Control-K
        */
        private void MediaCommands_TogglePlayPause(object sender, ExecutedRoutedEventArgs args)
        {
            try
            {
                string commandName = ((RoutedCommand)args.Command).Name;

                Logger.Info("Routed Command: [{0}] forom {1}", commandName, sender.ToString());

                HandlePlayPauseStopCommand(commandName);
            }
            catch (Exception e)
            {
                Logger.Error("Exception: {0}", e.ToString());

                throw e;
            }
        }

        private void MediaCommands_TogglePlayPause_CanExecute(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = true;
        }

        /**
            @brief MediaCommands.Rewind, MediaCommands.FastForward
            @note Shift-Left, Shift-Right
        */
        private void MediaCommands_FastForwardRewind(object sender, ExecutedRoutedEventArgs args)
        {
            try
            {
                Logger.Info("Routed Command: [{0}] forom {1}", ((RoutedCommand)args.Command).Name, sender.ToString());
            }
            catch(Exception e)
            {
                Logger.Error("Exception: {0}", e.ToString());
            }
        }

        private void MediaCommands_FastForwardRewind_CanExecute(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = true;
        }

        /**
            @brief MediaCommands.PreviousTrack, MediaCommands.NextTrack
            @note Control-Left, Control-Right
        */
        private void MediaCommands_TrackControl(object sender, ExecutedRoutedEventArgs args)
        {
            try
            {
                Logger.Info("Routed Command: [{0}] forom {1}", ((RoutedCommand)args.Command).Name, sender.ToString());
            }
            catch(Exception e)
            {
                Logger.Error("Exception: {0}", e.ToString());
            }
        }

        private void MediaCommands_TrackControl_CanExecute(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = true;
        }
                   
        /**
            @brief MediaCommands.IncreaseVolume, MediaCommands.DecreaseVolume, MediaCommands.MuteVolume
            @note Control-Up, Control-Down, Control-M
        */
        private void MediaCommands_VolumeControl(object sender, ExecutedRoutedEventArgs args)
        {
            try
            {
                Logger.Info("Routed Command: [{0}] forom {1}", ((RoutedCommand)args.Command).Name, sender.ToString());
            }
            catch(Exception e)
            {
                Logger.Error("Exception: {0}", e.ToString());
            }
        }

        private void MediaCommands_VolumeControl_CanExecute(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = true;
        }

        /**
            @brief ListViewCommands_RemovePlayListItem
            @note Delete
        */
        private static RoutedUICommand _removePlayListItemCommand = new RoutedUICommand("Remove Play List Item Command", "RemovePlayListItemCommand", typeof(RimtoWindow));
        public static RoutedUICommand RemovePlayListItemCommand { get { return _removePlayListItemCommand; } }

        private void ListViewCommands_RemovePlayListItem(object sender, ExecutedRoutedEventArgs args)
        {
            try
            {
                Logger.Info("Routed Command: [{0}] forom {1}", ((RoutedCommand)args.Command).Name, sender.ToString());

                HandleRemovePlayListItem();
            }
            catch (Exception e)
            {
                Logger.Error("Exception: {0}", e.ToString());
            }
        }

        private void ListViewCommands_RemovePlayListItem_CanExecute(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = true;
        }
    }
}
