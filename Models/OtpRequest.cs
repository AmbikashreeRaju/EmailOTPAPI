namespace EmailOTPAPI.Models
{
    public class OtpRequest
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Otp { get; set; }
        public DateTime GeneratedAt { get; set; }
        public int AttemptCount { get; set; }
    }
}
