using System.Text.Json;
using Newtonsoft.Json;

namespace API_ERP
{
    public class GHP : IERP
    {

        public GHPData Data;


        public void SetDataFromJson(string jsonString)
        {
            var GHPData = JsonConvert.DeserializeObject<GHPData>(jsonString);

            if (!GHPData.SalesForecast.Any()) throw new Exception("Error parsing the table");

            this.Data = GHPData;
        }

        public void FillTable()
        {

            this.Data.Inventory[0] = this.Data.StartingInventory;

            for (int i = 1; i < 10; i++)
            {
                this.Data.Inventory[i] = this.Data.Inventory[i - 1] - this.Data.SalesForecast[i] + this.Data.Production[i];
            }
        }

        public String DataToJson()
        {
            return JsonConvert.SerializeObject(this.Data);
        }
    }
}
