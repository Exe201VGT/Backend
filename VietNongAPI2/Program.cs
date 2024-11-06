using BOs.Models;
using Microsoft.OpenApi.Models;
using VietNongAPI2.AppStarts;
using BusinessLayer.Service;
using BusinessLayer.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddAutoMapper(typeof(AutoMapperConfig).Assembly);
builder.Services.InstallService(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Please enter a valid token"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


builder.Services.AddWebAPIService();


//odata
builder.Services.AddControllers().AddOData(option => option.Select().Filter()
.Count().OrderBy().Expand().SetMaxTop(100)
.AddRouteComponents("odata", GetEdmModel()));

builder.Services.ConfigureAuthService(builder.Configuration);
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseODataBatching();



//test middleware
app.Use(async (context, next) =>
{
    var endpoint = context.GetEndpoint();
    if (endpoint == null)
    {
        await next(); // Gọi middleware tiếp theo
        return;
    }

    IEnumerable<string> templates;
    IODataRoutingMetadata metadata =
        endpoint.Metadata.GetMetadata<IODataRoutingMetadata>();
    if (metadata != null)
    {
        templates = metadata.Template.GetTemplates();
    }

    await next(); // Gọi middleware tiếp theo
});


app.UseAuthentication();
app.UseAuthorization();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    builder.EntitySet<Category>("Category");
    builder.EntitySet<Role>("Role");
    
    var categoryEntity = builder.EntityType<Category>();
    categoryEntity.HasKey(c => c.CategoryId);
    
    var roleEntity = builder.EntityType<Role>();
    roleEntity.HasKey(r => r.RoleId);
   
    return builder.GetEdmModel();
}