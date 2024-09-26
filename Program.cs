using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using project.Database;
using project.Database.Configurations;

var builder = WebApplication.CreateBuilder(args);

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
// Add services to the container.

try
{
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddDbContext<StudentDbContext>(options =>
     options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch(Exception ex)
{
    logger.Error(ex, "Stopped program because of exceptions");
}
finally
{
    LogManager.Shutdown();
}