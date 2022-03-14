﻿using System.Text.Json;

namespace API_ERP
{
    public interface IERP
    {
        void SetDataFromJson(string jsonString, int RT = 0, int LS = 0, int BOM = 0, int SI = 0);
        void FillTable();
        string DataToJson();
    }
}