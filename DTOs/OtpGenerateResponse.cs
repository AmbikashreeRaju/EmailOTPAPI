namespace EmailOTPAPI.DTOs
{
    public class OtpGenerateResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string OTP {  get; set; }
    }
}
