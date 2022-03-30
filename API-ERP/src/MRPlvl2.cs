using API_ERP.DataModels;
using API_ERP.Interfaces;
using Newtonsoft.Json;

namespace API_ERP
{
    public class MRPlvl2 : ERP,IERP
    {
        public MRPDataModel _MRPDatalvl1;
        public MRPDataModel _MRPDatalvl2;
        public int _multiplier;

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
            _MRPDatalvl2.RealizationTime = RT;
            _MRPDatalvl2.LotSize = LS;
            _MRPDatalvl2.BOM = BOM;
            _MRPDatalvl2.StartingInventory = SI;
            _MRPDatalvl2.AutoPlanning = AP;
        }

        public void DataCheck()
        {
            foreach (var num in _MRPDatalvl1.SheduledReceipts) if (num < 0) throw new ArgumentOutOfRangeException();
            foreach (var num in _MRPDatalvl1.PlannedRelease) if (num < 0) throw new ArgumentOutOfRangeException();
            foreach (var num in _MRPDatalvl1.PlannedReceipt) if (num < 0) throw new ArgumentOutOfRangeException();
            foreach (var num in _MRPDatalvl1.GrossRequirements) if (num < 0) throw new ArgumentOutOfRangeException();
            foreach (var num in _MRPDatalvl1.ProjectedOnHand) if (num < 0) throw new ArgumentOutOfRangeException();
            foreach (var num in _MRPDatalvl1.NetRequirements) if (num < 0) throw new ArgumentOutOfRangeException();

            if (_MRPDatalvl2.RealizationTime < 0) throw new ArgumentOutOfRangeException();
            if (_MRPDatalvl2.LotSize < 0) throw new ArgumentOutOfRangeException();
            if (_MRPDatalvl2.StartingInventory < 0) throw new ArgumentOutOfRangeException();
        }

        public void FillTable()
        {
            //GrossRequirements
            _MRPDatalvl2.GrossRequirements = FillGrossRequirements(_MRPDatalvl2.GrossRequirements, _MRPDatalvl1.PlannedReceipt, 2);
            //ProjectedOnHand
            _MRPDatalvl2.ProjectedOnHand = FillProjectedOnHand(_MRPDatalvl2.ProjectedOnHand, _MRPDatalvl2.GrossRequirements, _MRPDatalvl2.SheduledReceipts, _MRPDatalvl2.StartingInventory);

            for (int i = 0; i < 10; i++)
            {
                //NetRequirements
                if (_MRPDatalvl2.ProjectedOnHand[i] < 0)
                {
                    _MRPDatalvl2.NetRequirements[i] = _MRPDatalvl2.ProjectedOnHand[i] * -1;
                }

                if (_MRPDatalvl2.NetRequirements[i] == 0) continue;
                
                if (i > _MRPDatalvl1.RealizationTime) //if this will be true we can start a production, otherwise we need to place an order
                {
                    //PlannedRelease
                    if (_MRPDatalvl2.NetRequirements[i] > 0) _MRPDatalvl2.PlannedRelease[i] = _MRPDatalvl2.LotSize;
                    //PlannedReceipt
                    if (_MRPDatalvl2.PlannedRelease[i] > 0) _MRPDatalvl2.PlannedReceipt[i - _MRPDatalvl2.RealizationTime] = _MRPDatalvl2.PlannedRelease[i];

                    if (_MRPDatalvl2.AutoPlanning && _MRPDatalvl2.NetRequirements[i] > _MRPDatalvl2.PlannedRelease[i]) // this is scenario when we can't produce enought and we need to order to meet the demand
                    {
                        //SheduledReceipts
                        _MRPDatalvl2.SheduledReceipts[i] = _MRPDatalvl2.NetRequirements[i] - _MRPDatalvl2.PlannedRelease[i];
                    }
                }
                else if (_MRPDatalvl2.AutoPlanning)
                {
                    //SheduledReceipts
                    _MRPDatalvl2.SheduledReceipts[i] = _MRPDatalvl2.NetRequirements[i];
                    _MRPDatalvl2.ProjectedOnHand[i] = 0;
                }

                

                //ProjectedOnHandCorrection
                for (int x = 1; x < 10; x++) _MRPDatalvl2.ProjectedOnHand[x] = _MRPDatalvl2.ProjectedOnHand[x - 1] - _MRPDatalvl2.GrossRequirements[x] + _MRPDatalvl2.PlannedRelease[x] + _MRPDatalvl2.SheduledReceipts[x];

            }
        }

        public string DataToJson()
        {
            return JsonConvert.SerializeObject(_MRPDatalvl2);
        }
    }

}

