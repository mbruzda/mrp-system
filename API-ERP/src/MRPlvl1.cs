using API_ERP.DataModels;
using Newtonsoft.Json;

namespace API_ERP
{
    public class MRPlvl1 : IERP
    {
        public GHPDataModel GHPData;
        public MRPDataModel MRPData;

        public void SetDataFromJson(string jsonString, int RT = 0, int LS = 0, int BOM = 0, int SI = 0)
        {
            var GHPData = JsonConvert.DeserializeObject<GHPDataModel>(jsonString);
            if (!GHPData.SalesForecast.Any()) throw new Exception("Error parsing the table");
            this.GHPData = GHPData;

            MRPData.RealizationTime = RT;
            MRPData.LotSize = LS;
            MRPData.BOM = BOM;
            MRPData.StartingInventory = SI;
        }

        public void FillTable()
        {
            var finish = false;
            while (!finish)
            {
                //GrossRequirements
                for (int i = MRPData.RealizationTime; i < 10; i++) MRPData.GrossRequirements[i - MRPData.BOM] = GHPData.SalesForecast[i];
                //ProjectedOnHand
                MRPData.ProjectedOnHand[0] = MRPData.StartingInventory; for (int i = 1; i < 10; i++) MRPData.ProjectedOnHand[i] = MRPData.ProjectedOnHand[i - 1] - MRPData.GrossRequirements[i];
                //NetRequirements
                for (int i = 0; i < 10; i++)
                {
                    if (MRPData.ProjectedOnHand[i] < 0)
                    {
                        MRPData.NetRequirements[i] = MRPData.ProjectedOnHand[i] * -1;
                        break;
                    }
                    if (i == 9) finish = true;
                }

                for (int i = 0; i < 10; i++)
                {
                    if (MRPData.NetRequirements[i] > 0)
                    {
                        if (i>MRPData.RealizationTime) //if this will be true we can start a production, otherwise we need to place an order
                        {
                            //PlannedRelease
                            for (int x = 0; x < 10; x++) if (MRPData.NetRequirements[i] > 0) MRPData.PlannedRelease[i] = MRPData.LotSize > MRPData.NetRequirements[i] ? MRPData.LotSize : MRPData.LotSize * 2;
                            //PlannedReceipt
                            for (int x = 0; x < 10; x++) if (MRPData.PlannedRelease[i] > 0) MRPData.PlannedReceipt[i - MRPData.RealizationTime] = MRPData.PlannedRelease[i];
                        }
                        else
                        {
                            //SheduledReceipts
                            MRPData.SheduledReceipts[i] = MRPData.GrossRequirements[i]-MRPData.ProjectedOnHand[i];
                            MRPData.ProjectedOnHand[i] = 0;
                        }

                    }
                }
                //ProjectedOnHandCorrection
                for (int i = 1; i < 10; i++) MRPData.ProjectedOnHand[i] = MRPData.ProjectedOnHand[i - 1] - MRPData.GrossRequirements[i] + MRPData.PlannedRelease[i];
            }
        }

        public string DataToJson()
        {
            return JsonConvert.SerializeObject(this.MRPData);
        }
    }
}