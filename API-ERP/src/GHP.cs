using API_ERP.DataModels;
using Newtonsoft.Json;

namespace API_ERP
{
    public class GHP : IERP
    {
        public GHPDataModel _Data;

        public GHP()
        {
            _Data = new GHPDataModel();
        }

        public void SetDataFromJson(string jsonString, int RT = 0, int LS = 0, int BOM = 0, int SI = 0 , bool AP = false)
        {
            var Data = JsonConvert.DeserializeObject<GHPDataModel>(jsonString);
            _Data = Data;
        }

        public void DataCheck()
        {
            foreach (var num in _Data.SalesForecast) if (num < 0) throw new ArgumentOutOfRangeException();
            foreach (var num in _Data.Production) if (num < 0) throw new ArgumentOutOfRangeException();
            foreach (var num in _Data.Inventory) if (num < 0) throw new ArgumentOutOfRangeException();

            if (_Data.RealizationTime < 0) throw new ArgumentOutOfRangeException();
            if (_Data.StartingInventory < 0) throw new ArgumentOutOfRangeException();
        }

        public void FillTable()
        {
            _Data.Inventory[0] = _Data.StartingInventory - _Data.SalesForecast[0];
            for (int i = 1; i < 10; i++)
            {
                _Data.Inventory[i] = _Data.Inventory[i - 1] - _Data.SalesForecast[i] + _Data.Production[i];

                if (_Data.Inventory[i]<0)
                {
                    var need = _Data.Inventory[i] * -1;
                    _Data.Inventory[i] += need;
                    _Data.Production[i] += need;
                }
            }
            
        }

        public string DataToJson()
        {
            return JsonConvert.SerializeObject(_Data);
        }
    }
}
