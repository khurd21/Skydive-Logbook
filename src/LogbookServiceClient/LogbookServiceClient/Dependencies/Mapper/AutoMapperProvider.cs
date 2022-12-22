namespace Logbook.Dependencies.Mapper;

public class AutoMapperProvider : IServiceProvider
{
    private AutoMapper.IMapper Mapper { get; init; }

    public AutoMapperProvider(AutoMapper.IConfigurationProvider configuration)
    {
        this.Mapper = new AutoMapper.Mapper(configuration);
    }

    public object? GetService(Type serviceType)
    {
        return this.Mapper;
    }
}