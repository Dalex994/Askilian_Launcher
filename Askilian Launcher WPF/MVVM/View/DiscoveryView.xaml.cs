using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Askilian_Launcher_WPF.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour DiscoveryView.xaml
    /// </summary>
    public partial class DiscoveryView : UserControl
    {
        public DiscoveryView()
        {
            InitializeComponent();
            Loaded += UserControl1_Loaded;
        }

        CancellationTokenSource cts = new CancellationTokenSource();


        private void UserControl1_Loaded(object sender, RoutedEventArgs e)
        {
             async void LongRunningProcess(CancellationToken token)
            {
                await Task.Run(() =>
                {
                    // Code here ^^
                    token.ThrowIfCancellationRequested();

                }, token);

                cts.Cancel();
            }
        }

        private void PlayMirum_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
