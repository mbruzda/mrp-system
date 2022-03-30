namespace API_ERP
{
    public class GHPDataModel
    {
        public int[] SalesForecast { get; set; } = new int[10];
        public int[] Production { get; set; } = new int[10];
        public int[] Inventory { get; set; } = new int[10];
        public int[] Orders { get; set; } = new int[10];

        public int RealizationTime { get; set; } = 0;
        public int StartingInventory { get; set; } = 0;

    }   
}
