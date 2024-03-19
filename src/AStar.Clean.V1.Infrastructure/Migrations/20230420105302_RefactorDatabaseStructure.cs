using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AStar.Clean.V1.Infrastructure.Migrations;

/// <inheritdoc />
public partial class RefactorDatabaseStructure : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropColumn(
            name: "FullName",
            schema: "jjb",
            table: "FileInfo");

        _ = migrationBuilder.AlterColumn<string>(
            name: "DirectoryName",
            schema: "jjb",
            table: "FileInfo",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("067a9f52-74ac-4a0c-aca8-f787903d5595"),
            column: "DetailsLastUpdated",
            value: new DateTime(2023, 4, 20, 10, 53, 2, 341, DateTimeKind.Utc).AddTicks(1167));

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("291e0dbc-a127-4bef-a5f0-bea7f711ea07"),
            column: "DetailsLastUpdated",
            value: new DateTime(2023, 4, 20, 10, 53, 2, 341, DateTimeKind.Utc).AddTicks(1205));

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("29b6d2e0-50ad-43c5-bcef-a12b3988c6de"),
            column: "DetailsLastUpdated",
            value: new DateTime(2023, 4, 20, 10, 53, 2, 341, DateTimeKind.Utc).AddTicks(1214));

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("34e38a94-81d0-4816-a726-2778822dd145"),
            column: "DetailsLastUpdated",
            value: new DateTime(2023, 4, 20, 10, 53, 2, 341, DateTimeKind.Utc).AddTicks(1221));

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("41f32591-c14f-4867-af3e-7c1e6723ef1c"),
            column: "DetailsLastUpdated",
            value: new DateTime(2023, 4, 20, 10, 53, 2, 341, DateTimeKind.Utc).AddTicks(1228));

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("42ceae0f-3694-4060-8664-4d5d4e1ba825"),
            column: "DetailsLastUpdated",
            value: new DateTime(2023, 4, 20, 10, 53, 2, 341, DateTimeKind.Utc).AddTicks(1237));

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("4f796939-bbeb-4f61-955a-409606378c4d"),
            column: "DetailsLastUpdated",
            value: new DateTime(2023, 4, 20, 10, 53, 2, 341, DateTimeKind.Utc).AddTicks(1245));

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("581d98f2-2c33-4941-a873-20b6ab7fbe34"),
            column: "DetailsLastUpdated",
            value: new DateTime(2023, 4, 20, 10, 53, 2, 341, DateTimeKind.Utc).AddTicks(1253));

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("5dc05c52-f64b-46f8-9db2-80d8f1b493db"),
            column: "DetailsLastUpdated",
            value: new DateTime(2023, 4, 20, 10, 53, 2, 341, DateTimeKind.Utc).AddTicks(1260));

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("6473ad6f-f134-4443-8c8a-c87b19e06989"),
            column: "DetailsLastUpdated",
            value: new DateTime(2023, 4, 20, 10, 53, 2, 341, DateTimeKind.Utc).AddTicks(1268));

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("6750d3bc-14a2-43f0-a10f-62bd1286f62c"),
            column: "DetailsLastUpdated",
            value: new DateTime(2023, 4, 20, 10, 53, 2, 341, DateTimeKind.Utc).AddTicks(1275));

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("69da8471-1fb9-4e91-88c6-b6f8b4445ead"),
            column: "DetailsLastUpdated",
            value: new DateTime(2023, 4, 20, 10, 53, 2, 341, DateTimeKind.Utc).AddTicks(1282));

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("7106bd02-49fe-48ca-8c07-6e849ddf4528"),
            column: "DetailsLastUpdated",
            value: new DateTime(2023, 4, 20, 10, 53, 2, 341, DateTimeKind.Utc).AddTicks(1289));

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("715c9823-4172-41fe-84d8-82d8bb271744"),
            column: "DetailsLastUpdated",
            value: new DateTime(2023, 4, 20, 10, 53, 2, 341, DateTimeKind.Utc).AddTicks(1296));

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("7294fa9c-998d-48d8-8786-7da77b5e7430"),
            column: "DetailsLastUpdated",
            value: new DateTime(2023, 4, 20, 10, 53, 2, 341, DateTimeKind.Utc).AddTicks(1303));

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("7657ea7d-339e-4a10-9a78-732a8f963556"),
            column: "DetailsLastUpdated",
            value: new DateTime(2023, 4, 20, 10, 53, 2, 341, DateTimeKind.Utc).AddTicks(1310));

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("913a36ac-f1ce-4ebd-97fc-f02e6e5ea98c"),
            column: "DetailsLastUpdated",
            value: new DateTime(2023, 4, 20, 10, 53, 2, 341, DateTimeKind.Utc).AddTicks(1317));

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("94b9d0ef-7ab6-4e51-a04c-40a3e818a4c1"),
            column: "DetailsLastUpdated",
            value: new DateTime(2023, 4, 20, 10, 53, 2, 341, DateTimeKind.Utc).AddTicks(1325));

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("99937628-fdb8-4190-b3e9-7a8441d10f6e"),
            column: "DetailsLastUpdated",
            value: new DateTime(2023, 4, 20, 10, 53, 2, 341, DateTimeKind.Utc).AddTicks(1332));

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("a393b8fe-84fa-475c-8808-aa17665fbbd9"),
            column: "DetailsLastUpdated",
            value: new DateTime(2023, 4, 20, 10, 53, 2, 341, DateTimeKind.Utc).AddTicks(1339));

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("ad871b14-7589-4e4f-b369-a68e20665a12"),
            column: "DetailsLastUpdated",
            value: new DateTime(2023, 4, 20, 10, 53, 2, 341, DateTimeKind.Utc).AddTicks(1345));

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("b2a4af69-15fa-41da-a328-b9941f25df8e"),
            column: "DetailsLastUpdated",
            value: new DateTime(2023, 4, 20, 10, 53, 2, 341, DateTimeKind.Utc).AddTicks(1352));

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("b2d1c973-004c-497d-b942-f92eaaeee9ba"),
            column: "DetailsLastUpdated",
            value: new DateTime(2023, 4, 20, 10, 53, 2, 341, DateTimeKind.Utc).AddTicks(1358));

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("b7dec2eb-ef63-457f-8b82-da3e08fbdc7d"),
            column: "DetailsLastUpdated",
            value: new DateTime(2023, 4, 20, 10, 53, 2, 341, DateTimeKind.Utc).AddTicks(1365));

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("b7dec2eb-ef63-457f-8b82-da3e08fbdc7e"),
            column: "DetailsLastUpdated",
            value: new DateTime(2023, 4, 20, 10, 53, 2, 341, DateTimeKind.Utc).AddTicks(1400));

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("c888cf12-63e2-41a5-a5be-3d9570855b81"),
            column: "DetailsLastUpdated",
            value: new DateTime(2023, 4, 20, 10, 53, 2, 341, DateTimeKind.Utc).AddTicks(1408));

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("d439a109-cb95-4e3c-afe8-984922b72806"),
            column: "DetailsLastUpdated",
            value: new DateTime(2023, 4, 20, 10, 53, 2, 341, DateTimeKind.Utc).AddTicks(1415));

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("dbcdad60-7fd1-4de3-b6bb-67c93448bc22"),
            column: "DetailsLastUpdated",
            value: new DateTime(2023, 4, 20, 10, 53, 2, 341, DateTimeKind.Utc).AddTicks(1422));

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("e0316f73-6ef7-4289-be97-27efd93b1eab"),
            column: "DetailsLastUpdated",
            value: new DateTime(2023, 4, 20, 10, 53, 2, 341, DateTimeKind.Utc).AddTicks(1430));

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("e0316f73-6ef7-4289-be97-27efd93b1eac"),
            column: "DetailsLastUpdated",
            value: new DateTime(2023, 4, 20, 10, 53, 2, 341, DateTimeKind.Utc).AddTicks(1437));
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.AlterColumn<string>(
            name: "DirectoryName",
            schema: "jjb",
            table: "FileInfo",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        _ = migrationBuilder.AddColumn<string>(
            name: "FullName",
            schema: "jjb",
            table: "FileInfo",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("067a9f52-74ac-4a0c-aca8-f787903d5595"),
            columns: new[] { "DetailsLastUpdated", "FullName" },
            values: new object[] { new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1359), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\1. Create a new Wiki.png" });

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("291e0dbc-a127-4bef-a5f0-bea7f711ea07"),
            columns: new[] { "DetailsLastUpdated", "FullName" },
            values: new object[] { new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1397), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\2. Clone.png" });

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("29b6d2e0-50ad-43c5-bcef-a12b3988c6de"),
            columns: new[] { "DetailsLastUpdated", "FullName" },
            values: new object[] { new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1406), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\2.jpg" });

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("34e38a94-81d0-4816-a726-2778822dd145"),
            columns: new[] { "DetailsLastUpdated", "FullName" },
            values: new object[] { new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1414), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\20220329_094236.jpg" });

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("41f32591-c14f-4867-af3e-7c1e6723ef1c"),
            columns: new[] { "DetailsLastUpdated", "FullName" },
            values: new object[] { new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1422), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\" });

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("42ceae0f-3694-4060-8664-4d5d4e1ba825"),
            columns: new[] { "DetailsLastUpdated", "FullName" },
            values: new object[] { new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1432), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\3. Copy the selected file and folder.png" });

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("4f796939-bbeb-4f61-955a-409606378c4d"),
            columns: new[] { "DetailsLastUpdated", "FullName" },
            values: new object[] { new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1440), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\C&CA with MACE.png" });

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("581d98f2-2c33-4941-a873-20b6ab7fbe34"),
            columns: new[] { "DetailsLastUpdated", "FullName" },
            values: new object[] { new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1478), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\c&ca-logo-microsoft.png" });

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("5dc05c52-f64b-46f8-9db2-80d8f1b493db"),
            columns: new[] { "DetailsLastUpdated", "FullName" },
            values: new object[] { new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1489), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\c&ca-logo.png" });

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("6473ad6f-f134-4443-8c8a-c87b19e06989"),
            columns: new[] { "DetailsLastUpdated", "FullName" },
            values: new object[] { new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1498), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\capgemini.png" });

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("6750d3bc-14a2-43f0-a10f-62bd1286f62c"),
            columns: new[] { "DetailsLastUpdated", "FullName" },
            values: new object[] { new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1506), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\d7e3b9c3d2a0101b74e30f0c448d9879.jfif" });

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("69da8471-1fb9-4e91-88c6-b6f8b4445ead"),
            columns: new[] { "DetailsLastUpdated", "FullName" },
            values: new object[] { new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1513), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\dates.webp" });

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("7106bd02-49fe-48ca-8c07-6e849ddf4528"),
            columns: new[] { "DetailsLastUpdated", "FullName" },
            values: new object[] { new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1522), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\desktop.ini" });

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("715c9823-4172-41fe-84d8-82d8bb271744"),
            columns: new[] { "DetailsLastUpdated", "FullName" },
            values: new object[] { new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1529), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\DTX Codes.png" });

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("7294fa9c-998d-48d8-8786-7da77b5e7430"),
            columns: new[] { "DetailsLastUpdated", "FullName" },
            values: new object[] { new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1537), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\Example-of-Result-Object-Usage.png" });

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("7657ea7d-339e-4a10-9a78-732a8f963556"),
            columns: new[] { "DetailsLastUpdated", "FullName" },
            values: new object[] { new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1546), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\Failed.png" });

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("913a36ac-f1ce-4ebd-97fc-f02e6e5ea98c"),
            columns: new[] { "DetailsLastUpdated", "FullName" },
            values: new object[] { new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1553), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\Galaxy S21 IMEI.jpg" });

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("94b9d0ef-7ab6-4e51-a04c-40a3e818a4c1"),
            columns: new[] { "DetailsLastUpdated", "FullName" },
            values: new object[] { new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1562), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\Galaxy S21.pdf" });

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("99937628-fdb8-4190-b3e9-7a8441d10f6e"),
            columns: new[] { "DetailsLastUpdated", "FullName" },
            values: new object[] { new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1570), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\Had to change account permissions from reader.png" });

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("a393b8fe-84fa-475c-8808-aa17665fbbd9"),
            columns: new[] { "DetailsLastUpdated", "FullName" },
            values: new object[] { new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1581), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\Handy.png" });

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("ad871b14-7589-4e4f-b369-a68e20665a12"),
            columns: new[] { "DetailsLastUpdated", "FullName" },
            values: new object[] { new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1589), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\Image (6) - Copy.jpg" });

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("b2a4af69-15fa-41da-a328-b9941f25df8e"),
            columns: new[] { "DetailsLastUpdated", "FullName" },
            values: new object[] { new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1597), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\Image (6)-2.jpg" });

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("b2d1c973-004c-497d-b942-f92eaaeee9ba"),
            columns: new[] { "DetailsLastUpdated", "FullName" },
            values: new object[] { new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1605), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\Image (6).jpg" });

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("b7dec2eb-ef63-457f-8b82-da3e08fbdc7d"),
            columns: new[] { "DetailsLastUpdated", "FullName" },
            values: new object[] { new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1613), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\JayB.jpg" });

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("b7dec2eb-ef63-457f-8b82-da3e08fbdc7e"),
            columns: new[] { "DetailsLastUpdated", "FullName" },
            values: new object[] { new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1620), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\MACE.png" });

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("c888cf12-63e2-41a5-a5be-3d9570855b81"),
            columns: new[] { "DetailsLastUpdated", "FullName" },
            values: new object[] { new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1628), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\maxresdefault.jpg" });

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("d439a109-cb95-4e3c-afe8-984922b72806"),
            columns: new[] { "DetailsLastUpdated", "FullName" },
            values: new object[] { new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1635), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\o2 in PINK.jpg" });

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("dbcdad60-7fd1-4de3-b6bb-67c93448bc22"),
            columns: new[] { "DetailsLastUpdated", "FullName" },
            values: new object[] { new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1642), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\Screenshot 2020-10-12 092159.png" });

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("e0316f73-6ef7-4289-be97-27efd93b1eab"),
            columns: new[] { "DetailsLastUpdated", "FullName" },
            values: new object[] { new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1649), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\signature.jpg" });

        _ = migrationBuilder.UpdateData(
            schema: "jjb",
            table: "FileInfo",
            keyColumn: "Id",
            keyValue: new Guid("e0316f73-6ef7-4289-be97-27efd93b1eac"),
            columns: new[] { "DetailsLastUpdated", "FullName" },
            values: new object[] { new DateTime(2023, 4, 20, 10, 15, 17, 35, DateTimeKind.Utc).AddTicks(1656), "C:\\Users\\jbarden\\OneDrive - Capgemini\\Pictures\\Solution Structure.png" });
    }
}
