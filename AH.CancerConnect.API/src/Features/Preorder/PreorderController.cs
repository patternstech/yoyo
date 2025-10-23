namespace AH.CancerConnect.API.Features.Preorder;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PreorderController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post()
    {
        return Ok(new { Status = "Healthy" });
    }
}
