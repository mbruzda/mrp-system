namespace API_ERP.Interfaces
{
    public abstract class ERP
    {
        public int[] FillGrossRequirementsTable(int[] GrossRequirementsTable, int[] SupplyTTable, int mulitplier = 1)
        {
            for (int i = 0; i < 10; i++) GrossRequirementsTable[i] = SupplyTTable[i] * mulitplier;
            return GrossRequirementsTable;
        }

    }
}
