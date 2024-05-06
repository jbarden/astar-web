namespace AStar.Web.UI.Services;

public class PaginationService
{
    public IReadOnlyCollection<int> GetPaginationInformation(int totalPageCount)
    {
        if(totalPageCount == 0)
        {
            return [];
        }

        if(totalPageCount <= 10)
        {
            return (IReadOnlyCollection<int>)Enumerable.Range(1, totalPageCount);
        }
        else
        {
            const int requiredPageCount = 5;

            return Enumerable.Range(1, requiredPageCount).Union(Enumerable.Range(totalPageCount - 4, requiredPageCount)).ToList();
        }
    }
}
