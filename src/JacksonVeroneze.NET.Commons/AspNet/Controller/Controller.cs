using Microsoft.AspNetCore.Mvc;

namespace JacksonVeroneze.NET.Commons.AspNet.Controller
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseController : ControllerBase
    {
    }
}