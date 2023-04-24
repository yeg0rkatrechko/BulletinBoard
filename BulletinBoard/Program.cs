using Models;
using Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.Converters.Add(new StringEnumConverter
    {
        CamelCaseText = true
    })
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGenNewtonsoftSupport();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AdvertService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddDbContext<BulletinBoardDbContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
