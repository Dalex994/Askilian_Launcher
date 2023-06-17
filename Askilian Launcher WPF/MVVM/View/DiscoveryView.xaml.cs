using System;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Diagnostics.Eventing.Reader;

namespace Askilian_Launcher_WPF.MVVM.View
{

    enum LauncherStatus
    {
        ready,
        failed,
        downloadingGame,
        downloadingUpdate,
        searchingUpdate,
    }

    /// <summary>
    /// Logique d'interaction pour DiscoveryView.xaml
    /// </summary>
    public partial class DiscoveryView : UserControl
    {
        private LauncherStatus _status;
        private string MirumPath;
        private string MirumRemoteUrl;
        private bool exeExists;
        private string folderPath;
        private string rootPath;
        private string folderName;



        internal LauncherStatus Status
        {
            get => _status;
            set 
            {
                _status = value;
                switch (_status)
                {
                    case LauncherStatus.ready:
                        PlayButton1.Content = "Entrer";
                        break;
                    case LauncherStatus.failed:
                        PlayButton1.Content = "Incantation ratée";
                        break;
                    case LauncherStatus.downloadingGame:
                        PlayButton1.Content = "Invocation...";
                        break;
                    case LauncherStatus.downloadingUpdate:
                        PlayButton1.Content = "Révision des sorts en cours";
                        break;
                    case LauncherStatus.searchingUpdate:
                        PlayButton1.Content = "Recherche magique...";
                        break;
                    default:
                        break;
                }
            
            }
        }

        

        public DiscoveryView()
        {
            InitializeComponent();
            MirumPath = Directory.GetCurrentDirectory();
            MirumRemoteUrl = "https://onedrive.live.com/download?cid=FB971FAC14D737C0&resid=FB971FAC14D737C0%21201&authkey=AGzJT1iQYzx0oWo";
            exeExists = File.Exists(MirumPath);
            rootPath = rootPath;
            folderPath = System.IO.Path.Combine(rootPath, folderName);
            folderName = "Mirum Orbis";
            Loaded += PlayButton1_Click;
        }

        private CancellationTokenSource cts;

        private void UpdateGame()
        {
            
            // Step 1: Get a list of files in the local folder
            var localFiles = Directory.GetFiles(MirumPath, "*", SearchOption.AllDirectories);

            try
            {
                foreach (var localFile in localFiles)
                {
                    // Get the file path relative to the source folder
                    var relativeFilePath = localFile.Replace(MirumPath + Path.DirectorySeparatorChar, "");

                    // Get the corresponding path for the file on the web server
                    var webServerFilePath = Path.Combine(MirumRemoteUrl, relativeFilePath);
                    cts.Token.ThrowIfCancellationRequested();

                    if (File.GetLastWriteTimeUtc(webServerFilePath) == File.GetLastWriteTimeUtc(localFile))
                    {
                        Status = LauncherStatus.ready;
                        Process.Start(rootPath, "Mirum Orbis.exe");
                    }
                    else
                    {
                        // Check if the file exists on the web server and if it is newer than the local file
                        if (File.Exists(webServerFilePath) && File.GetLastWriteTimeUtc(webServerFilePath) > File.GetLastWriteTimeUtc(localFile))
                        {
                            // Get the directory path for the file in the destination folder
                            var destinationFolderFilePath = Path.Combine(MirumRemoteUrl, Path.GetDirectoryName(relativeFilePath));

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
            }
            catch (Exception ex)
            {
                Status = LauncherStatus.failed;
                //Debug
                MessageBox.Show($"Error attempting to download: {ex}");
            }

        }


        private void DownloadGame()
        {
            // Open a dialog window to choose the directory => works only for Windows
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.FileName = "Select a Directory"; // Default file name
            dialog.Filter = "Folders|*.this.directory.only"; // Filter files by extension
            dialog.CheckFileExists = false;
            dialog.CheckPathExists = true;
            dialog.ValidateNames = false;
            dialog.Title = "Select a Directory";

            try
            {
                if (dialog.ShowDialog() == true)
                {
                    rootPath = System.IO.Path.GetDirectoryName(dialog.FileName);
                    // Debug
                    Console.WriteLine($"Selected directory: {rootPath}");
                    
                    // Download the Game folder
                    Status = LauncherStatus.downloadingGame;
                    Directory.CreateDirectory(folderPath);
                    using (var client = new WebClient())
                    {
                        client.DownloadFile(MirumRemoteUrl, folderPath);
                        cts.Token.ThrowIfCancellationRequested();
                        
                    }
                    Status = LauncherStatus.ready;
                }

            }
            catch (Exception ex)
            {
                Status = LauncherStatus.failed;
                MessageBox.Show($"Invocation interrompue, erreur magique: {ex}");
                // Handle the cancellation, if necessary
            }

        }

        private void PlayButton1_Click(object sender, RoutedEventArgs e)
        {
            if (exeExists)
            {
                // If the .exe file already exists, then find if any update is available
                Status = LauncherStatus.searchingUpdate;
                

            }
            else 
            {
                // If it doesn't exists, install the game
                DownloadGame();
                Process.Start(rootPath,"Mirum Orbis.exe");

            }
        }

    }
}
