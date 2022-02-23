namespace YMF_1;

public static class BoundaryFunc
{
    //    GRID TYPE
    //   +--Upper----
    //   |         LeftUpper
    //   |          +
    //   |     |LowerRight
    //   |     |
    //   |     |
    // Left   LeftLower
    //   |     |
    //   |     |
    //   +-----+
    //    LowerLeft
    
    public static Dictionary<string, Func<double, double, double>> First = new()
    {
        {
            "Left", (x, y) => x + y
        },
        {
            "Upper", (x, y) => x + y
        },
        {
            "RightUpper", (x, y) => x + y
        },
        {
            "LowerRight", (x, y) => x + y
        },
        {
            "RightLower", (x, y) => x + y
        },
        {
            "LowerLeft", (x, y) => x + y
        }
    };
    
    public static Dictionary<string, Func<double, double, double>> Second = new()
    {
        {
            "Left", (x, y) => x + y
        },
        {
            "Upper", (x, y) => x + y
        },
        {
            "RightUpper", (x, y) => x + y
        },
        {
            "LowerRight", (x, y) => x + y
        },
        {
            "RightLower", (x, y) => x + y
        },
        {
            "LowerLeft", (x, y) => x + y
        }
    };

}