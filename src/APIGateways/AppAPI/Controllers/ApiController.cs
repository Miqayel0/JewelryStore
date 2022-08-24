using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class ApiController : ControllerBase
{
}
