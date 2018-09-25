using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Gen1
{
    public class FinancialData
    {
        public double High { get; set; }
        public double Low { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public DateTime Date { get; set; }
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            GenerateChart("1m");
        }

        private async void GenerateChart(string duration)
        {
            this.OhlcChart.DataContext = "";
            this.secondChart.DataContext = "";

            string ticker = ((ComboBoxItem)this.TickerSelector.SelectedItem).Content.ToString().ToLower();

            List<FinancialData> newData = new List<FinancialData>();
            List<RootObject> stockData = await IEXDataProxy.GetData(duration, ticker);

            foreach (var dataPoint in stockData)
            {
                DateTime nextDate = new DateTime();

                if(DateTime.TryParse(dataPoint.date, out nextDate))
                {
                    newData.Add(new FinancialData()
                    {
                        High = dataPoint.high,
                        Open = dataPoint.open,
                        Low = dataPoint.low,
                        Close = dataPoint.close,
                        Date = nextDate
                    });
                }
                else
                {
                    newData.Add(new FinancialData()
                    {
                        High = dataPoint.high,
                        Open = dataPoint.open,
                        Low = dataPoint.low,
                        Close = dataPoint.close,
                        Date = DateTime.Today
                    });
                }
            }

            this.OhlcChart.DataContext = newData;
            this.secondChart.DataContext = newData;
        }

        private void Button_1_Day_Click(object sender, RoutedEventArgs e)
        {   this.GenerateChart("1d");   }

        private void Button_1_Month_Click(object sender, RoutedEventArgs e)
        {   this.GenerateChart("1m");   }

        private void Button_3_Month_Click(object sender, RoutedEventArgs e)
        {   this.GenerateChart("3m");   }

        private void Button_6_Month_Click(object sender, RoutedEventArgs e)
        {   this.GenerateChart("6m");   }

        private void Button_YTD_Click(object sender, RoutedEventArgs e)
        {   this.GenerateChart("ytd");  }

        private void Button_1_Year_Click(object sender, RoutedEventArgs e)
        {   this.GenerateChart("1y");   }

        private void Button_2_Year_Click(object sender, RoutedEventArgs e)
        {   this.GenerateChart("2y");   }

        private void Button_5_Year_Click(object sender, RoutedEventArgs e)
        {   this.GenerateChart("5y");   }

    }
}
