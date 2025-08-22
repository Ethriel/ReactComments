using Microsoft.AspNetCore.Mvc;
using ReactComments.DAL;

namespace ReactComments.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DALTestController : ControllerBase
    {
        [HttpGet("get")]
        public string DALTestGet()
        {
            var dalTestClass = new DALTestClass();
            return dalTestClass.DALTestMethod();
        }
    }
}
