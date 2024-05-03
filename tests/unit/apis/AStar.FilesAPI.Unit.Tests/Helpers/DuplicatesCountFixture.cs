using AStar.FilesApi.Files;
using AStar.Infrastructure.Data;
using Microsoft.Extensions.Logging.Abstractions;

namespace AStar.FilesAPI.Helpers;

public class DuplicatesCountFixture : IDisposable
{
    private bool disposedValue;

    public DuplicatesCountFixture()
    {
        MockFilesContext = new MockFilesContext().CreateContext();
        SUT = new DuplicatesCount(MockFilesContext, NullLogger<DuplicatesCount>.Instance);
    }

    public FilesContext MockFilesContext { get; }

    public DuplicatesCount SUT { get; }

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
