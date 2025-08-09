using HGD_SSO_API.Data;
using Microsoft.AspNetCore.Mvc;

namespace HGD_SSO_API.Controllers.Configs;

[ApiController]
[Route("api/[controller]")]
public class RedirectUrlController : ControllerBase
{
    private readonly AppDbContext _db;
    
    public RedirectUrlController(AppDbContext db)
    {
        _db = db;
    }
    [HttpGet("check-redirect-url")]
    public IActionResult CheckRedirectUrl([FromQuery] string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return BadRequest(new { message = "آدرس وارد نشده است." });

        var isValid = _db.RedirectUrls.Any(r => r.Url == url);

        return Ok(new
        {
            url,
            isValid
        });
    }

    
}