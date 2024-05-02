using AStar.FilesApi.Controllers;
using AStar.FilesApi.Files;
using AStar.Infrastructure.Data;
using Microsoft.Extensions.Logging.Abstractions;

namespace AStar.FilesAPI.Unit.Tests.Helpers;

public class FilesControllerFixture: IDisposable
{
    private bool disposedValue;

    public FilesControllerFixture()
    {
        MockFilesContext = new MockFilesContext().CreateContext();
        SUT = new FilesCounterController(MockFilesContext, NullLogger<FilesControllerBase>.Instance);
    }

    public FilesContext MockFilesContext { get; }

    public FilesCounterController SUT { get; }

    protected virtual void Dispose(bool disposing)
    {
        if(!disposedValue)
        {
            if(disposing)
            {
                MockFilesContext.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
