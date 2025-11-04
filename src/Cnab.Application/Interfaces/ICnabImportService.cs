namespace Cnab.Application.Interfaces;

public interface ICnabImportService
{
    Task ImportAsync(Stream cnabStream, CancellationToken cancellationToken = default);
}

