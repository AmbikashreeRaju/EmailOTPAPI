using EmailOTPAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EmailOTPAPI.Data
{
    public class OtpDbContext : DbContext
    {
        public OtpDbContext(DbContextOptions<OtpDbContext> options) : base(options) { }

        public DbSet<OtpRequest> OtpRequests { get; set; }
    }
}