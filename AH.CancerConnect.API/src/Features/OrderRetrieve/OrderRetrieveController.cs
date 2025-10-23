namespace AH.CancerConnect.API.Features.OrderRetrieve;

using AH.CancerConnect.API.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/order")]
public class OrderRetrieveController : ControllerBase
{
    private readonly IOrderRetrieveService _orderService;

    public OrderRetrieveController(IOrderRetrieveService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("{deployment}/{orderId}")]
    public async Task<IActionResult> GetOrder(string deployment, string orderId)
    {
        var result = await _orderService.GetOrderAsync(deployment, orderId);

        if (result.Error != null)
        {
            if (result.StatusCode.HasValue)
            {
                return StatusCode(result.StatusCode.Value, result.Error);
            }

            return BadRequest(result.Error);
        }

        return Ok(result.Data);
    }
}
