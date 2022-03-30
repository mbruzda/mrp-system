using API_ERP.DataModels;
using API_ERP.Interfaces;
using Newtonsoft.Json;

namespace API_ERP
{
    public class MRPlvl1 : ERP,IERP
    {
        private GHPDataModel _GHPData;
        private MRPDataModel _MRPData;

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
            //Orders
            _MRPData.SheduledReceipts = FillOrders(_MRPData.SheduledReceipts, _GHPData.Orders);
            //GrossRequirements
            _MRPData.GrossRequirements = FillGrossRequirements(_MRPData.GrossRequirements, _GHPData.Production, 1);
            //ProjectedOnHand
            _MRPData.ProjectedOnHand = FillProjectedOnHand(_MRPData.ProjectedOnHand, _MRPData.GrossRequirements, _MRPData.SheduledReceipts, _MRPData.StartingInventory);

            for (int i = 0; i < 10; i++)
            {
                //NetRequirements
                if (_MRPData.ProjectedOnHand[i] < 0) _MRPData.NetRequirements[i] = _MRPData.ProjectedOnHand[i] * -1;

                if (_MRPData.NetRequirements[i] == 0) continue;
                
                if (i > _MRPData.RealizationTime) //if this will be true we can start a production, otherwise we need to place an order
                {
                    //PlannedRelease
                    if (_MRPData.NetRequirements[i] > 0) _MRPData.PlannedRelease[i] = _MRPData.LotSize;
                    //PlannedReceipt
                    if (_MRPData.PlannedRelease[i] > 0) _MRPData.PlannedReceipt[i - _MRPData.RealizationTime] = _MRPData.PlannedRelease[i];

                    if (_MRPData.AutoPlanning && _MRPData.NetRequirements[i] > _MRPData.PlannedRelease[i]) // this is scenario when we can't produce enought and we need to order to meet the demand
                    {
                        //SheduledReceipts
                        _MRPData.SheduledReceipts[i] = _MRPData.NetRequirements[i] - _MRPData.PlannedRelease[i];
                    }
                }
                else if (_MRPData.AutoPlanning)
                {
                    //SheduledReceipts
                    _MRPData.SheduledReceipts[i] = _MRPData.NetRequirements[i];
                    _MRPData.ProjectedOnHand[i] = 0;
                }

                //ProjectedOnHandCorrection
                for (int x = 1; x < 10; x++) _MRPData.ProjectedOnHand[x] = _MRPData.ProjectedOnHand[x - 1] - _MRPData.GrossRequirements[x] + _MRPData.PlannedRelease[x] + _MRPData.SheduledReceipts[x];
                
            }
        }

        public string DataToJson()
        {
            return DataToJson<MRPDataModel>(_MRPData);
        }
    }
}