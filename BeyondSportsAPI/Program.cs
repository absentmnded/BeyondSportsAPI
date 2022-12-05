using Microsoft.EntityFrameworkCore;
using BeyondSportsAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<BeyondSportsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BeyondSportsDbContext")));
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.DateFormatString = "dd-MM-yyyy";
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<BeyondSportsDbContext>();
    context.Database.EnsureCreated();
    DbInitializer.Initialize(context);

}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
