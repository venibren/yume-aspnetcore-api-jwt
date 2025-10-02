using Asp.Versioning;
using AspNetCoreRateLimit;
using Microsoft.EntityFrameworkCore;
using OtpNet;
using System.Text.Json.Serialization;
using Yume.Data.Contexts;
using Yume.Data.Factories;
using Yume.Middleware;
using Yume.Services;
using Yume.Services.Interfaces;

///////////////////////////////
/// Web Builder
///////////////////////////////
WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IFactoryService, FactoryService>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserFactory>();

builder.Services.AddSingleton(builder.Configuration);

// Authentication
builder.Services.AddJwtAuthentication(settings: builder.Configuration.GetSection("Security:Jwt:Settings"), secret: builder.Configuration["Security:Jwt:Secret"]);

// API
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
        builder.SetIsOriginAllowed(_ => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Rate limiting configuration
builder.Services.AddOptions();
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
builder.Services.AddInMemoryRateLimiting();

builder.Services.AddMvc();

builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version"),
        new MediaTypeApiVersionReader("x-api-version"));
});
builder.Services.AddCustomSwagger();

// Rate limiting configuration
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// Database - PostgreSQL
string postgreConnectionString = builder.Configuration.GetConnectionString("PostgreSql") ?? throw new InvalidOperationException("Connection string 'PostgreSql' not found.");
builder.Services.AddDbContext<PostgreDbContext>(options =>
{
    options.UseNpgsql(postgreConnectionString);

#if DEBUG
    options.EnableSensitiveDataLogging();
    options.EnableDetailedErrors();
#endif
});

if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development)
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

///////////////////////////////
/// App Configuration
///////////////////////////////
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCustomSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

// Rate limiting middleware
app.UseIpRateLimiting();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
