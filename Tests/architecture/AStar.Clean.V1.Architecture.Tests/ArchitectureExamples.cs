using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using static ArchUnitNET.Fluent.Slices.SliceRuleDefinition;

namespace AStar.Clean.V1.Architecture.Tests;

// https://github.com/TNG/ArchUnitNET
public class ArchitectureExamples
{
    private const string UiName = "AStar.Clean.V1.BlazorUI";
    private const string FilesApiName = "AStar.Clean.V1.Files.API";
    private const string DomainModelName = "AStar.Clean.V1.DomainModel";
    private const string InfrastructureName = "AStar.Clean.V1.Infrastructure";
    private const string ServicesProjectName = "AStar.Clean.V1.Services";

    private static readonly ArchUnitNET.Domain.Architecture Architecture = new ArchLoader().LoadAssemblies(
            System.Reflection.Assembly.Load("AStar.Clean.V1.Architecture.Tests"),
            System.Reflection.Assembly.Load(UiName),
            System.Reflection.Assembly.Load(ServicesProjectName),
            System.Reflection.Assembly.Load(InfrastructureName),
            System.Reflection.Assembly.Load(DomainModelName),
            System.Reflection.Assembly.Load(FilesApiName))
        .Build();

    [Fact]
    public void ClassDependency()
    {
        var rule = Classes().That().AreAssignableTo(typeof(ICar)).Should()
            .NotDependOnAny(Classes().That().AreAssignableTo(typeof(ICanvas)));

        _ = rule.HasNoViolations(Architecture).Should().BeTrue();
    }

    [Fact]
    public void InheritanceNaming()
    {
        var rule = Classes().That().AreAssignableTo(typeof(ICar)).Should()
            .HaveNameContaining("Car");

        _ = rule.HasNoViolations(Architecture).Should().BeTrue();
    }

    [Fact]
    public void ClassNamespaceContainment()
    {
        var rule = Classes().That().HaveNameContaining("Canvas").Should()
            .ResideInNamespace(typeof(ICanvas).Namespace);

        _ = rule.HasNoViolations(Architecture).Should().BeTrue();
    }

    [Fact]
    public void AttributeAccess()
    {
        var rule = Classes().That().DoNotHaveAnyAttributes(typeof(DisplayAttribute)).Should()
            .NotDependOnAny(Classes().That().AreAssignableTo(typeof(ICanvas)));

        _ = rule.HasNoViolations(Architecture).Should().BeTrue();
    }

    [Fact]
    public void Cycles()
    {
        IArchRule rule = Slices().Matching("Module.(*)").Should().BeFreeOfCycles();

        _ = rule.HasNoViolations(Architecture).Should().BeTrue();
    }
}
