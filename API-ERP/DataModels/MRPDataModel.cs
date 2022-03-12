namespace API_ERP.DataModels
{
    public class MRPDataModel
    {
        public List<int> GrossRequirements { get; set; }
        public List<int> SheduledReceipts { get; set; }
        public List<int> ProjectedOnHand { get; set; }
        public List<int> NetRequirements { get; set; }
        public List<int> PlannedReceipt { get; set; }
        public List<int> PlannedRelease { get; set; }
        public int RealizationTime { get; set; }
        public int LotSize { get; set; }
        public int BOM { get; set; }
        public int StartingInventory { get; set; }
    }
}
