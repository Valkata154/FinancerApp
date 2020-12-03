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
using System.Configuration;
using System.Data.SqlClient;
using System.Data;


namespace Financer
{
    /// <summary>
    /// Interaction logic for AddAlarmWindow.xaml
    /// </summary>
    public partial class AddAlarmWindow : Window
    {
        private string connectionString;
        private SqlConnection connection;

        private void Window_Closed(object sender, EventArgs e)
        {
            AlarmWindow win = new AlarmWindow();
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.PopulateData();
            win.Show();
        }

        public AddAlarmWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString;
        }

        private void AddAlarm(object sender, RoutedEventArgs e)
        {
            string query = "INSERT INTO Alarm VALUES (@Ticker, @Price)";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@Ticker", this.textBoxTicker.Text);
                command.Parameters.AddWithValue("@Price", decimal.Parse(this.textBoxPrice.Text));
                command.ExecuteScalar();
            }

            this.textBoxPrice.Clear();
            this.textBoxTicker.Clear();
        }
    }
}
