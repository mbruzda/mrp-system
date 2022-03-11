using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API_ERP;
using System.Text.Json;
using System.Web.Http.Cors;

namespace API_ERP.Controllers
{
    [Route("api")]
    [ApiController]
    public class ERPController : ControllerBase
    {
        [HttpPost("GetERPTable")]
        public string Post([FromBody] string jsonString)
        {
            var ghp = new GHP();
            ghp.SetDataFromJson(jsonString);
            ghp.FillTable();
            return ghp.DataToJson();

        }

    }
}
