namespace API_ERP.DataModels
{
    public class MRPDataModel
    {
        public int[] GrossRequirements { get; set; } = new int[10];
        public int[] SheduledReceipts { get; set; } = new int[10];
        public int[] ProjectedOnHand { get; set; } = new int[10];
        public int[] NetRequirements { get; set; } = new int[10];
        public int[] PlannedReceipt { get; set; } = new int[10];
        public int[] PlannedRelease { get; set; } = new int[10];
        public int[] Orders { get; set; } = new int[10];

        public int RealizationTime { get; set; } = 0;
        public int LotSize { get; set; } = 0;
        public int BOM { get; set; } = 0;
        public int StartingInventory { get; set; } = 0;
        public bool AutoPlanning { get; set; } = false;
    }
}
