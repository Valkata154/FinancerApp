/*
 * Class for the creation of Tickers.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financer
{
    public class Ticker
    {
        private int id;
        private string name;
        private double currPrice;
        private double openPrice;
        private string volume;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

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
