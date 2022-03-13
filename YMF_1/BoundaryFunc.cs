namespace YMF_1;

public static class BoundaryFunc
{
    //    GRID TYPE
    //   +--Upper----
    //   |         RightUpper
    //   |          +
    //   |     |LowerRight
    //   |     |
    //   |     |
    // Left   RightLower
    //   |     |
    //   |     |
    //   +-----+
    //    LowerLeft

    public static Dictionary<string, Func<double, double, double>> First = new()
    {
        {
            "Left", (x, y) => Math.Pow(y, 4)
        },
        {
            "Upper", (x, y) =>81.0 + Math.Pow(x, 4)
        },
        {
            "RightUpper", (x, y) => 81.0 + Math.Pow(y, 4)
        },
        {
            "LowerRight", (x, y) => 16.0 + Math.Pow(x, 4)
        },
        {
            "RightLower", (x, y) => 1.0 + Math.Pow(y, 4)
        },
        {
            "LowerLeft", (x, y) => Math.Pow(x, 4)
        }
    };

    public static Dictionary<string, Func<double, double, double>> Second = new()
    {
        {
            "Left", (x, y) => throw new Exception("Illegal access!")
        },
        {
            "Upper", (x, y) => throw new Exception("Illegal access!")
        },
        {
            "RightUpper", (x, y) => throw new Exception("Illegal access!")
        },
        {
            "LowerRight", (x, y) => throw new Exception("Illegal access!")
        },
        {
            "RightLower", (x, y) => throw new Exception("Illegal access!")
        },
        {
            "LowerLeft", (x, y) => throw new Exception("Illegal access!")
        }
    };
}