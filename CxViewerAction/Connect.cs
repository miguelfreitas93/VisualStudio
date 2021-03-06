using System;
using Common;
using EnvDTE80;
using CxViewerAction.Commands;
using CxViewerAction.Dispatchers;
using CxViewerAction.Entities;
using CxViewerAction.Helpers;

namespace CxViewerAction
{
    /// <summary>The object for implementing an Add-in.</summary>
    /// <seealso class='IDTExtensibility2' />OnConnection
    public class Connect
    {
        #region [ Private Members ]

        private DTE2 _applicationObject;
        

        #endregion [ Private Members ]
        
        private static bool isLoaded = true;

        public static bool IsLoaded
        {
            get { return isLoaded; }
        }


        /// <summary>
        ///     Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.
        /// </summary>
        /// <param term='application'>Root object of the host application.</param>
        /// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
        /// <param term='addInInst'>Object representing this Add-in.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnConnection(DTE2 application)
        {
            try
            {
                isLoaded = true;
                _applicationObject = application;

                try
                {
                    if (_applicationObject != null && _applicationObject.DTE != null)
                    {
                        LoginHelper.StudioVersion = _applicationObject.DTE.Version;
                        Logger.LogPath = LoginHelper.FolderPath;
                        Logger.SetConfig();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Create().Error(ex.ToString());
                }
                Logger.Create().Info("Start Loading Plugin");

                InitMenu();
            }
            catch (Exception ex)
            {
                Logger.Create().Error(ex.ToString());
            }
        }

        /// <summary>
        ///     Initialize Project and Solution menu.
        /// </summary>
        private void InitMenu()
        {
            try
            {
                ExecuteCommandHandler output;

                if (!Dispatcher.CommandExecutors.TryGetValue(typeof(LoginData), out output))
                {
                    Dispatcher.CommandExecutors.Add(typeof(LoginData), CommandExecutor.Login);
                }

                if (!Dispatcher.CommandExecutors.TryGetValue(typeof(Upload), out output))
                {
                    Dispatcher.CommandExecutors.Add(typeof(Upload), CommandExecutor.Upload);
                }

                if (!Dispatcher.CommandExecutors.TryGetValue(typeof(Scan), out output))
                {
                    Dispatcher.CommandExecutors.Add(typeof(Scan), CommandExecutor.Scan);
                }

                if (!Dispatcher.CommandExecutors.TryGetValue(typeof(BindProjectEntity), out output))
                {
                    Dispatcher.CommandExecutors.Add(typeof(BindProjectEntity), CommandExecutor.BindProject);
                }
            }
            catch (Exception ex)
            {
                Logger.Create().Error(ex.ToString());
                TopMostMessageBox.Show(ex.Message);
            }
        }
    }
}