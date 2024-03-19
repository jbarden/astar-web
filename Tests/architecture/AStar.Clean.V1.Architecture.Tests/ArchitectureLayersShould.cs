using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using Microsoft.EntityFrameworkCore;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace AStar.Clean.V1.Architecture.Tests;

// https://github.com/TNG/ArchUnitNET
public class ArchitectureLayersShould
{
    private const string UiName = "AStar.Clean.V1.BlazorUI";
    private const string FilesApiName = "AStar.Clean.V1.Files.API";
    private const string ImagesApiName = "AStar.Clean.V1.Images.API";
    private const string DomainModelName = "AStar.Clean.V1.DomainModel";
    private const string InfrastructureName = "AStar.Clean.V1.Infrastructure";
    private const string ServicesProjectName = "AStar.Clean.V1.Services";
    private readonly IObjectProvider<IType> uiLayer;
    private readonly IObjectProvider<IType> filesApiLayer;
    private readonly IObjectProvider<IType> imagesApiLayer;
    private readonly IObjectProvider<IType> domainModelLayer;
    private readonly IObjectProvider<IType> infrastructureLayer;
    private readonly IObjectProvider<IType> servicesLayer;
    private readonly ArchUnitNET.Domain.Architecture architecture;

    public ArchitectureLayersShould()
    {
        uiLayer = Types().That().ResideInAssembly(System.Reflection.Assembly.Load(UiName)).As("UI Layer");
        filesApiLayer = Types().That().ResideInAssembly(System.Reflection.Assembly.Load(FilesApiName)).As("Files API Layer");
        imagesApiLayer = Types().That().ResideInAssembly(System.Reflection.Assembly.Load(ImagesApiName)).As("Images API Layer");
        domainModelLayer = Types().That().ResideInAssembly(System.Reflection.Assembly.Load(DomainModelName)).As("DomainModel Layer");
        infrastructureLayer = Types().That().ResideInAssembly(System.Reflection.Assembly.Load(InfrastructureName)).As("Infrastructure Layer");
        servicesLayer = Types().That().ResideInAssembly(System.Reflection.Assembly.Load(ServicesProjectName)).As("Services Layer");
        architecture = new ArchLoader().LoadAssemblies(
            System.Reflection.Assembly.Load(UiName),
            System.Reflection.Assembly.Load(ServicesProjectName),
            System.Reflection.Assembly.Load(InfrastructureName),
            System.Reflection.Assembly.Load(DomainModelName),
            System.Reflection.Assembly.Load(FilesApiName),
            System.Reflection.Assembly.Load(ImagesApiName))
            .Build();
    }

    [Fact]
    public void NotAllowTheUiToDirectlyDependOnTheFilesApiLayer()
    {
        IArchRule rule = Types().That().Are(uiLayer).Should()
            .NotDependOnAny(filesApiLayer).Because("it should be consumed via the API, not directly");

        _ = rule.HasNoViolations(architecture).Should().BeTrue();
    }

    [Fact]
    public void NotAllowTheUiToDirectlyAccessTheDatabase()
    {
        IArchRule rule = Types().That().Are(uiLayer).Should()
            .NotDependOnAny(Classes().That().AreAssignableTo(typeof(DbContext)));

        _ = rule.HasNoViolations(architecture).Should().BeTrue();
    }

    [Fact]
    public void NotAllowTheFilesApiLayerToDirectlyAccessTheDatabase()
    {
        IArchRule rule = Types().That().Are(filesApiLayer).Should()
            .NotDependOnAny(Classes().That().AreAssignableTo(typeof(DbContext)));

        _ = rule.HasNoViolations(architecture).Should().BeTrue();
    }

    [Fact]
    public void NotAllowTheImagesApiLayerToDirectlyAccessTheDatabase()
    {
        IArchRule rule = Types().That().Are(imagesApiLayer).Should()
            .NotDependOnAny(Classes().That().AreAssignableTo(typeof(DbContext)));

        _ = rule.HasNoViolations(architecture).Should().BeTrue();
    }

    [Fact]
    public void NotAllowTheDomainLayerToDirectlyAccessTheDatabase()
    {
        IArchRule rule = Types().That().Are(domainModelLayer).Should()
            .NotDependOnAny(Classes().That().AreAssignableTo(typeof(DbContext)));

        _ = rule.HasNoViolations(architecture).Should().BeTrue();
    }

