using EmailOTPAPI.Data;
using EmailOTPAPI.DTOs;
using EmailOTPAPI.Interfaces;
using EmailOTPAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace EmailOTPAPI.Services
{
    public class OtpService : IOtpService
    {
        private readonly OtpDbContext _context;
        private const int OTP_EXPIRY_MINUTES = 1;
        private static readonly Regex EmailRegex = new(@"^[^@\s]+@dso\.org\.sg$", RegexOptions.Compiled);
        private const int MAX_ATTEMPTS = 10;

        public OtpService(OtpDbContext context)
        {
            _context = context;
        }

        public async Task<OtpGenerateResponse> GenerateOtpAsync(OtpGenerateRequest request)
        {
            if (!EmailRegex.IsMatch(request.Email))
            {
                return new OtpGenerateResponse
                {
                    Status = OtpStatus.STATUS_EMAIL_INVALID,
                    Message = "Invalid email format."
                };
            }

            string otp = new Random().Next(100000, 999999).ToString();

            var otpRequest = new OtpRequest
            {
                Email = request.Email,
                Otp = otp,
                GeneratedAt = DateTime.UtcNow
            };

            _context.OtpRequests.Add(otpRequest);
            await _context.SaveChangesAsync();

            string emailBody = $"Your OTP Code is {otp}. The code is valid for 1 minute.";
            bool emailSent = SendEmail(request.Email, emailBody);

            return new OtpGenerateResponse
            {
                Status = emailSent ? OtpStatus.STATUS_EMAIL_OK : OtpStatus.STATUS_EMAIL_FAIL,
                Message = emailSent ? "OTP sent successfully." : "Failed to send OTP.",
                OTP = otp
            };
        }

        public async Task<OtpValidateResponse> ValidateOtpAsync(OtpValidateRequest request)
        {
            var existing = await _context.OtpRequests
                .Where(x => x.Email == request.Email)
                .OrderByDescending(x => x.GeneratedAt)
                .FirstOrDefaultAsync();

            if (existing == null || DateTime.UtcNow - existing.GeneratedAt > TimeSpan.FromMinutes(OTP_EXPIRY_MINUTES))
            {
                return new OtpValidateResponse
                {
                    Status = OtpStatus.STATUS_OTP_TIMEOUT,
                    Message = "OTP Timeout after 1 min"
                };
            }

            if (existing.AttemptCount > MAX_ATTEMPTS)
                return new OtpValidateResponse
                {
                    Status = OtpStatus.STATUS_OTP_FAIL,
                    Message = "OTP is wrong after 10 tries"
                };            
            existing.AttemptCount++;
            await _context.SaveChangesAsync();

            return new OtpValidateResponse
            {
                Status = existing.Otp == request.Otp ? OtpStatus.STATUS_OTP_OK : OtpStatus.STATUS_OTP_FAIL,
                Message = existing.Otp == request.Otp ? "OTP validated successfully." : "Invalid OTP."
            };
        }

        private bool SendEmail(string to, string body)
        {
            //Replace with actual email sending logic.
            return true;
        }
    }
}
