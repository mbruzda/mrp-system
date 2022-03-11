using System.Text.Json;

namespace API_ERP
{
    public interface IERP
    {
        void SetDataFromJson(string jsonString);
        void FillTable();
        string DataToJson();
    }
}