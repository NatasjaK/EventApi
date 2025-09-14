using AutoMapper;
using EventApi.Clients;
using EventApi.Data;
using EventApi.Mapping;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var paymentsBaseUrl = builder.Configuration["PaymentsApi:BaseUrl"];
if (string.IsNullOrWhiteSpace(paymentsBaseUrl))
{
    throw new InvalidOperationException("Missing configuration: PaymentsApi:BaseUrl");
}

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddCors(o => o.AddPolicy("AllowFrontend", p =>
    p.WithOrigins("http://localhost:5173", "https://localhost:5173")
     .AllowAnyHeader()
     .AllowAnyMethod()
));

builder.Services.AddControllers()
    .AddJsonOptions(o =>
        o.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<IPaymentsClient, PaymentsClient>(c =>
{
    c.BaseAddress = new Uri(paymentsBaseUrl!);
});

var app = builder.Build();
app.UseStaticFiles();

app.Logger.LogInformation("PaymentsApi BaseUrl = {Url}", paymentsBaseUrl);

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => Results.Redirect("/swagger"));
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapGet("/_config", (IConfiguration cfg) => Results.Ok(new
{
    paymentsBaseUrl = cfg["PaymentsApi:BaseUrl"],
    hasDbConn = !string.IsNullOrWhiteSpace(cfg.GetConnectionString("DefaultConnection"))
}));

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    await DbSeeder.SeedAsync(db);
}

app.Run();
