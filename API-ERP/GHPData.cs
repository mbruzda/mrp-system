namespace API_ERP
{
    public class GHPData
    {
        public List<int> SalesForecast { get; set; }
        public List<int> Production { get; set; }
        public List<int> Inventory { get; set; }
        public int RealizationTime { get; set; }
        public int StartingInventory { get; set; }
    }   
}
