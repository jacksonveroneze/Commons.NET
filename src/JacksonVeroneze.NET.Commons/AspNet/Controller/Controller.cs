using Microsoft.AspNetCore.Mvc;

namespace JacksonVeroneze.NET.Commons.AspNet.Controller
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseController : ControllerBase
    {
    }
}