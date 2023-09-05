using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Http.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SubscriptionController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var items = new List<Subscription>()
        {
            new Subscription() { OwnerId = "user_2Uw2kk7WIQyyr792qEd4kSWNDsI", FirstName = "Nicholas", LastName = "Hollas "},
            new Subscription() { OwnerId = "user_2Uw5mp3EFHJOrAJpBVm9Wd0sKg1", FirstName = "John", LastName = "Doe" },
        };
        
        var user = HttpContext.User;

        var token = HttpContext.Request.Headers.Authorization.ToString();

        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var subscription = items.FirstOrDefault(x => x.OwnerId == userId);
        
        return Ok(subscription);
    }
}

public class Subscription
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string OwnerId { get; set; }
}