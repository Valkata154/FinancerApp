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
    /// <summary>
    /// Interaction logic for WatchlistWindow.xaml
    /// </summary>
    public partial class WatchlistWindow : Window
    {
        private static System.Timers.Timer aTimer;
        private List<Ticker> tickerList = new List<Ticker>();
        private double prevPrice;
        private double currPrice;
        private double openPrice;
        public WatchlistWindow()
        {
            InitializeComponent();
            SetTimer();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            aTimer.Stop();
        }

        private void openAddTicker(object sender, RoutedEventArgs e)
        {
            if (!this.txtBoxTicker.IsVisible)
            {
                this.txtBoxTicker.Visibility = Visibility.Visible;
                this.txtBoxTicker.Focus();
            }
            else
            {
                if (!(this.txtBoxTicker.Text == ""))
                    getTicker(this.txtBoxTicker.Text);
                this.txtBoxTicker.Visibility = Visibility.Hidden;
                this.txtBoxTicker.Text = "";
            }
        }

        private async void getTicker(string name)
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

        private void SetTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(2000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private async void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.tickerListView.Items.Clear();
            });
            foreach (Ticker ticker in tickerList)
            {
                var securities = await Yahoo.Symbols(ticker.Name).Fields(Field.RegularMarketPrice).QueryAsync();
                prevPrice = ticker.CurrPrice;
                openPrice = ticker.OpenPrice;
                ticker.CurrPrice = securities[ticker.Name].RegularMarketPrice + 1;
                this.Dispatcher.Invoke(() =>
                {
                    currPrice = ticker.CurrPrice;
                    this.tickerListView.Items.Add(ticker);
                });
            }
        }
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

        private  string FormatNumber(long num)
        {
            long i = (long)Math.Pow(10, (int)Math.Max(0, Math.Log10(num) - 2));
            num = num / i * i;

            if (num >= 1000000000)
                return (num / 1000000000D).ToString("0.##") + "B";
            if (num >= 1000000)
                return (num / 1000000D).ToString("0.##") + "M";
            if (num >= 1000)
                return (num / 1000D).ToString("0.##") + "K";

            return num.ToString("#,0");
        }
    }
}
