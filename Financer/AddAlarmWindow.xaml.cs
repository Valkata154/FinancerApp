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
using YahooFinanceApi;

namespace Financer
{
    /// <summary>
    /// Interaction logic for AddAlarmWindow.xaml
    /// </summary>
    public partial class AddAlarmWindow : Window
    {
        private string connectionString;
        private SqlConnection connection;

        // When closing the window, initialise a window with the tickers.
        private void Window_Closed(object sender, EventArgs e)
        {
            AlarmWindow win = new AlarmWindow();
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.PopulateData();
            win.Show();
        }

        // Init add alarm window.
        public AddAlarmWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString;
        }

        // Method to add alarms.
        private async void AddAlarm(object sender, RoutedEventArgs e)
        {
            string query = "INSERT INTO Alarm VALUES (@Ticker, @Price)";

            try
            {
                // Used for checking if the symbol is valid, else show message box.
                var securities = await Yahoo.Symbols(this.textBoxTicker.Text).Fields(Field.Symbol, Field.RegularMarketPrice, Field.RegularMarketVolume, Field.RegularMarketOpen, Field.FiftyTwoWeekHigh).QueryAsync();
                var check = securities[this.textBoxTicker.Text];
                var price = check.RegularMarketVolume;

                bool alreadyExists = false;

                // Used to check if the alarm already exists, else show message box.
                using (connection = new SqlConnection(connectionString))
                using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Alarm", connection))
                {
                    DataTable alarmTable = new DataTable();
                    adapter.Fill(alarmTable);
                    foreach (DataRow row in alarmTable.Rows)
                    {
                        if (this.textBoxTicker.Text.Equals(row.Field<string>(1)) && double.Parse(this.textBoxPrice.Text) == row.Field<double>(2))
                            alreadyExists = true;
                    }
                }

                // Add the alarm.
                if(!alreadyExists)
                {
                    using (connection = new SqlConnection(connectionString))
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@Ticker", this.textBoxTicker.Text);
                        command.Parameters.AddWithValue("@Price", decimal.Parse(this.textBoxPrice.Text));
                        command.ExecuteScalar();
                    }
                }
                else
                {
                    MessageBox.Show("Alarm for this target already exists.");
                }
            }
            catch
            {
                MessageBox.Show("Invalid Ticker symbol.");
            }

            this.textBoxPrice.Clear();
            this.textBoxTicker.Clear();
        }
    }
}
