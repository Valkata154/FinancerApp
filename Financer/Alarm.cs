using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financer
{
    class Alarm
    {
        private int id;
        private string ticker;
        private double price;
        private Boolean notifiedAbove;
        private Boolean notifiedBellow;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Ticker
        {
            get { return ticker; }
            set { ticker = value; }
        }

        public double Price
        {
            get { return price; }
            set { price = value; }
        }

        public Boolean NotifiedAbove
        {
            get { return notifiedAbove; }
            set { notifiedAbove = value; }
        }

        public Boolean NotifiedBellow
        {
            get { return notifiedBellow; }
            set { notifiedBellow = value; }
        }
    }
}
