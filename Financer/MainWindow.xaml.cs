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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Financer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        // Open watchlist.
        private void OpenWatchlist(object sender, RoutedEventArgs e)
        {
            WatchlistWindow watchWin = new WatchlistWindow();
            watchWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            watchWin.Show();
        }

        // Open alarms window.
        private void CreateAlarm(object sender, RoutedEventArgs e)
        {
            AlarmWindow alarmWin = new AlarmWindow();
            alarmWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            alarmWin.Show();
        }
    }
}
