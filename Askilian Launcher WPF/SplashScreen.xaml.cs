using Askilian_Launcher_WPF;
using Askilian_Launcher_WPF.MVVM.View;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace Askilian_Launcher
{
    /// <summary>
    /// Logique d'interaction pour SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        private string rootPath;
        private string versionFile;
        private string LauncherZip;
        private string LauncherExe;

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
                    default:
                        break;
                }

            }
        }

        public SplashScreen()
        {
            InitializeComponent();
            rootPath = Directory.GetCurrentDirectory();
            versionFile = Path.Combine(rootPath, "Version.txt");
            LauncherZip = Path.Combine(rootPath, "BuildLauncher.zip");
            LauncherExe = Path.Combine(rootPath, "AskilianPortal.exe");
            
        }

        // TEMPORARY
        private void timer_Tick(object sender, EventArgs e)
        {
            
                // MUST TELL THAT THE UPDATE MUST BE COMPLETED BEFORE LOADED
            new MainWindow().Show();
            // Close the splash screen window
            this.Close();
        }
        // End of Temporary Function


        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) 
                DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CheckForUpdates()
        {
            if (File.Exists(versionFile))
            {
                Version localversion = new Version(File.ReadAllText(versionFile));
                VersionText.Text = localversion.ToString();

                try
                {
                    WebClient webClient = new WebClient();
                    Version onlineVersion = new Version(webClient.DownloadString("Version File Link"));

                    if (onlineVersion.IsDifferentThan(localversion))
                    {
                        InstallGameFiles(true, onlineVersion);
                    }
                    else
                    {
                        Status = LauncherStatus.ready;
                    }
                }
                catch (Exception ex) 
                {
                    Status = LauncherStatus.failed;
                    //Debug
                    MessageBox.Show($"Error checking for game updates: {ex}");
                }
            }
            else
            {
                InstallGameFiles(false, Version.zero);
            }
        }

        private void InstallGameFiles(bool _isUpdate, Version _onlineversion)
        {
            try
            {
                WebClient webClient = new WebClient();
                if (_isUpdate)
                {
                    Status = LauncherStatus.downloadingUpdate;
                }

                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadGAmeCompletedCallback);
                webClient.DownloadFileAsync(new Uri("GameZipLink"), LauncherZip, _onlineversion);

            }
            catch (Exception ex)
            {
                Status = LauncherStatus.failed;
                // Debug
                MessageBox.Show($"Error installing game files: {ex}");
            }
        }

        private void DownloadGAmeCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                string onlineVersion = ((Version)e.UserState).ToString();
                ZipFile.ExtractToDirectory(LauncherZip, rootPath, true);
                File.Delete(LauncherZip);

                File.WriteAllText(versionFile, onlineVersion);

                VersionText.Text = onlineVersion;
                Status = LauncherStatus.ready;
            }
            catch (Exception ex)
            {
                Status = LauncherStatus.failed;
                // Debug
                MessageBox.Show($"Error finishing download: {ex}");
            }
        }

        private void InstanceCall(object sender, EventArgs e)
        {
            if (File.Exists(LauncherExe) && Status == LauncherStatus.ready)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(LauncherExe);
                startInfo.WorkingDirectory = Path.Combine(rootPath, "Build");
                Process.Start(startInfo);

                Close();
            }
            else if (Status == LauncherStatus.failed) 
            { 
                CheckForUpdates();
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            CheckForUpdates();
        }

        struct Version
        {
            internal static Version zero = new Version(0, 0, 0);

            private short major;
            private short minor;
            private short subMinor;

            internal Version(short _major, short _minor, short _subMinor)
            {
                major = _major; 
                minor = _minor; 
                subMinor = _subMinor;

            }

            internal Version(string _version)
            {
                string[] _versionStrings = _version.Split('.');
                if (_versionStrings.Length != 3 ) {
                    major = 0;
                    minor = 0;
                    subMinor = 0;
                    return;
                }

                major = short.Parse(_versionStrings[0]);
                minor = short.Parse(_versionStrings[1]);
                subMinor = short.Parse(_versionStrings[2]);
            }

            internal bool IsDifferentThan(Version _otherVersion)
            {
                if (major != _otherVersion.minor)
                {
                    return true;
                }
                else
                {
                    if (minor != _otherVersion.minor)
                    {
                        return true;
                    }
                    else
                    {
                        if (subMinor != _otherVersion.subMinor)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            public override string ToString()
            {
                return $"{major}.{minor}.{subMinor}";
            }
        }

    }
}
