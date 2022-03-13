namespace YMF_1;

/// <summary>
/// Right side of differential equation, named f
/// -div(\lambda * grad(u)) + \gamma * u = f
/// </summary>
public static class RightSideFunc
{
    public static readonly Func<double, double, double> Eval = (x, y) =>
        -12.0 * x * x - 12.0 * y * y + Math.Pow(x, 4) + Math.Pow(y, 4);
}