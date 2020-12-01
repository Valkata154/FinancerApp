using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YahooFinanceApi;

namespace Financer
{
    public class Ticker
    {
        private string name;
        private double currPrice;
        private double openPrice;
        private string volume;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public double CurrPrice
        {
            get { return currPrice; }
            set { currPrice = value; }
        }

        public double OpenPrice
        {
            get { return openPrice; }
            set { openPrice = value; }
        }

        public string Volume
        {
            get { return volume; }
            set { volume = value; }
        }
    }
}
