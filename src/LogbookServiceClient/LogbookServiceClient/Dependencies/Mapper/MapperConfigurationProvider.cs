using Logbook.Requests.Logbook;
using LogbookService.Records;

namespace Logbook.Dependencies.Mapper;

public class MapperConfigurationProvider : IServiceProvider
{
    private AutoMapper.IConfigurationProvider ConfigurationProvider { get; init; }

    public MapperConfigurationProvider()
    {
        this.ConfigurationProvider = new AutoMapper.MapperConfiguration(config =>
        {
            config.CreateMap<EditJumpRequest, LoggedJump>();
            config.CreateMap<LogJumpRequest, LoggedJump>();
        });
    }

    public object? GetService(Type serviceType)
    {
        return this.ConfigurationProvider;
    }
}