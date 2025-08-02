using EmailOTPAPI.Data;
using EmailOTPAPI.Interfaces;
using EmailOTPAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<OtpDbContext>(opt => opt.UseInMemoryDatabase("OtpDb"));
builder.Services.AddScoped<IOtpService, OtpService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();