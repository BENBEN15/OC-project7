using PoseidonAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using PoseidonAPI.Repositories;
using PoseidonAPI.Services;
using PoseidonAPI.Model;
using PoseidonAPI.Dtos;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
{
    //Clear providers
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();

    //Automapper
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    //Repositories and serives
    builder.Services.AddScoped<IRepository<Bid>, BidRepository>();
    builder.Services.AddScoped<IService<BidDTO>, BidService>();

    builder.Services.AddScoped<IRepository<CurvePoint>, CurvePointRepository>();
    builder.Services.AddScoped<IService<CurvePointDTO>, CurvePointService>();

    builder.Services.AddScoped<IRepository<Rating>, RatingRepository>();
    builder.Services.AddScoped<IService<RatingDTO>, RatingService>();

    builder.Services.AddScoped<IRepository<Rule>, RuleRepository>();
    builder.Services.AddScoped<IService<RuleDTO>, RuleService>();

    builder.Services.AddScoped<IRepository<Trade>, TradeRepository>();
    builder.Services.AddScoped<IService<TradeDTO>, TradeService>();

    //email service
    builder.Services.AddScoped<IEmailService, EmailService>();

    //Dbcontext
    builder.Services.AddDbContext<PoseidonDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    //Identity
    builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<PoseidonDBContext>().AddDefaultTokenProviders();

    //Authorization
    builder.Services.AddAuthorization();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    //Swagger
    builder.Services.AddSwaggerGen(options =>
    {
        //Adding doc
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "PoseidonAPI",
            Description = "An ASP.NET Core Web API for managing trades, Project 7 .NET Back-end path for Openclassroom",
        });

        //generate the xml docs for swagger
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });

    builder.Services.AddRazorPages();
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "BasicAuth v1"));
    }

    //Adding logger 
    var loggerFactory = app.Services.GetService<ILoggerFactory>();
    loggerFactory.AddFile($@"{Directory.GetCurrentDirectory()}\Logs\log.txt");

    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
        endpoints.MapRazorPages();
    });

    app.Run();
}