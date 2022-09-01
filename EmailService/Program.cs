using EmailService;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

var emailConfig = Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);

builder.Services.AddControllers();
var app = builder.Build();

app.Run();
