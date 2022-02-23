namespace YMF_1;

public static class BoundaryFunc
{
    //    UpperLeft
    //    +----+
    //    |    |
    //    |    |
    // Left    |
    //    |   RightUpper
    //    |    |
    //    |    |
    //    |    +-UpperRight
    //    |            +
    //    |           RightLower
    //    |            |
    //    +------------+
    //      Lower

    #region FirstBoundaryFunctions
    
    // Insert testing number
    public static readonly Func<double, double, double>[] FirstLeft =
    {
        (x, y) => 1.0,
    };

    // Insert testing number
    public static readonly Func<double, double, double>[] FirstRightLower =
    {
        (x, y) => 2.0,
    };
    
    // Insert testing number
    public static readonly Func<double, double, double>[] FirstUpperRight =
    {
        (x, y) => 2.0,
    };
    
    // Insert testing number
    public static readonly Func<double, double, double>[] FirstRightUpper =
    {
        (x, y) => 2.0,
    };
    
    // Insert testing number
    public static readonly Func<double, double, double>[] FirstUpperLeft =
    {
        (x, y) => 2.0,
    };
    
    // Insert testing number
    public static readonly Func<double, double, double>[] FirstLower =
    {
        (x, y) => 2.0,
    };
    
    #endregion

    #region SecondBoundaryFunctions
    
    // Insert testing number
    public static readonly Func<double, double, double>[] SecondLeft =
    {
        (x, y) => 1.0,
    };

    // Insert testing number
    public static readonly Func<double, double, double>[] SecondRightLower =
    {
        (x, y) => 2.0,
    };
    
    // Insert testing number
    public static readonly Func<double, double, double>[] SecondUpperRight =
    {
        (x, y) => 2.0,
    };
    
    // Insert testing number
    public static readonly Func<double, double, double>[] SecondRightUpper =
    {
        (x, y) => 2.0,
    };
    
    // Insert testing number
    public static readonly Func<double, double, double>[] SecondUpperLeft =
    {
        (x, y) => 2.0,
    };
    
    // Insert testing number
    public static readonly Func<double, double, double>[] SecondLower =
    {
        (x, y) => 2.0,
    };
    
    #endregion
}