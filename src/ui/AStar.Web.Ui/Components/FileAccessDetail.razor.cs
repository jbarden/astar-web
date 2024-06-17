using System.Diagnostics;
using AStar.FilesApi.Client.SDK.FilesApi;
using AStar.Web.UI.ApiClients.ImagesApi;
using AStar.Web.UI.Models;
using Microsoft.AspNetCore.Components;

namespace AStar.Web.UI.Components;

public partial class FileAccessDetail
{
    public string DeletionStatus { get; private set; } = string.Empty;

    [Parameter]
    public Guid FileId { get; set; }

    [Parameter]
    public string FileNameWithPath { get; set; } = string.Empty;

    public AStar.FilesApi.Client.SDK.Models.FileAccessDetail FileAccessDetails { get; set; } = new();

    [Inject]
    private FilesApiClient FilesApiClient { get; set; } = default!;

    protected override async Task OnInitializedAsync()
        => FileAccessDetails = await FilesApiClient.GetFileAccessDetail(FileId);

    private Task OnButtonClicked()
    {
        _ = Process.Start("explorer.exe", FileNameWithPath);

        return Task.CompletedTask;
    }

    private async Task MarkForMoving(Guid fileId)
    {
        var result = await FilesApiClient.MarkForMovingAsync(fileId);

        if(result == "Mark for moving was successful")
        {
            FileAccessDetails.NeedsToMove = true;
        }

        DeletionStatus = result;
    }

    private async Task MarkForSoftDeletion(Guid fileId)
    {
        var result = await FilesApiClient.MarkForSoftDeletionAsync(fileId);

        if(result == "Marked for soft deletion")
        {
            FileAccessDetails.SoftDeletePending = true;
        }

        DeletionStatus = result;
    }

    private async Task MarkForHardDeletion(Guid fileId)
    {
        var result = await FilesApiClient.MarkForHardDeletionAsync(fileId);

        if(result == "Marked for hard deletion.")
        {
            FileAccessDetails.HardDeletePending = true;
        }

        DeletionStatus = result;
    }

    private async Task UndoMarkForSoftDeletion(Guid fileId)
    {
        var result = await FilesApiClient.UndoMarkForSoftDeletionAsync(fileId);

        if(result == "Mark for soft deletion has been undone")
        {
            FileAccessDetails.SoftDeletePending = false;
        }

        DeletionStatus = result;
    }

    private async Task UndoMarkForHardDeletion(Guid fileId)
    {
        var result = await FilesApiClient.UndoMarkForHardDeletionAsync(fileId);

        if(result == "Mark for hard deletion has been undone")
        {
            FileAccessDetails.HardDeletePending = false;
        }

        DeletionStatus = result;
    }

    private async Task UndoMarkForMoving(Guid fileId)
    {
        var result = await FilesApiClient.UndoMarkForMovingAsync(fileId);

        if(result == "Undo mark for moving was successful")
        {
            FileAccessDetails.NeedsToMove = false;
        }

        DeletionStatus = result;
    }
}
