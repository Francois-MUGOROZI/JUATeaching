// using Application.Services.Identity;
// using Microsoft.AspNetCore.Mvc;

// namespace NIDAClone.Controllers;

// [ApiController]
// [Route("[controller]")]
// public class IdentityController : ControllerBase
// {
//     private readonly IIdentityService _identityService;

//     public IdentityController(IIdentityService identityService)
//     {
//         _identityService = identityService;
//     }

//     [HttpGet("/api/identity/{idNumber}")]
//     public async Task<IActionResult> GetIdentityByIdNumber(string idNumber)
//     {
//         var identity = await _identityService.GetIdentityByIdNumberAsync(idNumber);
//         if (identity == null)
//         {
//             return NotFound();
//         }

//         return Ok(identity);
//     }
// }
