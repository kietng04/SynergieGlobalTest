using News.Api.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using News.Api.Models;
using Hangfire;
using News.Api.Infrastructure;
using News.Api.Data;
using News.Api.Utils;
using News.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using Hangfire.SqlServer;
using Microsoft.Extensions.Options;
using Resend;

var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
Env.Load(envPath);
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddSwaggerDocumentation();

var hangfireConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddHangfire(config => config.UseSqlServerStorage(hangfireConnection));
builder.Services.AddHangfireServer();
builder.Services.AddHttpClient<INewsApiService, NewsApiService>();

var jwtConfig = builder.Configuration.GetSection("Jwt").Get<JwtConfig>() ?? new JwtConfig();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key)),
            ValidateIssuer = true,
            ValidIssuer = jwtConfig.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtConfig.Audience,
            ValidateLifetime = true,
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


builder.Services.AddOptions();
builder.Services.AddHttpClient<ResendClient>();
builder.Services.Configure<ResendClientOptions>( o =>
{
    o.ApiToken = Environment.GetEnvironmentVariable( "RESEND_APITOKEN" )!;
} );
builder.Services.AddTransient<IResend, ResendClient>();
var app = builder.Build();



app.UseSwagger();
app.UseSwaggerUI();
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new AllowAllDashboardAuthorizationFilter() }
});
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/health", () => new
{
    Status = "OK",
    Message = "News API is running",
    Timestamp = DateTime.UtcNow
});

RecurringJob.AddOrUpdate<INewsApiService>(
    "sync-top-news-hourly",
    service => service.SyncTopNewsToDatabase(),
    Cron.Hourly
);
RecurringJob.AddOrUpdate<INewsApiService>(
    "send-daily-digest",
    service => service.SendDailyDigestToSubscribers(),
    Cron.Minutely
);
RecurringJob.AddOrUpdate<INewsApiService>(
    "send-weekly-digest",
    service => service.SendWeeklyDigestToSubscribers(),
    Cron.Weekly
);
app.Run();
