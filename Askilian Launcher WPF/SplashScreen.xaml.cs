using Askilian_Launcher_WPF;
using Askilian_Launcher_WPF.MVVM.View;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Net.Http;
using System.Linq;
using System.Threading;
using System.Reflection;

namespace Askilian_Launcher
{
    /// <summary>
    /// Logique d'interaction pour SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        private string remoteFolderUrl;
        private string localFolder;
        private string[] webFiles;
        private string[] localFiles;
        private string VersionName;
        private string VersionContent;
        private CancellationTokenSource cts;

        private LauncherStatus _status;

        internal LauncherStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                switch (_status)
                {
                    case LauncherStatus.ready:
                        TextButton.Text = "Ouverture du portail...";
                        break;
                    case LauncherStatus.failed:
                        TextButton.Text = "Magicien crevé, c'est raté ^^'";
                        break;
                    case LauncherStatus.downloadingUpdate:
                        TextButton.Text = "Changement de gardien";
                        break;
                    case LauncherStatus.searchingUpdate:
                        TextButton.Text = "Recherche des grimoires...";
                        break;
                    default:
                        break;
                }

            }
        }

        public SplashScreen()
        {
            InitializeComponent();
            GetVersion();
            cts = new CancellationTokenSource();
            remoteFolderUrl = "https://onedrive.live.com/download?cid=FB971FAC14D737C0&resid=FB971FAC14D737C0%21200&authkey=ALOCmxYUGFVZatQ";
            localFolder = Directory.GetCurrentDirectory();
            webFiles = null;
            VersionName = "Askilian_Launcher.Version.txt";
            ContentRendered += Window_ContentRendered;
            // MUST TELL THAT THE UPDATE MUST BE COMPLETED BEFORE LOADED
            new MainWindow().Show();
            this.Close();   
        }


        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) 
                DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        // Update function by automatic search
        private void UpdateLocalFolder(string localFolder, string remoteFolderUrl)
        {
            Status = LauncherStatus.searchingUpdate;
            // Step 1: Get a list of files in the local folder
            var localFiles = Directory.GetFiles(localFolder, "*", SearchOption.AllDirectories);

            try
            {
                foreach (var localFile in localFiles)
                {
                    // Get the file path relative to the source folder
                    var relativeFilePath = localFile.Replace(localFolder + Path.DirectorySeparatorChar, "");

                        // Get the corresponding path for the file on the web server
                        var webServerFilePath = Path.Combine(remoteFolderUrl, relativeFilePath);
                        cts.Token.ThrowIfCancellationRequested();

                        // Check if the file exists on the web server and if it is newer than the local file
                        if (File.Exists(webServerFilePath) && File.GetLastWriteTimeUtc(webServerFilePath) > File.GetLastWriteTimeUtc(localFile))
                        {
                            // Get the directory path for the file in the destination folder
                            var destinationFolderFilePath = Path.Combine(remoteFolderUrl, Path.GetDirectoryName(relativeFilePath));

                            try
                            {
                            // Create the directory if it doesn't exist
                            cts.Token.ThrowIfCancellationRequested();
                            if (!Directory.Exists(destinationFolderFilePath))
                            {
                                Directory.CreateDirectory(destinationFolderFilePath);
                            }

                            // Copy the file from the web server to the local folder
                            Status = LauncherStatus.downloadingUpdate;
                            File.Copy(webServerFilePath, localFile, true);
                            cts.Token.ThrowIfCancellationRequested();

                            }
                            catch (Exception ex)
                            {
                                Status = LauncherStatus.failed;
                                //Debug
                                MessageBox.Show($"Error finishing to download: {ex}");
                            }
                        }
                        else
                        {
                            // Delete the file if it doesn't exist on the web server
                            cts.Token.ThrowIfCancellationRequested();
                            File.Delete(localFile);
                        }
                    

                    
                }
            }
            catch (Exception ex) 
            { 
                Status = LauncherStatus.failed;
                //Debug
                MessageBox.Show($"Error attempting to download: {ex}");    
            }

        }

        private void GetVersion()
        {
            // Step Optional: Read the Version file and show it. The Version file must be in the project Path
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(VersionName))
            {
                using (var reader = new StreamReader(stream))
                {
                    VersionContent = reader.ReadToEnd();
                }
            }
            Version.Text = VersionContent;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            UpdateLocalFolder(localFolder, remoteFolderUrl);
        }

        
    }
}
