using PoseidonAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PoseidonAPI.Repositories;
using PoseidonAPI.Services;
using PoseidonAPI.Model;
using PoseidonAPI.Dtos;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.
    ///CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy",
                builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
            );
    });

    ///Automapper
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    ///Repositories
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

    ///Dbcontext
    builder.Services.AddDbContext<PoseidonDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    ///Identity
    builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<PoseidonDBContext>();
    ///Authorization
    builder.Services.AddAuthorization();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
}



var app = builder.Build();
{
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseCors("CorsPolicy");
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseEndpoints(endpoints => endpoints.MapControllers());
    app.Run();
}


