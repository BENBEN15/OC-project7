using PoseidonAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using PoseidonAPI.Repositories;
using PoseidonAPI.Services;
using PoseidonAPI.Model;
using PoseidonAPI.Dtos;
using System.Reflection;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.

    //Automapper
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    //Repositories
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

        //Add authorisation for swagger
        options.AddSecurityDefinition("basic", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter your credentials",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "basic"
        });

        //Adding authentication requirements for swagger
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id="basic"
                    }
                },
                new string[]{}
            }
        });
    });
    /*builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);*/
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });

    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseEndpoints(endpoints => endpoints.MapControllers());
    app.Run();
}