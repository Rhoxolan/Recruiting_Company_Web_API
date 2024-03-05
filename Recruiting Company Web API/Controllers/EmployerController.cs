using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Recruiting_Company_Web_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class EmployerController : ControllerBase
	{
	}
}
