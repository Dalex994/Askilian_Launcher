using System;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Net.Http;
using System.Threading.Tasks;
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
        downloadingUpdate
    }

    /// <summary>
    /// Logique d'interaction pour DiscoveryView.xaml
    /// </summary>
    public partial class DiscoveryView : UserControl
    {
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
                        PlayButton1.Content = "Entrer";
                        break;
                    case LauncherStatus.failed:
                        PlayButton1.Content = "Incantation ratée - Réessayer";
                        break;
                    case LauncherStatus.downloadingGame:
                        PlayButton1.Content = "Archivage du monde en cours";
                        break;
                    case LauncherStatus.downloadingUpdate:
                        PlayButton1.Content = "Révision des sorts en cours";
                        break;
                    default:
                        break;
                }
            
            }
        }


        public DiscoveryView()
        {
            InitializeComponent();
            Loaded += UserControl1_Loaded;
        }

        private CancellationTokenSource cts;

        private void CheckForUpdates()
        {
            
            
        }  
            private void ProcessUserControl()
        {
            // Create an instance of UserControl1
            UserControl userControl = new UserControl();

            // Create a new CancellationTokenSource object
            cts = new CancellationTokenSource();

            // Attach UserControl1_Loaded event handler
            userControl.Loaded += UserControl1_Loaded;
        }


        private async void UserControl1_Loaded(object sender, RoutedEventArgs e)
        {
            if (true)
            {
                try
                {
                    // Check if the CancellationToken has been cancelled
                    cts.Token.ThrowIfCancellationRequested();

                    CheckForUpdates();
                    if (true)
                    {

                    }
                    else
                    {
                        Status = LauncherStatus.ready;
                    } // CODE HERE !!!!

                }
                catch (Exception ex)
                {
                    Status = LauncherStatus.failed;
                    MessageBox.Show($"Invocation interrompue, erreur magique: {ex}");// Handle the cancellation, if necessary
                }
            }
            else
            {
                // Install game files
            }
        }
        
        private void PlayButton1_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
