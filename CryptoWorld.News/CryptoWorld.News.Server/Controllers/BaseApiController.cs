using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoWorld.Application.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class BaseApiController : ControllerBase
    {
    }
}
