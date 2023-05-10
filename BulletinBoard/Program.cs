using BulletinBoard.Common;
using BulletinBoard.ServiceModel.Validators;
using Dal;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Services;
using Services.Mapping;
using Services.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var mvcBuilder = builder.Services.AddControllers().AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
        options.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy())
        );
    }
);

mvcBuilder.AddFluentValidation(x =>
{
    x.RegisterValidatorsFromAssemblyContaining<CreateUserRequestValidator>();
    x.RegisterValidatorsFromAssemblyContaining<CreateAdvertRequestValidator>();
});

mvcBuilder.AddMvcOptions(x => x.Filters.Add<ModelStateFilter>());


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGenNewtonsoftSupport();
builder.Services.Configure<AdvertOptions>(builder.Configuration.GetSection(AdvertOptions.Options));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAdvertService, AdvertService>();
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
