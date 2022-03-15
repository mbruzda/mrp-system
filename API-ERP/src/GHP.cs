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

        public void SetDataFromJson(string jsonString, int RT = 0, int LS = 0, int BOM = 0, int SI = 0)
        {
            var Data = JsonConvert.DeserializeObject<GHPDataModel>(jsonString);
            _Data = Data;
        }

        public void FillTable()
        {
            _Data.Inventory[0] = _Data.StartingInventory - _Data.SalesForecast[0];
            for (int i = 1; i < 10; i++) _Data.Inventory[i] = _Data.Inventory[i - 1] - _Data.SalesForecast[i] + _Data.Production[i];
        }

        public string DataToJson()
        {
            return JsonConvert.SerializeObject(_Data);
        }
    }
}
