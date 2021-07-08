using System;
using YahooFinanceApi;
using System.Threading.Tasks;
using System.Linq;
namespace StockHistData
{
    class Program
    {
        static void Main(string[] args)
        {
            char continueStr = 'y';
            while(continueStr=='y')
            {
                Console.WriteLine("Enter ticker: ");
                string ticker = Console.ReadLine().ToUpper();
                Console.WriteLine("Amount of months: ");
                int time = Convert.ToInt32(Console.ReadLine());
                DateTime end = DateTime.Today;
                DateTime start = DateTime.Today.AddMonths(-time);
                Data stock = new Data();
                var awaiter = stock.getStockData(ticker, start, end);
                if(awaiter.Result==1)
                {
                    Console.WriteLine();
                    Console.WriteLine("Another ticker? [y/n]: ");
                    continueStr = Convert.ToChar(Console.ReadLine());
                }
            }
            Console.WriteLine();
            Console.WriteLine(":)");
        }
    }
    class Data
    {
        public async Task<int> getStockData(string ticker, DateTime start, DateTime end)
        {
            try
            {
                var historicalData = await Yahoo.GetHistoricalAsync(ticker, start, end);
                var security = await Yahoo.Symbols(ticker).Fields(Field.LongName).QueryAsync();
                var symbol = security[ticker];
                var corpName = symbol[Field.LongName];
                for(int i=0; i<historicalData.Count; i++)
                {
                    Console.WriteLine(corpName + "Closing price: " + historicalData.ElementAt(i).DateTime.Month + "/" + historicalData.ElementAt(i).DateTime.Year + ": $" + Math.Round(historicalData.ElementAt(i).Close, 2));
                }
            }
            catch
            {
                Console.WriteLine(":(");
            }
            return 1;
        }
    }
}
