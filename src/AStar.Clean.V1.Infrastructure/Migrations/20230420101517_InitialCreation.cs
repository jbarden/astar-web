using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AStar.Clean.V1.Infrastructure.Migrations;

/// <inheritdoc />
public partial class InitialCreation : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.EnsureSchema(
            name: "jjb");

        _ = migrationBuilder.CreateTable(
            name: "FileInfo",
            schema: "jjb",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Size = table.Column<long>(type: "bigint", nullable: false),
                DirectoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                DetailsLastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                FileLastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                Height = table.Column<int>(type: "int", nullable: true),
                Width = table.Column<int>(type: "int", nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_FileInfo", x => x.Id));
#pragma warning disable S6588
        _ = migrationBuilder.InsertData(
            schema: "jjb",
            table: "FileInfo",
            columns: new[] { "Id", "DetailsLastUpdated", "DirectoryName", "FileLastUpdated", "FileName", "FullName", "Size" },
            values: new object[,]
            {
                { new Guid("067a9f52-74ac-4a0c-aca8-f787903d5595"), new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1359), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "1. Create a new Wiki.png", "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\1. Create a new Wiki.png", 34448L },
                { new Guid("291e0dbc-a127-4bef-a5f0-bea7f711ea07"), new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1397), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "2. Clone.png", "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\2. Clone.png", 10092L },
                { new Guid("29b6d2e0-50ad-43c5-bcef-a12b3988c6de"), new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1406), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "2.jpg", "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\2.jpg", 14610907L },
                { new Guid("34e38a94-81d0-4816-a726-2778822dd145"), new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1414), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "20220329_094236.jpg", "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\20220329_094236.jpg", 9523777L },
                { new Guid("41f32591-c14f-4867-af3e-7c1e6723ef1c"), new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1422), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "3. Copy the selected file and folder.png", "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\", 45388L },
                { new Guid("42ceae0f-3694-4060-8664-4d5d4e1ba825"), new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1432), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\3. Copy the selected file and folder.png", 1L },
                { new Guid("4f796939-bbeb-4f61-955a-409606378c4d"), new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1440), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "C&CA with MACE.png", "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\C&CA with MACE.png", 10417L },
                { new Guid("581d98f2-2c33-4941-a873-20b6ab7fbe34"), new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1478), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "c&ca-logo-microsoft.png", "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\c&ca-logo-microsoft.png", 20937L },
                { new Guid("5dc05c52-f64b-46f8-9db2-80d8f1b493db"), new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1489), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "c&ca-logo.png", "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\c&ca-logo.png", 30379L },
                { new Guid("6473ad6f-f134-4443-8c8a-c87b19e06989"), new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1498), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "capgemini.png", "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\capgemini.png", 5103L },
                { new Guid("6750d3bc-14a2-43f0-a10f-62bd1286f62c"), new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1506), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "d7e3b9c3d2a0101b74e30f0c448d9879.jfif", "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\d7e3b9c3d2a0101b74e30f0c448d9879.jfif", 65964L },
                { new Guid("69da8471-1fb9-4e91-88c6-b6f8b4445ead"), new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1513), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "dates.webp", "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\dates.webp", 62316L },
                { new Guid("7106bd02-49fe-48ca-8c07-6e849ddf4528"), new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1522), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "desktop.ini", "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\desktop.ini", 520L },
                { new Guid("715c9823-4172-41fe-84d8-82d8bb271744"), new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1529), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "DTX Codes.png", "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\DTX Codes.png", 89072L },
                { new Guid("7294fa9c-998d-48d8-8786-7da77b5e7430"), new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1537), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Example-of-Result-Object-Usage.png", "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\Example-of-Result-Object-Usage.png", 54927L },
                { new Guid("7657ea7d-339e-4a10-9a78-732a8f963556"), new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1546), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Failed.png", "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\Failed.png", 39925L },
                { new Guid("913a36ac-f1ce-4ebd-97fc-f02e6e5ea98c"), new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1553), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Galaxy S21 IMEI.jpg", "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\Galaxy S21 IMEI.jpg", 190660L },
                { new Guid("94b9d0ef-7ab6-4e51-a04c-40a3e818a4c1"), new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1562), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Galaxy S21.pdf", "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\Galaxy S21.pdf", 136996L },
                { new Guid("99937628-fdb8-4190-b3e9-7a8441d10f6e"), new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1570), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Had to change account permissions from reader.png", "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\Had to change account permissions from reader.png", 64552L },
                { new Guid("a393b8fe-84fa-475c-8808-aa17665fbbd9"), new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1581), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Handy.png", "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\Handy.png", 174083L },
                { new Guid("ad871b14-7589-4e4f-b369-a68e20665a12"), new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1589), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Image (6) - Copy.jpg", "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\Image (6) - Copy.jpg", 668673L },
                { new Guid("b2a4af69-15fa-41da-a328-b9941f25df8e"), new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1597), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Image (6)-2.jpg", "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\Image (6)-2.jpg", 112830L },
                { new Guid("b2d1c973-004c-497d-b942-f92eaaeee9ba"), new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1605), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Image (6).jpg", "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\Image (6).jpg", 668673L },
                { new Guid("b7dec2eb-ef63-457f-8b82-da3e08fbdc7d"), new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1613), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "JayB.jpg", "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\JayB.jpg", 15417L },
                { new Guid("b7dec2eb-ef63-457f-8b82-da3e08fbdc7e"), new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1620), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MACE.png", "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\MACE.png", 1583L },
                { new Guid("c888cf12-63e2-41a5-a5be-3d9570855b81"), new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1628), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "maxresdefault.jpg", "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\maxresdefault.jpg", 67614L },
                { new Guid("d439a109-cb95-4e3c-afe8-984922b72806"), new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1635), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "o2 in PINK.jpg", "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\o2 in PINK.jpg", 117992L },
                { new Guid("dbcdad60-7fd1-4de3-b6bb-67c93448bc22"), new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1642), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Screenshot 2020-10-12 092159.png", "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\Screenshot 2020-10-12 092159.png", 450910L },
                { new Guid("e0316f73-6ef7-4289-be97-27efd93b1eab"), new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1649), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "signature.jpg", "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\signature.jpg", 11712L },
                { new Guid("e0316f73-6ef7-4289-be97-27efd93b1eac"), new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1656), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Solution Structure.png", "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\Solution Structure.png", 58855L }
            });

#pragma warning restore S6588
        _ = migrationBuilder.CreateIndex(
            name: "IX_Size_Descending",
            schema: "jjb",
            table: "FileInfo",
            column: "Size",
            descending: Array.Empty<bool>());
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable(
            name: "FileInfo",
            schema: "jjb");
}
