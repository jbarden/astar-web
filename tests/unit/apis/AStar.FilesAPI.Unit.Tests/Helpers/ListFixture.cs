using AStar.FilesApi.Controllers;
using AStar.FilesApi.Files;
using AStar.Infrastructure.Data;
using Microsoft.Extensions.Logging.Abstractions;

namespace AStar.FilesAPI.Helpers;

public class ListFixture : IDisposable
{
    private bool disposedValue;

    public ListFixture()
    {
        MockFilesContext = new MockFilesContext().CreateContext();
        SUT = new List(MockFilesContext, NullLogger<List>.Instance);
    }

    public FilesContext MockFilesContext { get; }

    public List SUT { get; }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

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
}
