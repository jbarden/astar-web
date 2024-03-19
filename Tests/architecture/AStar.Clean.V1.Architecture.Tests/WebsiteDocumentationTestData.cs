// ReSharper disable InconsistentNaming
// ReSharper disable SuggestVarOrType_SimpleTypes
#pragma warning disable 169
#pragma warning disable IDE0052
#pragma warning disable CA1822

namespace AStar.Clean.V1.Architecture.Tests;

// There are class definitions below that are commented out.
// They exist to allow you to uncomment and see the effect (failure) on the tests,
public interface ICar
{
}
#pragma warning disable S4487
#pragma warning disable S125
//public class FastCar : ICar
//{
//    public void IllegalAccess(ICanvas canvas)
//    {
//        // Method intentionally left empty.
//    }
//}

//public class SlowCar : ICar
//{
//    private readonly ICanvas _canvas;

//    public SlowCar(StartCanvas canvas)
//    {
//        _canvas = canvas;
//    }
//}

//public class SlowRocket : ICar
//{
//}

public interface ICanvas
{
}

public class StartCanvas : ICanvas
{
}

[AttributeUsage(AttributeTargets.Class)]
public sealed class DisplayAttribute : Attribute
{
}

public class Steering
{
    private readonly ICar _car;

    public Steering(ICar car)
    {
        _car = car;
    }

    public void ApplySteering()
    {
        // Method intentionally left empty.
    }
}

internal class EndCanvas : ICanvas
{
}

[Display]
internal class StartButton
{
    private readonly ICanvas _startCanvas;

    public StartButton()
    {
        _startCanvas = new StartCanvas();
    }
}

//internal class EndButton
//{
//    private readonly ICanvas _endCanvas;

//    public EndButton()
//    {
//        _endCanvas = new EndCanvas();
//    }
//}

#pragma warning disable S2094
internal class ModuleOneClassOne
{
}

internal class ModuleOneClassTwo
{
}

internal class ModuleTwoClassOne
{
}

internal class ModuleTwoClassTwo
{
}

internal class ModuleThreeClassOne
{
}

internal class ModuleThreeClassTwo
{
}

#pragma warning restore 169
#pragma warning restore IDE0052
#pragma warning restore CA1822

#pragma warning restore S4487
#pragma warning restore S2094
#pragma warning restore S125