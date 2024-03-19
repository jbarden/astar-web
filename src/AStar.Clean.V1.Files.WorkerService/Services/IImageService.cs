using AStar.Clean.V1.DomainModel;

namespace AStar.Clean.V1.Files.WorkerService.Services;

public interface IImageService
{
    Dimensions GetImage(string imagePath);
}
