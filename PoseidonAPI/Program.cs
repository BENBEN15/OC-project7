using PoseidonAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PoseidonAPI.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

///Dbcontext
builder.Services.AddDbContext<PoseidonDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
///Identity
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<PoseidonDBContext>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
