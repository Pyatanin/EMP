namespace YMF_2.Fem;

public static class LinearBasis
{
    public static readonly Func<double, double>[] Func =
    {
        x => 1.0 - x,
        x => x
    };
}