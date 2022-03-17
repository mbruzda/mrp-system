using API_ERP.DataModels;
using Newtonsoft.Json;

namespace API_ERP
{
    public class MRPlvl1 : IERP
    {
        public GHPDataModel _GHPData;
        public MRPDataModel _MRPData;

        public MRPlvl1()
        {
            _GHPData = new GHPDataModel();
            _MRPData = new MRPDataModel();
        }

        public void SetDataFromJson(string jsonString, int RT = 0, int LS = 0, int BOM = 0, int SI = 0, bool AP = false)
        {
            var GHPData = JsonConvert.DeserializeObject<GHPDataModel>(jsonString);
            _GHPData = GHPData;
            _MRPData.RealizationTime = RT;
            _MRPData.LotSize = LS;
            _MRPData.BOM = BOM;
            _MRPData.StartingInventory = SI;
            _MRPData.AutoPlanning = AP;
        }

        public void DataCheck()
        {
            foreach (var num in _GHPData.SalesForecast) if (num < 0) throw new ArgumentOutOfRangeException();
            foreach (var num in _GHPData.Production) if (num < 0) throw new ArgumentOutOfRangeException();
            foreach (var num in _GHPData.Inventory) if (num < 0) throw new ArgumentOutOfRangeException();

            if (_MRPData.RealizationTime < 0) throw new ArgumentOutOfRangeException();
            if (_MRPData.LotSize < 0) throw new ArgumentOutOfRangeException();
            if (_MRPData.StartingInventory < 0) throw new ArgumentOutOfRangeException();
        }

        public void FillTable()
        {
            //GrossRequirements
            for (int i = 0; i < 10; i++) _MRPData.GrossRequirements[i - _GHPData.RealizationTime] = _GHPData.Production[i];
            //ProjectedOnHand
            _MRPData.ProjectedOnHand[0] = _MRPData.StartingInventory - _MRPData.GrossRequirements[0] + _MRPData.SheduledReceipts[0]; 
            for (int i = 1; i < 10; i++) _MRPData.ProjectedOnHand[i] = _MRPData.ProjectedOnHand[i - 1] - _MRPData.GrossRequirements[i] + _MRPData.SheduledReceipts[i];

            for (int i = 0; i < 10; i++)
            {
                //NetRequirements
                if (_MRPData.ProjectedOnHand[i] < 0)
                {
                    _MRPData.NetRequirements[i] = _MRPData.ProjectedOnHand[i] * -1;
                }

                if (_MRPData.NetRequirements[i] > 0)
                {
                    if (i > _MRPData.RealizationTime - 1 ) //if this will be true we can start a production, otherwise we need to place an order
                    {
                        //PlannedRelease
                        if (_MRPData.NetRequirements[i] > 0) _MRPData.PlannedRelease[i] = _MRPData.LotSize > _MRPData.NetRequirements[i] ? _MRPData.LotSize : _MRPData.LotSize * 2;
                        //PlannedReceipt
                        if (_MRPData.PlannedRelease[i] > 0) _MRPData.PlannedReceipt[i - _MRPData.RealizationTime] = _MRPData.PlannedRelease[i];
                    }
                    else
                    {
                        if (_MRPData.AutoPlanning)
                        {
                            //SheduledReceipts
                            _MRPData.SheduledReceipts[i] = _MRPData.NetRequirements[i];
                            _MRPData.ProjectedOnHand[i] = 0;
                        }
  
                    }

                }

                //ProjectedOnHandCorrection
                for (int x = 1; x < 10; x++) _MRPData.ProjectedOnHand[x] = _MRPData.ProjectedOnHand[x - 1] - _MRPData.GrossRequirements[x] + _MRPData.PlannedRelease[x] + _MRPData.SheduledReceipts[x];
                
            }
        }


        public string DataToJson()
        {
            return JsonConvert.SerializeObject(_MRPData);
        }
    }
}