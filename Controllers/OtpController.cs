using EmailOTPAPI.DTOs;
using EmailOTPAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmailOTPAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OtpController : ControllerBase
    {
        private readonly IOtpService _otpService;

        public OtpController(IOtpService otpService)
        {
            _otpService = otpService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromBody] OtpGenerateRequest request)
        {
            var response = await _otpService.GenerateOtpAsync(request);
            return Ok(response);
        }

        [HttpPost("validate")]
        public async Task<IActionResult> Validate([FromBody] OtpValidateRequest request)
        {
            var response = await _otpService.ValidateOtpAsync(request);
            return Ok(response);
        }
    }
}