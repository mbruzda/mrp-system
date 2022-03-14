using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API_ERP;
using System.Text.Json;

namespace API_ERP.Controllers
{
    [Route("api")]
    [ApiController]
    public class ERPController : ControllerBase
    {
        private IERP _ghp = new GHP();
        private IERP _mrplvl1 = new MRPlvl1();
        //private IERP _ghp = new GHP();

        [HttpPost("GetGHPTable")]
        public string PostGHP([FromBody] string jsonString)
        {
            _ghp.SetDataFromJson(jsonString);
            _ghp.FillTable();
            return _ghp.DataToJson();
        }

        [HttpPost("GetMRPlvl1Table/{RT}/{LS}/{BOM}/{SI}")]
        public string PostMRPlvl1([FromBody] string jsonString,int RT,int LS, int BOM, int SI)
        {
            _mrplvl1.SetDataFromJson(jsonString, RT, LS, BOM, SI);
            _mrplvl1.FillTable();
            return _mrplvl1.DataToJson();
        }

        [HttpPost("Ping")]
        public string Ping()
        {
            return "OK";
        }

    }
}