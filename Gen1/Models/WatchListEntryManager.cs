using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using System.Collections.ObjectModel;

namespace Gen1.Models
{
    class WatchListEntryManager
    {
        public static async Task<ObservableCollection<WatchListEntry>> GetWatchListEntries()
        {
            var watchListEntries = new ObservableCollection<WatchListEntry>();

            List<OneDayData> AAPLdata = await IEXDataProxy.GetOneDayData("aapl");
            //List<OneDayData> MSFTdata = await IEXDataProxy.GetOneDayData("msft");
            //List<OneDayData> NFLXdata = await IEXDataProxy.GetOneDayData("nflx");
            //List<OneDayData> NKEdata = await IEXDataProxy.GetOneDayData("nke");

            watchListEntries.Add(new WatchListEntry
            {
                Symbol = "AAPL",
                Price = String.Format("{0:0.##}", AAPLdata[AAPLdata.Count - 1].close),
                PriceChange = String.Format("{0:0.##}", AAPLdata[AAPLdata.Count - 1].close - AAPLdata[0].open),
                PercentChange = String.Format("{0:0.##}", (AAPLdata[AAPLdata.Count - 1].close / (AAPLdata[AAPLdata.Count - 1].close - AAPLdata[0].open)) * 100),
                TotalVolume = String.Format("{0} K", AAPLdata.Sum(item => item.volume))
            });

            /*
            watchListEntries.Add(new WatchListEntry
            {
                Symbol = "MSFT",
                Price = MSFTdata[MSFTdata.Count - 1].close,
                PriceChange = MSFTdata[MSFTdata.Count - 1].close - MSFTdata[0].open,
                PercentChange = String.Format("{0:0.##}", (MSFTdata[MSFTdata.Count - 1].close / (MSFTdata[MSFTdata.Count - 1].close - MSFTdata[0].open)) * 100),
                TotalVolume = String.Format("{0} K", MSFTdata.Sum(item => item.volume))
            });

            watchListEntries.Add(new WatchListEntry
            {
                Symbol = "NFLX",
                Price = NFLXdata[NFLXdata.Count - 1].close,
                PriceChange = NFLXdata[NFLXdata.Count - 1].close - NFLXdata[0].open,
                PercentChange = String.Format("{0:0.##}", (NFLXdata[NFLXdata.Count - 1].close / (NFLXdata[NFLXdata.Count - 1].close - NFLXdata[0].open)) * 100),
                TotalVolume = String.Format("{0} K", NFLXdata.Sum(item => item.volume))
            });

            watchListEntries.Add(new WatchListEntry
            {
                Symbol = "NKE",
                Price = NKEdata[NKEdata.Count - 1].close,
                PriceChange = NKEdata[NKEdata.Count - 1].close - NKEdata[0].open,
                PercentChange = String.Format("{0:0.##}", (NKEdata[AAPLdata.Count - 1].close / (NKEdata[NKEdata.Count - 1].close - NKEdata[0].open)) * 100),
                TotalVolume = String.Format("{0} K", NKEdata.Sum(item => item.volume))
            });
            */

           // watchListEntries.Add(new WatchListEntry { Symbol = "MSFT", Price = 99.09, PriceChange = -0.76, PercentChange = String.Format("{0:0.##}", (-0.76 / 99.09) * 100), TotalVolume = "6543 K" });
           // watchListEntries.Add(new WatchListEntry { Symbol = "NFLX", Price = 64.64, PriceChange = 1.20, PercentChange = String.Format("{0:0.##}", (1.20 / 64.64) * 100), TotalVolume = "123 K" });
           // watchListEntries.Add(new WatchListEntry { Symbol = "NKE", Price = 120.34, PriceChange = -1.10, PercentChange = String.Format("{0:0.##}", (-1.10 / 120.34) * 100), TotalVolume = "111222 K" });

            return watchListEntries;
        }

    }
}