    [Fact]
    public void NotAllowTheServicesLayerToDirectlyAccessTheDatabase()
    {
        IArchRule rule = Types().That().Are(servicesLayer).Should()
            .NotDependOnAny(Classes().That().AreAssignableTo(typeof(DbContext)));

        _ = rule.HasNoViolations(architecture).Should().BeTrue();
    }

    [Fact]
    public void NotAllowTheUiToDirectlyDependOnTheServicesLayer()
    {
        IArchRule rule = Types().That().Are(uiLayer).Should()
            .NotDependOnAny(servicesLayer).Because("it should be consumed via the API, not directly");

        _ = rule.HasNoViolations(architecture).Should().BeTrue();
    }

    [Fact]
    public void NotAllowTheUiToDirectlyDependOnTheInfrastructureLayer()
    {
        IArchRule rule = Types().That().Are(uiLayer).Should()
            .NotDependOnAny(infrastructureLayer).Because("it should be consumed via the API, not directly");

        _ = rule.HasNoViolations(architecture).Should().BeTrue();
    }

    [Fact]
    public void NotAllowTheUiToDirectlyDependOnTheDomainModelLayer()
    {
        IArchRule rule = Types().That().Are(uiLayer).Should()
            .NotDependOnAny(domainModelLayer).Because("it should be consumed via the API, not directly");

        _ = rule.HasNoViolations(architecture).Should().BeTrue();
    }

    [Fact]
    public void NotAllowTheFilesApiLayerToDirectlyDependOnTheUiLayer()
    {
        IArchRule rule = Types().That().Are(filesApiLayer).Should()
            .NotDependOnAny(uiLayer).Because("it should not need to access the UI namespace");

        _ = rule.HasNoViolations(architecture).Should().BeTrue();
    }

    [Fact]
    public void NotAllowTheImagesApiLayerToDirectlyDependOnTheUiLayer()
    {
        IArchRule rule = Types().That().Are(imagesApiLayer).Should()
            .NotDependOnAny(uiLayer).Because("it should not need to access the UI namespace");

        _ = rule.HasNoViolations(architecture).Should().BeTrue();
    }

    [Fact]
    public void NotAllowTheFilesApiLayerToDirectlyDependOnTheServicesLayer()
    {
        IArchRule rule = Types().That().Are(filesApiLayer).Should()
            .NotDependOnAny(servicesLayer).Because("it should not need to access the UI namespace");

        _ = rule.HasNoViolations(architecture).Should().BeTrue();
    }

    [Fact]
    public void NotAllowTheImagesApiLayerToDirectlyDependOnTheServicesLayer()
    {
        IArchRule rule = Types().That().Are(imagesApiLayer).Should()
            .NotDependOnAny(servicesLayer).Because("it should not need to access the UI namespace");

        _ = rule.HasNoViolations(architecture).Should().BeTrue();
    }

    [Fact]
    public void NotAllowTheFilesApiLayerToDirectlyDependOnTheInfrastructureLayer()
    {
        IArchRule rule = Types().That().Are(filesApiLayer).Should()
            .NotDependOnAny(infrastructureLayer).Because("am sure we will need to remove this test");

        _ = rule.HasNoViolations(architecture).Should().BeTrue();
    }

    [Fact]
    public void NotAllowTheImagesApiLayerToDirectlyDependOnTheInfrastructureLayer()
    {
        IArchRule rule = Types().That().Are(imagesApiLayer).Should()
            .NotDependOnAny(infrastructureLayer).Because("am sure we will need to remove this test");

        _ = rule.HasNoViolations(architecture).Should().BeTrue();
    }

    [Fact]
    public void NotAllowTheFilesApiLayerToDirectlyDependOnTheDomainModelLayer()
    {
        IArchRule rule = Types().That().Are(filesApiLayer).Should()
            .NotDependOnAny(domainModelLayer).Because("it should not access the Domain Model directly.");

        _ = rule.HasNoViolations(architecture).Should().BeTrue();
    }

    [Fact]
    public void NotAllowTheImagesApiLayerToDirectlyDependOnTheDomainModelLayer()
    {
        IArchRule rule = Types().That().Are(imagesApiLayer).Should()
            .NotDependOnAny(domainModelLayer).Because("it should not access the Domain Model directly.");

        _ = rule.HasNoViolations(architecture).Should().BeTrue();
    }
}
