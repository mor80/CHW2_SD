using ZooApplication.Abstractions;
using ZooApplication.Services;
using ZooInfrastructure.Events;
using ZooInfrastructure.Repositories;

namespace ZooPresentation;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSingleton<IAnimalRepository, InMemoryAnimalRepository>();
        builder.Services.AddSingleton<IEnclosureRepository, InMemoryEnclosureRepository>();
        builder.Services.AddSingleton<IFeedingScheduleRepository, InMemoryFeedingScheduleRepository>();
        builder.Services.AddSingleton<IEventDispatcher, InMemoryEventDispatcher>();
        builder.Services.AddSingleton<IFeedStockRepository, InMemoryFeedStockRepository>();
        builder.Services.AddSingleton<IFeedStockRepository, InMemoryFeedStockRepository>();
        builder.Services.AddSingleton<ITreatmentRepository, InMemoryTreatmentRepository>();

        builder.Services.AddScoped<HealthService>();
        builder.Services.AddScoped<AnimalTransferService>();
        builder.Services.AddScoped<FeedingOrganizationService>();
        builder.Services.AddScoped<ZooStatisticsService>();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapControllers();

        app.Run();
    }
}