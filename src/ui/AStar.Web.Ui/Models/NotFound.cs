namespace AStar.Web.UI.Models;

public static class NotFound
{
    public static byte[] Image => File.ReadAllBytes("404.jpg");
}
