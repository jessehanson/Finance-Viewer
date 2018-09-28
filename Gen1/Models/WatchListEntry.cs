using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gen1.Models
{
    class WatchListEntry : INotifyPropertyChanged
    {
        private string _symbol;
        private string _price;
        private string _priceChange;
        private string _percentChange;
        private string _totalVolume;

        public string Symbol {
            get { return _symbol; }
            set
            {
                _symbol = value;
                OnPropertyChanged("Symbol");
            }
        }

        public string Price {
            get { return _price; }
            set
            {
                _price = value;
                OnPropertyChanged("Price");
            }
        }

        public string PriceChange {
            get { return _priceChange; }
            set
            {
                _priceChange = value;
                OnPropertyChanged("PriceChange");
            }
        }

        public string PercentChange {
            get { return _percentChange; }
            set
            {
                _percentChange = value;
                OnPropertyChanged("PercentChange");
            }
        }

        public string TotalVolume {
            get { return _totalVolume; }
            set
            {
                _totalVolume = value;
                OnPropertyChanged("TotalVolume");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
