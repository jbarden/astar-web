using AStar.FilesApi.Files;
using AStar.Infrastructure.Data;
using Microsoft.Extensions.Logging.Abstractions;

namespace AStar.FilesAPI.Helpers;

public class MarkForDeletionFixture : IDisposable
{
    private bool disposedValue;

    public MarkForDeletionFixture()
    {
        MockFilesContext = new MockFilesContext().CreateContext();
        SUT = new MarkForDeletion(MockFilesContext, NullLogger<MarkForDeletion>.Instance);
    }

    public FilesContext MockFilesContext { get; }

    public MarkForDeletion SUT { get; }

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
