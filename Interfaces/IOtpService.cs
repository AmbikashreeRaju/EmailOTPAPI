using EmailOTPAPI.DTOs;
using EmailOTPAPI.Models;
using System.Threading.Tasks;

namespace EmailOTPAPI.Interfaces
{
    public interface IOtpService
    {
        Task<OtpGenerateResponse> GenerateOtpAsync(OtpGenerateRequest request);
        Task<OtpValidateResponse> ValidateOtpAsync(OtpValidateRequest request);
    }
}