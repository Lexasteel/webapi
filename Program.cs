using Microsoft.EntityFrameworkCore;
using WebApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mySQLConnection = builder.Configuration.GetConnectionString("MySQLConnection");
builder.Services.AddDbContext<StartsContext>(options =>
options.UseMySql(mySQLConnection, new MySqlServerVersion(new Version(8, 0, 30)), options => options.EnableRetryOnFailure()));
//var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(
//        policy =>
//        {
//            policy.WithOrigins("https://localhost")
//            .AllowAnyHeader()
//            .AllowAnyMethod()
//            .AllowAnyOrigin();
//        });
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyAllowSpecific",
                      policy =>
                      {
                          policy.AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowAnyOrigin();
                          
                      });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

//app.UseAuthorization();
app.UseStaticFiles();
//app.UseCors("MyAllowSpecificOrigins");
app.UseCors("MyAllowSpecific");
app.MapControllers();

app.Run();
