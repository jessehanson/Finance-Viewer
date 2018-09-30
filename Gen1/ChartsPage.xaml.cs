using System;
using System.Collections.Generic;
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
using Gen1.Models;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI;
using Gen1;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Gen1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChartsPage : Page
    {
        // Class variables
        private ObservableCollection<WatchListEntry> watchListEntries;
        public static string watchListHighlightedEntry;
        Color WatchListBackgroundColor = Colors.Black;
        Color WatchListSelectionColor = Colors.DarkGray;


        // Page Constructor
        public ChartsPage()
        {
            this.InitializeComponent();
            GenerateChart("1m");
            CollapseAndExpandWatchListButton.Content = "<";

            watchListEntries = new ObservableCollection<WatchListEntry>();

            watchListHighlightedEntry = "";
        }

        public async void AddEntry(string ticker)
        {
            List<OneDayData> stockData = await IEXDataProxy.GetOneDayData(ticker);

            watchListEntries.Add(new WatchListEntry
            {
                Symbol = ticker.ToUpper(),
                Price = String.Format("{0:0.##}", stockData[stockData.Count - 1].close),
                PriceChange = String.Format("{0:0.##}", stockData[stockData.Count - 1].close - stockData[0].open),
                PercentChange = String.Format("{0:0.##}", ((stockData[stockData.Count - 1].close - stockData[0].open) / stockData[0].open) * 100),
                TotalVolume = String.Format("{0:0.#} K", stockData.Sum(item => item.volume) / 1000)
            });
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

                if (DateTime.TryParse(dataPoint.date, out nextDate))
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
        { this.GenerateChart("1d"); }

        private void Button_1_Month_Click(object sender, RoutedEventArgs e)
        { this.GenerateChart("1m"); }

        private void Button_3_Month_Click(object sender, RoutedEventArgs e)
        { this.GenerateChart("3m"); }

        private void Button_6_Month_Click(object sender, RoutedEventArgs e)
        { this.GenerateChart("6m"); }

        private void Button_YTD_Click(object sender, RoutedEventArgs e)
        { this.GenerateChart("ytd"); }

        private void Button_1_Year_Click(object sender, RoutedEventArgs e)
        { this.GenerateChart("1y"); }

        private void Button_2_Year_Click(object sender, RoutedEventArgs e)
        { this.GenerateChart("2y"); }

        private void Button_5_Year_Click(object sender, RoutedEventArgs e)
        { this.GenerateChart("5y"); }

        private async void CollapseAndExpandWatchListButton_Click(object sender, RoutedEventArgs e)
        {
            if (!WatchListSplitView.IsPaneOpen)
            {
                CollapseAndExpandWatchListButton.Content = ">";

                // re-populate the watchlist with new data
                // watchListEntries = await WatchListEntryManager.GetWatchListEntries();

                WatchListSplitView.IsPaneOpen = true;
            }
            else
            {
                CollapseAndExpandWatchListButton.Content = "<";
                WatchListSplitView.IsPaneOpen = false;
            }
        }

        private void UpdateWatchList_Click(object sender, RoutedEventArgs e)
        {
            watchListEntries[0].Price = "-99";
        }

        private void WatchListAddTickerButton_Click(object sender, RoutedEventArgs e)
        {
            string ticker = TickerInput.Text.ToLower();

            AddEntry(ticker);
        }

        private void WatchListListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var watchListEntry = (WatchListEntry)e.ClickedItem;

            foreach (var item in WatchListListView.Items)
            {
                var watchListItem = item as WatchListEntry;
                var container = WatchListListView.ContainerFromItem(watchListItem) as ListViewItem;
                var ItemGridView = container.ContentTemplateRoot as Grid;

                if (watchListItem.Symbol.ToLower() == watchListEntry.Symbol.ToLower())
                {
                    SolidColorBrush brush = ItemGridView.Background as SolidColorBrush;
                    if (brush != null)
                    {
                        if (brush.Color == WatchListSelectionColor)
                            ItemGridView.Background = null;
                        else
                            ItemGridView.Background = new SolidColorBrush(WatchListSelectionColor);
                    }
                    else
                        ItemGridView.Background = new SolidColorBrush(WatchListSelectionColor);
                }
                else
                    ItemGridView.Background = null;
            }
        }

        private void RemoveWatchListSelection_Click(object sender, RoutedEventArgs e)
        {
            WatchListEntry entryToBeRemoved = null;

            foreach (var item in WatchListListView.Items)
            {
                var watchListItem = item as WatchListEntry;
                var container = WatchListListView.ContainerFromItem(watchListItem) as ListViewItem;
                var ItemGridView = container.ContentTemplateRoot as Grid;

                SolidColorBrush brush = ItemGridView.Background as SolidColorBrush;
                if (brush != null)
                {
                    if(brush.Color == WatchListSelectionColor)
                    {
                        ItemGridView.Background = null;
                        entryToBeRemoved = watchListItem;
                    }
                }
            }

            if (entryToBeRemoved != null)
                watchListEntries.Remove(entryToBeRemoved);
        }
    }

    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double testValue;
            if (double.TryParse(((WatchListEntry)value).PriceChange, out testValue))
            {
                if (testValue > 0.0)
                    return new SolidColorBrush(Colors.Green);
                else if (testValue == 0)
                    return new SolidColorBrush(Colors.White);
                else if (testValue < 0.0)
                    return new SolidColorBrush(Colors.Red);
                else
                    throw new Exception();
            }
            else
                throw new Exception();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new Exception();
        }
    }

    /*
    public class BackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (((WatchListEntry)value).Symbol.ToLower() == ChartsPage.watchListHighlightedEntry[0].ToLower())
                return new SolidColorBrush(Colors.Pink);
            else
                return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new Exception();
        }
    }
    */
}

