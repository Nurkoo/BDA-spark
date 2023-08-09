namespace BigDataAcademy.Api.Jobs;

public interface IJob
{
    public string CronExpression { get; }

    public Task Execute(CancellationToken cancellationToken = default);
}
