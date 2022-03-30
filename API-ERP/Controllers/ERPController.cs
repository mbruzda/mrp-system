using Microsoft.AspNetCore.Mvc;

namespace API_ERP.Controllers
{
    [Route("api")]
    [ApiController]
    public class ERPController : ControllerBase
    {
        private IERP _ghp = new GHP();
        private IERP _mrplvl1 = new MRPlvl1();
        private IERP _mrplvl2 = new MRPlvl2();

        [HttpPost("GetGHPTable")]
        public string PostGHP([FromBody] string jsonString)
        {
            try
            {
                _ghp.SetDataFromJson(jsonString);
                _ghp.FillTable();
                return _ghp.DataToJson();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return "Value cant be less that 0:\n\n" + ex;
            }
            catch (Exception ex)
            {
                return "Error:" + ex;
            }
        }

        [HttpPost("GetMRPlvl1Table/{RT}/{LS}/{BOM}/{SI}/{AP}")]
        public string PostMRPlvl1([FromBody] string jsonString,int RT,int LS, int BOM, int SI , bool AP)
        {
            try
            {
                _mrplvl1.SetDataFromJson(jsonString, RT, LS, BOM, SI , AP);
                _mrplvl1.FillTable();
                return _mrplvl1.DataToJson();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return "Value cant be less that 0:\n\n" + ex;
            }
            catch (Exception ex)
            {
                return "Error:" + ex;
            }
        }

        [HttpPost("GetMRPlvl2Table/{RT}/{LS}/{BOM}/{SI}/{AP}")]
        public string PostMRPlvl2([FromBody] string jsonString, int RT, int LS, int BOM, int SI, bool AP)
        {
            try
            {
                _mrplvl2.SetDataFromJson(jsonString, RT, LS, BOM, SI, AP);
                _mrplvl2.FillTable();
                return _mrplvl2.DataToJson();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return "Value cant be less that 0:\n\n" + ex;
            }
            catch (Exception ex)
            {
                return "Error:" + ex;
            }
        }

        [HttpPost("Ping")]
        public string Ping()
        {
            return "OK";
        }

    }
}