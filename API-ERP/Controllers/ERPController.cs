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

        [HttpPost(Name = "GetERPTable")]
        public string Post([FromBody] string jsonString)
        {
            _ghp.SetDataFromJson(jsonString);
            _ghp.FillTable();
            return _ghp.DataToJson();

        }

    }
}
