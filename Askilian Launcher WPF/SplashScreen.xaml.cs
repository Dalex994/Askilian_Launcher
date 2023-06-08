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
                        TextButton.Text = "Incantation ratée ^^' - Réessayer";
                        break;
                    case LauncherStatus.downloadingUpdate:
                        TextButton.Text = "Invocation magique en cours...";
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
            cts = new CancellationTokenSource();
            remoteFolderUrl = "Url Here";
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
            // Step 1: Get a list of files in the local folder
            var localFiles = Directory.GetFiles(localFolder, "*", SearchOption.AllDirectories);

            try
            {
                // Step 2: Send an HTTP GET request to retrieve a list of files from the remote server folder
                var request = (HttpWebRequest)WebRequest.Create(remoteFolderUrl);
                request.Method = WebRequestMethods.Http.Get;
                request.Accept = "*/*";
                request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, none");
                request.Headers.Add(HttpRequestHeader.CacheControl, "max-age=0");
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.None;
                using (var response = (HttpWebResponse)request.GetResponse())
                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    var remoteFiles = reader.ReadToEnd().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    cts.Token.ThrowIfCancellationRequested();
                    // Step 3: Compare the list of files in the local and remote folders to identify the files that are missing from the local folder
                    var missingFiles = remoteFiles.Select(f => new Uri(remoteFolderUrl + "/" + f))
                                                  .Except(localFiles.Select(f => new Uri(f)))
                                                  .ToList();

                    try
                    {
                        // Step 4: Download the missing files to the local folder, using the same subfolders as on the web
                        Status = LauncherStatus.downloadingUpdate;
                        using (var webClient = new WebClient())
                        {
                            cts.Token.ThrowIfCancellationRequested();
                            foreach (var missingFileUrl in missingFiles)
                            {
                                var missingFilePath = Path.Combine(localFolder, missingFileUrl.LocalPath.TrimStart('/'));
                                if (!Directory.Exists(Path.GetDirectoryName(missingFilePath)))
                                    Directory.CreateDirectory(Path.GetDirectoryName(missingFilePath));
                                webClient.DownloadFile(missingFileUrl, missingFilePath);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Status = LauncherStatus.failed;
                        // Debug
                        MessageBox.Show($"Error finishing download: {ex}");
                    }

                    // Step 5: Delete any files in the local subfolder that are not present in the remote subfolder
                    foreach (var localFile in localFiles)
                    {
                        cts.Token.ThrowIfCancellationRequested();
                        var localFileName = Path.GetFileName(localFile);
                        if (!remoteFiles.Contains(Path.GetFileName(localFile)))
                            // Verify for the remoteFolderUrl
                        {
                            File.Delete(localFile);
                        }
                    }
                }
            }
            catch (Exception ex) 
            { 
                Status = LauncherStatus.failed;
                //Debug
                MessageBox.Show($"Error attempting to doxnload: {ex.Message}");    
            }


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
