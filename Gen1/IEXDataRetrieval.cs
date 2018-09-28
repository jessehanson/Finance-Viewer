using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Gen1
{
    public class IEXDataProxy
    {
        public async static Task<List<RootObject>> GetData(string duration, string ticker)
        {
            var http = new HttpClient();
            string URI = String.Format("https://api.iextrading.com/1.0/stock/{0}/chart/{1}", ticker, duration);
            var response = await http.GetAsync(URI);
            var result = await response.Content.ReadAsStringAsync();

            // remove all instances of null
            //result = result.Replace("null", "0.0");

            var serializer = new DataContractJsonSerializer(typeof(List<RootObject>));

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (List<RootObject>)serializer.ReadObject(ms);

            return data;
        }

        public async static Task<List<OneDayData>> GetOneDayData(string ticker)
        {
            var http = new HttpClient();
            string URI = String.Format("https://api.iextrading.com/1.0/stock/{0}/chart/1d", ticker);
            var response = await http.GetAsync(URI);
            var result = await response.Content.ReadAsStringAsync();

            // remove all instances of null
            result = result.Replace("null", "0.0");

            var serializer = new DataContractJsonSerializer(typeof(List<OneDayData>));

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));

            return (List<OneDayData>)serializer.ReadObject(ms);
        }
    }

    [DataContract]
    public class RootObject
    {
        [DataMember]
        public string date { get; set; }

        [DataMember]
        public double open { get; set; }

        [DataMember]
        public double high { get; set; }

        [DataMember]
        public double low { get; set; }

        [DataMember]
        public double close { get; set; }

        [DataMember]
        public int volume { get; set; }

        [DataMember]
        public int unadjustedVolume { get; set; }

        [DataMember]
        public double change { get; set; }

        [DataMember]
        public double changePercent { get; set; }

        [DataMember]
        public double vwap { get; set; }

        [DataMember]
        public string label { get; set; }

        [DataMember]
        public double changeOverTime { get; set; }
    }

    [DataContract]
    public class OneDayData
    {
        public string date { get; set; }

        [DataMember]
        public string minute { get; set; }

        [DataMember]
        public string label { get; set; }

        [DataMember]
        public double high { get; set; }

        [DataMember]
        public double low { get; set; }

        [DataMember]
        public double average { get; set; }

        [DataMember]
        public int volume { get; set; }

        [DataMember]
        public double notional { get; set; }

        [DataMember]
        public int numberOfTrades { get; set; }

        [DataMember]
        public double marketHigh { get; set; }

        [DataMember]
        public double marketLow { get; set; }

        [DataMember]
        public double marketAverage { get; set; }

        [DataMember]
        public int marketVolume { get; set; }

        [DataMember]
        public double marketNotional { get; set; }

        [DataMember]
        public int marketNumberOfTrades { get; set; }

        [DataMember]
        public double open { get; set; }

        [DataMember]
        public double close { get; set; }

        [DataMember]
        public double marketOpen { get; set; }

        [DataMember]
        public double marketClose { get; set; }

        [DataMember]
        public double changeOverTime { get; set; }

        [DataMember]
        public double marketChangeOverTime { get; set; }
    }

    /* This is for the original IEX data way
    public class IEXDataRetrieval
    {
        public String getData()
        {
            var symbol = "msft";
            var IEXTrading_API_PATH = "https://api.iextrading.com/1.0/stock/{0}/chart/1y";

            IEXTrading_API_PATH = string.Format(IEXTrading_API_PATH, symbol);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                String returnString = "";

                //For IP-API
                client.BaseAddress = new Uri(IEXTrading_API_PATH);
                HttpResponseMessage response = client.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    var historicalDataList = response.Content.ReadAsAsync<List<HistoricalDataResponse>>().GetAwaiter().GetResult();
                    foreach (var historicalData in historicalDataList)
                    {
                        if (historicalData != null)
                        {
                            returnString += ("Open: " + historicalData.open + "\n");
                            returnString += ("Close: " + historicalData.close + "\n");
                            returnString += ("Low: " + historicalData.low + "\n");
                            returnString += ("High: " + historicalData.high + "\n");
                            returnString += ("Change: " + historicalData.change + "\n");
                            returnString += ("Change Percentage: " + historicalData.changePercent + "\n");

                            /*
                            Console.WriteLine("Open: " + historicalData.open);
                            Console.WriteLine("Close: " + historicalData.close);
                            Console.WriteLine("Low: " + historicalData.low);
                            Console.WriteLine("High: " + historicalData.high);
                            Console.WriteLine("Change: " + historicalData.change);
                            Console.WriteLine("Change Percentage: " + historicalData.changePercent);
                            
                        }
                    }

                    return returnString;
                }
                else
                    return "fail";
            }
        }

    }
    

    public class HistoricalDataResponse
    {
        public string date { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double close { get; set; }
        public int volume { get; set; }
        public int unadjustedVolume { get; set; }
        public double change { get; set; }
        public double changePercent { get; set; }
        public double vwap { get; set; }
        public string label { get; set; }
        public double changeOverTime { get; set; }
    }
    */

}
