using API_ERP.DataModels;
using Newtonsoft.Json;

namespace API_ERP
{
    public class MRPlvl2 : IERP
    {
        private MRPDataModel _MRPDatalvl1;
        private MRPDataModel _MRPDatalvl2;
        private int _multiplier;

        public MRPlvl2()
        {
            _MRPDatalvl1 = new MRPDataModel();
            _MRPDatalvl2 = new MRPDataModel();
            _multiplier = 2;
        }

        public void SetDataFromJson(string jsonString, int RT = 0, int LS = 0, int BOM = 0, int SI = 0, bool AP = false)
        {
            var MRPData = JsonConvert.DeserializeObject<MRPDataModel>(jsonString);
            _MRPDatalvl1 = MRPData;
            _MRPDatalvl1.RealizationTime = RT;
            _MRPDatalvl1.LotSize = LS;
            _MRPDatalvl1.BOM = BOM;
            _MRPDatalvl1.StartingInventory = SI;
            _MRPDatalvl1.AutoPlanning = AP;
        }

        public void FillTable()
        {
            //GrossRequirements
<<<<<<< Updated upstream
            for (int i = _MRPDatalvl1.RealizationTime; i < 10; i++) _MRPDatalvl2.GrossRequirements[i] = _MRPDatalvl1.PlannedReceipt[i] * _multiplier;
=======
            _MRPDatalvl2.GrossRequirements = FillGrossRequirements(_MRPDatalvl2.GrossRequirements, _MRPDatalvl1.PlannedReceipt, _multiplier);
>>>>>>> Stashed changes
            //ProjectedOnHand
            _MRPDatalvl2.ProjectedOnHand[0] = _MRPDatalvl2.StartingInventory; for (int i = 1; i < 10; i++) _MRPDatalvl2.ProjectedOnHand[i] = _MRPDatalvl2.ProjectedOnHand[i - 1] - _MRPDatalvl2.GrossRequirements[i];

            var finish = false;
            while (!finish)
            {
                //NetRequirements
                for (int i = 0; i < 10; i++)
                {
                    if (_MRPDatalvl2.ProjectedOnHand[i] < 0)
                    {
                        _MRPDatalvl2.NetRequirements[i] = _MRPDatalvl2.ProjectedOnHand[i] * -1;
                        break;
                    }
                    if (i == 9) finish = true;
                }

                for (int i = 0; i < 10; i++)
                {
                    if (_MRPDatalvl2.NetRequirements[i] > 0)
                    {
                        if (i > _MRPDatalvl2.RealizationTime - 1) //if this will be true we can start a production, otherwise we need to place an order
                        {
                            //PlannedRelease
                            for (int x = 0; x < 10; x++) if (_MRPDatalvl2.NetRequirements[i] > 0) _MRPDatalvl2.PlannedRelease[i] = _MRPDatalvl1.LotSize > _MRPDatalvl2.NetRequirements[i] ? _MRPDatalvl1.LotSize : _MRPDatalvl1.LotSize * 2;
                            //PlannedReceipt
                            for (int x = 0; x < 10; x++) if (_MRPDatalvl2.PlannedRelease[i] > 0) _MRPDatalvl2.PlannedReceipt[i - _MRPDatalvl1.RealizationTime] = _MRPDatalvl2.PlannedRelease[i];
                        }
                        else
                        {
                            if (_MRPDatalvl1.AutoPlanning)
                            {
                                //SheduledReceipts
                                _MRPDatalvl2.SheduledReceipts[i] = _MRPDatalvl2.GrossRequirements[i] - _MRPDatalvl2.ProjectedOnHand[i];
                                _MRPDatalvl2.ProjectedOnHand[i] = 0;
                            }
                        }

                    }
                }
                //ProjectedOnHandCorrection
                for (int i = 1; i < 10; i++) _MRPDatalvl2.ProjectedOnHand[i] = _MRPDatalvl2.ProjectedOnHand[i - 1] - _MRPDatalvl2.GrossRequirements[i] + _MRPDatalvl2.PlannedRelease[i];
            }
        }

        public string DataToJson()
        {
            return DataToJson<MRPDataModel>(_MRPDatalvl2);
        }
    }

}

