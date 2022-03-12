﻿using Newtonsoft.Json;

namespace API_ERP
{
    public class GHP : IERP
    {
        public GHPDataModel Data;

        public void SetDataFromJson(string jsonString, int RT = 0, int LS = 0, int BOM = 0, int SI = 0)
        {
            var GHPData = JsonConvert.DeserializeObject<GHPDataModel>(jsonString);
            if (!GHPData.SalesForecast.Any()) throw new Exception("Error parsing the table");
            this.Data = GHPData;
        }

        public void FillTable()
        {
            this.Data.Inventory[0] = this.Data.StartingInventory;
            for (int i = 1; i < 10; i++) this.Data.Inventory[i] = this.Data.Inventory[i - 1] - this.Data.SalesForecast[i] + this.Data.Production[i];
        }

        public string DataToJson()
        {
            return JsonConvert.SerializeObject(this.Data);
        }
    }
}