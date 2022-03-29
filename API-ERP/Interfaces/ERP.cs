﻿namespace API_ERP.Interfaces
{
    public class ERP
    {
        public int[] FillGrossRequirements(int[] GrossRequirementsTable, int[] SupplyTTable, int mulitplier = 1)
        {
            for (int i = 0; i < 10; i++) GrossRequirementsTable[i] = SupplyTTable[i] * mulitplier;
            return GrossRequirementsTable;
        }


        public int[] FillProjectedOnHand(int[] ProjectedOnHand, int[] GrossRequirements,int[] SheduledReceipts, int StartingInventory)
        {
            ProjectedOnHand[0] = StartingInventory - GrossRequirements[0] + SheduledReceipts[0];
            for (int i = 1; i < 10; i++) ProjectedOnHand[i] = ProjectedOnHand[i - 1] - GrossRequirements[i] + SheduledReceipts[i];

            return ProjectedOnHand;
        }
    }
}
