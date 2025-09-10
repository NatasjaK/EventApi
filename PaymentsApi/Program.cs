using PaymentsApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PaymentsDb>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("PaymentsDb")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PaymentsDb>();
    await db.Database.MigrateAsync();
    await PaymentsSeeder.SeedAsync(db);
}
app.Run();
