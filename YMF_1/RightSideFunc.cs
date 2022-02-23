namespace YMF_1;

/// <summary>
/// Right side of differential equation, named f
/// -div(\lambda * grad(u)) + \gamma * u = f
/// </summary>
public static class RightSideFunc
{
    public static readonly Func<double, double, double> Eval = (x, y) => 0.0;

}