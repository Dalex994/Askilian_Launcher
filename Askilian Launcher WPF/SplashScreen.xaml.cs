using Askilian_Launcher_WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Askilian_Launcher
{
    /// <summary>
    /// Logique d'interaction pour SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        //Temporary
        DispatcherTimer timer = new DispatcherTimer();

        public SplashScreen()
        {
            InitializeComponent();

            // Temporary until the update system is finished
            timer.Interval = TimeSpan.FromMilliseconds(5000);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
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
    }
}
