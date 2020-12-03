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
using YahooFinanceApi;
using System.Timers;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace Financer
{
    /// <summary>
    /// Interaction logic for AlarmWindow.xaml
    /// </summary>
    public partial class AlarmWindow : Window
    {
        private string connectionString;
        private SqlConnection connection;
        private List<Alarm> alarmsList = new List<Alarm>();
        private static Timer aTimer;
        private string prevString = "";
        private int iteration = 1;

        public AlarmWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString;
            PopulateData();
            StartTimer();
        }

        private void OpenAddAlarmWindow(object sender, RoutedEventArgs e)
        {
            AddAlarmWindow addAlarmWindow = new AddAlarmWindow();
            addAlarmWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            addAlarmWindow.Show();
            this.Close();
        }

        public void PopulateData()
        {
            using (connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Alarm", connection))
            {
                DataTable alarmTable = new DataTable();
                this.alarmListView.Items.Clear();
                alarmsList.Clear();
                adapter.Fill(alarmTable);
                foreach (DataRow row in alarmTable.Rows)
                {
                    Alarm alarm = new Alarm();
                    alarm.Id = row.Field<int>(0);
                    alarm.Ticker = row.Field<string>(1);
                    alarm.Price = row.Field<double>(2);
                    alarm.NotifiedAbove = false;
                    alarm.NotifiedBellow = false;
                    this.alarmListView.Items.Add(alarm);
                    alarmsList.Add(alarm);
                }
            }
        }

        public void DeleteAlarm(object sender, RoutedEventArgs e)
        {
            Alarm alarm = (Alarm)alarmListView.SelectedItem;
            string query = "DELETE FROM Alarm WHERE Id=@Id";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@Id", alarm.Id);
                command.ExecuteScalar();
            }
            alarmListView.Items.Remove(alarmListView.SelectedItem);  
        }

        public void SendMessage(string messageBody)
        {
            var accountSid = "Sid";
            var authToken = "Token";

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: messageBody,
                from: new Twilio.Types.PhoneNumber("whatsapp:+14155238886"),
                to: new Twilio.Types.PhoneNumber("whatsapp:+359895730322")
            );
        }

        private void StartTimer()
        {
            aTimer = new Timer(2000);
            aTimer.Elapsed += UpdatePrices;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private async void UpdatePrices(Object source, ElapsedEventArgs e)
        {
            foreach (Alarm alarm in alarmsList)
            {
                var securities = await Yahoo.Symbols(alarm.Ticker).Fields(Field.RegularMarketPrice).QueryAsync();
                double currPrice = securities[alarm.Ticker].RegularMarketPrice;
                string aboveMessage = alarm.Ticker + " " + currPrice.ToString("#.##") + " is ABOVE " + alarm.Price + " alarm.";
                string bellowMessage = alarm.Ticker + " " + currPrice.ToString("#.##") + " is BELLOW " + alarm.Price + " alarm.";

                if (currPrice >= alarm.Price && !alarm.NotifiedAbove && !(prevString.Equals(aboveMessage)))
                {
                    if(iteration != 1)
                        SendMessage(aboveMessage);
                    prevString = aboveMessage;
                    alarm.NotifiedAbove = true;
                    alarm.NotifiedBellow = false;
                }
                else if (currPrice < alarm.Price && !alarm.NotifiedBellow &&!(prevString.Equals(bellowMessage)))
                {
                    if (iteration != 1)
                        SendMessage(bellowMessage);
                    prevString = bellowMessage;
                    alarm.NotifiedAbove = false;
                    alarm.NotifiedBellow = true;
                }
            }
            iteration++;
        }
    }
}
