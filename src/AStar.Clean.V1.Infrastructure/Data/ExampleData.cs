using AStar.Clean.V1.DomainModel;
using Microsoft.EntityFrameworkCore;

namespace AStar.Clean.V1.Infrastructure.Data;

internal static class ExampleData
{
    public static void SeedData(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<FileDetail>().HasData(new FileDetail
        {
            FileName = "1. Create a new Wiki.png",
            DirectoryName = "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures",
            FileSize = 34448
        });
        _ = modelBuilder.Entity<FileDetail>().HasData(new FileDetail
        {
            FileName = "2. Clone.png",
            DirectoryName = "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures",
            FileSize = 10092
        });
        _ = modelBuilder.Entity<FileDetail>().HasData(new FileDetail
        {
            FileName = "2.jpg",
            DirectoryName = "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures",
            FileSize = 14610907
        });
        _ = modelBuilder.Entity<FileDetail>().HasData(new FileDetail
        {
            FileName = "20220329_094236.jpg",
            DirectoryName = "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures",
            FileSize = 9523777
        });
        _ = modelBuilder.Entity<FileDetail>().HasData(new FileDetail
        {
            FileName = "3. Copy the selected file and folder.png",
            DirectoryName = "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures",
            FileSize = 45388
        });
        _ = modelBuilder.Entity<FileDetail>().HasData(new FileDetail
        {
            FileName = "",
            DirectoryName = "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures",
            FileSize = 1
        });
        _ = modelBuilder.Entity<FileDetail>().HasData(new FileDetail
        {
            FileName = "C&CA with MACE.png",
            DirectoryName = "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures",
            FileSize = 10417
        });
        _ = modelBuilder.Entity<FileDetail>().HasData(new FileDetail
        {
            FileName = "c&ca-logo-microsoft.png",
            DirectoryName = "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures",
            FileSize = 20937
        });
        _ = modelBuilder.Entity<FileDetail>().HasData(new FileDetail
        {
            FileName = "c&ca-logo.png",
            DirectoryName = "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures",
            FileSize = 30379
        });
        _ = modelBuilder.Entity<FileDetail>().HasData(new FileDetail
        {
            FileName = "capgemini.png",
            DirectoryName = "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures",
            FileSize = 5103
        });
        _ = modelBuilder.Entity<FileDetail>().HasData(new FileDetail
        {
            FileName = "d7e3b9c3d2a0101b74e30f0c448d9879.jfif",
            DirectoryName = "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures",
            FileSize = 65964
        });
        _ = modelBuilder.Entity<FileDetail>().HasData(new FileDetail
        {
            FileName = "dates.webp",
            DirectoryName = "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures",
            FileSize = 62316
        });
        _ = modelBuilder.Entity<FileDetail>().HasData(new FileDetail
        {
            FileName = "desktop.ini",
            DirectoryName = "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures",
            FileSize = 520
        });
        _ = modelBuilder.Entity<FileDetail>().HasData(new FileDetail
        {
            FileName = "DTX Codes.png",
            DirectoryName = "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures",
            FileSize = 89072
        });
        _ = modelBuilder.Entity<FileDetail>().HasData(new FileDetail
        {
            FileName = "Example-of-Result-Object-Usage.png",
            DirectoryName = "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures",
            FileSize = 54927
        });
        _ = modelBuilder.Entity<FileDetail>().HasData(new FileDetail
        {
            FileName = "Failed.png",
            DirectoryName = "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures",
            FileSize = 39925
        });
        _ = modelBuilder.Entity<FileDetail>().HasData(new FileDetail
        {
            FileName = "Galaxy S21 IMEI.jpg",
            DirectoryName = "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures",
            FileSize = 190660
        });
        _ = modelBuilder.Entity<FileDetail>().HasData(new FileDetail
        {
            FileName = "Galaxy S21.pdf",
            DirectoryName = "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures",
            FileSize = 136996
        });
        _ = modelBuilder.Entity<FileDetail>().HasData(new FileDetail
        {
            FileName = "Had to change account permissions from reader.png",
            DirectoryName = "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures",
            FileSize = 64552
        });
        _ = modelBuilder.Entity<FileDetail>().HasData(new FileDetail
        {
            FileName = "Handy.png",
            DirectoryName = "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures",
            FileSize = 174083
        });
        _ = modelBuilder.Entity<FileDetail>().HasData(new FileDetail
        {
            FileName = "Image (6) - Copy.jpg",
            DirectoryName = "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures",
            FileSize = 668673
        });
        _ = modelBuilder.Entity<FileDetail>().HasData(new FileDetail
        {
            FileName = "Image (6)-2.jpg",
            DirectoryName = "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures",
            FileSize = 112830
        });
        _ = modelBuilder.Entity<FileDetail>().HasData(new FileDetail
        {
            FileName = "Image (6).jpg",
            DirectoryName = "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures",
            FileSize = 668673
        });
        _ = modelBuilder.Entity<FileDetail>().HasData(new FileDetail
        {
            FileName = "JayB.jpg",
            DirectoryName = "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures",
            FileSize = 15417
        });
        _ = modelBuilder.Entity<FileDetail>().HasData(new FileDetail
        {
            FileName = "MACE.png",
            DirectoryName = "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures",
            FileSize = 1583
        });
        _ = modelBuilder.Entity<FileDetail>().HasData(new FileDetail
        {
            FileName = "maxresdefault.jpg",
            DirectoryName = "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures",
            FileSize = 67614
        });
        _ = modelBuilder.Entity<FileDetail>().HasData(new FileDetail
        {
            FileName = "o2 in PINK.jpg",
            DirectoryName = "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures",
            FileSize = 117992
        });
        _ = modelBuilder.Entity<FileDetail>().HasData(new FileDetail
        {
            FileName = "Screenshot 2020-10-12 092159.png",
            DirectoryName = "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures",
            FileSize = 450910
        });
        _ = modelBuilder.Entity<FileDetail>().HasData(new FileDetail
        {
            FileName = "signature.jpg",
            DirectoryName = "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures",
            FileSize = 11712
        });
        _ = modelBuilder.Entity<FileDetail>().HasData(new FileDetail
        {
            FileName = "Solution Structure.png",
            DirectoryName = "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures",
            FileSize = 58855
        });
    }
}
