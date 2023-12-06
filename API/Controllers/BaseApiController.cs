using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //Use action filter to update last activity
    [ServiceFilter(typeof(LogUserActivity))]
    //Assign api controller attribute to access http api resonses
    [ApiController]
    //Assign a route( for the url) to access the controller, in this case using placeholder text, which will be
    //replaced during runtime with the word "Users" i.e. /api/Users
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {

        
    }
}