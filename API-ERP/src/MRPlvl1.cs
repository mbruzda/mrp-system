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

        public void FillTable()
        {
            //GrossRequirements
            for (int i = _GHPData.RealizationTime; i < 10; i++) _MRPData.GrossRequirements[i - _GHPData.RealizationTime] = _GHPData.Production[i];
            //ProjectedOnHand
            _MRPData.ProjectedOnHand[0] = _MRPData.StartingInventory; for (int i = 1; i < 10; i++) _MRPData.ProjectedOnHand[i] = _MRPData.ProjectedOnHand[i - 1] - _MRPData.GrossRequirements[i];

            var finish = false;
            while (!finish)
            {
                //NetRequirements
                for (int i = 0; i < 10; i++)
                {
                    if (_MRPData.ProjectedOnHand[i] < 0)
                    {
                        _MRPData.NetRequirements[i] = _MRPData.ProjectedOnHand[i] * -1;
                        break;
                    }
                    if (i == 9) finish = true;
                }

                for (int i = 0; i < 10; i++)
                {
                    if (_MRPData.NetRequirements[i] > 0)
                    {
                        if (i > _MRPData.RealizationTime - 1 ) //if this will be true we can start a production, otherwise we need to place an order
                        {
                            //PlannedRelease
                            for (int x = 0; x < 10; x++) if (_MRPData.NetRequirements[i] > 0) _MRPData.PlannedRelease[i] = _MRPData.LotSize > _MRPData.NetRequirements[i] ? _MRPData.LotSize : _MRPData.LotSize * 2;
                            //PlannedReceipt
                            for (int x = 0; x < 10; x++) if (_MRPData.PlannedRelease[i] > 0) _MRPData.PlannedReceipt[i - _MRPData.RealizationTime] = _MRPData.PlannedRelease[i];
                        }
                        else
                        {
                            if (_MRPData.AutoPlanning)
                            {
                                //SheduledReceipts
                                _MRPData.SheduledReceipts[i] = _MRPData.GrossRequirements[i] - _MRPData.ProjectedOnHand[i];
                                _MRPData.ProjectedOnHand[i] = 0;
                            }
  
                        }

                    }
                }
                //ProjectedOnHandCorrection
                for (int i = 1; i < 10; i++) _MRPData.ProjectedOnHand[i] = _MRPData.ProjectedOnHand[i - 1] - _MRPData.GrossRequirements[i] + _MRPData.PlannedRelease[i];
            }
        }

        public string DataToJson()
        {
            return JsonConvert.SerializeObject(_MRPData);
        }
    }
}