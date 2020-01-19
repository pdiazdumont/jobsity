namespace Jobsity.Bots.Stock.Api.Model
{
    public class StockInformation
    {
        public StockInformation()
        {
            
        }

        public string Symbol { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }
    }
}
