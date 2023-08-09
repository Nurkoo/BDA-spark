namespace BigDataAcademy.Api.Initializers;

public class Initializer
{
    private readonly IEnumerable<IServiceInitializer> serviceSetters;

    public Initializer(IEnumerable<IServiceInitializer> serviceSetters)
    {
        this.serviceSetters = serviceSetters;
    }

    public async Task Initialize()
    {
        await Task.WhenAll(this.serviceSetters.Select(o => o.Initialize()));
    }
}
