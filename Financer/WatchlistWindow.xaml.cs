using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using System.Windows.Shapes;
using YahooFinanceApi;
using System.Timers;

namespace Financer
{
    public partial class WatchlistWindow : Window
    {
        // Variables
        private static System.Timers.Timer aTimer;
        private List<Ticker> tickerList = new List<Ticker>();
        private double prevPrice;
        private double currPrice;
        private double openPrice;

        // Initialize the component and start the timer.
        public WatchlistWindow()
        {
            InitializeComponent();
            StartTimer();
        }

        // OnClosed method to stop the timer thread.
        private void Window_Closed(object sender, EventArgs e)
        {
            aTimer.Stop();
        }

        // Method to toggle visibility on the TextBox and pass the ticker name to be added.
        private void OpenAddTicker(object sender, RoutedEventArgs e)
        {
            if (!this.txtBoxTicker.IsVisible)
            {
                this.txtBoxTicker.Visibility = Visibility.Visible;
                this.txtBoxTicker.Focus();
            }
            else
            {
                if (!(this.txtBoxTicker.Text == ""))
                    GetTicker(this.txtBoxTicker.Text);
                this.txtBoxTicker.Visibility = Visibility.Hidden;
                this.txtBoxTicker.Text = "";
            }
        }

        // Method to create the ticker from YahooFinance and add it to a list.
        private async void GetTicker(string name)
        {
            var securities = await Yahoo.Symbols(name).Fields(Field.Symbol, Field.RegularMarketPrice, Field.RegularMarketVolume, Field.RegularMarketOpen, Field.FiftyTwoWeekHigh).QueryAsync();
            var aapl = securities[name];
            Ticker ticker = new Ticker();
            ticker.Name = name;
            ticker.CurrPrice = aapl.RegularMarketPrice;
            ticker.OpenPrice = aapl.RegularMarketOpen;
            ticker.Volume = FormatNumber(aapl.RegularMarketVolume);
            tickerList.Add(ticker);
            this.tickerListView.Items.Add(ticker);
        }

        // Method to start the 2sec timer and add the function.
        private void StartTimer()
        {
            aTimer = new System.Timers.Timer(2000);
            aTimer.Elapsed += UpdatePrices;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        // Method to update the prices of the tickers in the list.
        private async void UpdatePrices(Object source, ElapsedEventArgs e)
        {
            // Allows using the thread for multiple purposes.
            this.Dispatcher.Invoke(() =>
            {
                this.tickerListView.Items.Clear();
            });
            // For each ticker update the price, reset the listview and add the tickers again.
            foreach (Ticker ticker in tickerList)
            {
                var securities = await Yahoo.Symbols(ticker.Name).Fields(Field.RegularMarketPrice).QueryAsync();
                prevPrice = ticker.CurrPrice;
                openPrice = ticker.OpenPrice;
                ticker.CurrPrice = securities[ticker.Name].RegularMarketPrice;
                this.Dispatcher.Invoke(() =>
                {
                    currPrice = ticker.CurrPrice;
                    this.tickerListView.Items.Add(ticker);
                });
            }
        }
        // Method to color the price based on the price from 2 secs ago. Red if lower and green if above the previous 2sec price.
        private void ColorPrice(object sender, RoutedEventArgs e)
        {
            if (prevPrice < currPrice)
                (sender as TextBlock).Foreground = Brushes.LimeGreen;
            if(prevPrice > currPrice)
                (sender as TextBlock).Foreground = Brushes.Red;
            if(prevPrice == currPrice)
                (sender as TextBlock).Foreground = Brushes.White;
            currPrice = 0;
            prevPrice = 0;
            openPrice = 0;
        }

        // Method to color the name. Red if the price is lower than the open price, green if above the open price.
        private void ColorName(object sender, RoutedEventArgs e)
        {
            if (openPrice < currPrice)
                (sender as TextBlock).Foreground = Brushes.LimeGreen;
            if (openPrice > currPrice)
                (sender as TextBlock).Foreground = Brushes.Red;
            if (openPrice == currPrice)
                (sender as TextBlock).Foreground = Brushes.White;
            openPrice = 0;
        }

        // Method to format the volume into millions and hundreds.
        private string FormatNumber(long num)
        {
            long i = (long)Math.Pow(10, (int)Math.Max(0, Math.Log10(num) - 2));
            num = num / i * i;

            if (num >= 1000000000)
                return (num / 1000000000D).ToString("0.##") + " B";
            if (num >= 1000000)
                return (num / 1000000D).ToString("0.##") + " M";
            if (num >= 1000)
                return (num / 1000D).ToString("0.##") + " K";

            return num.ToString("#,0");
        }
    }
}
