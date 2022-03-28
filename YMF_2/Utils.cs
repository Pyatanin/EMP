using Microsoft.Data.Analysis;
using YMF_2.JsonModels;

namespace YMF_2;

/// <summary>
/// Implements compilation of a function from a string, creating a lambda expression
/// </summary>
public static class Utils
{
    public static double EvalFunc(string inputFuncString, double arg)
    {
        var calc = new Sprache.Calc.XtensibleCalculator();
        var toEval = calc.ParseFunction(inputFuncString).Compile();
        return toEval(MakeDict1D(arg));
    }

    public static Dictionary<string, double> MakeDict1D(double x) => new() {{"x", x}};
    
    public static Dictionary<string, double> MakeDict2D(double x, double u) => new() {{"x", x}, {"u",u}};
    
    public static void WriteTable(double[] resultVec, Fem.Grid grid, InputFuncs inputFuncs)
    {
        var xBuf = new List<double>();
        var uBuf = new List<double>();
        var uStarBuf = new List<double>();
        var absTolBuf = new List<double>();
        var calc = new Sprache.Calc.XtensibleCalculator();
        var toEvalUStar = calc.ParseFunction(inputFuncs.UStar).Compile();
        for (var i = 0; i < grid.X.Length; i++)
        {
            xBuf.Add(grid.X[i]);
            uBuf.Add(resultVec[i]);
            uStarBuf.Add(toEvalUStar(MakeDict1D(grid.X[i])));
            absTolBuf.Add(Math.Abs(toEvalUStar(MakeDict1D(grid.X[i])) - resultVec[i]));
        }

        var xCol = new PrimitiveDataFrameColumn<double>("x", xBuf);
        var uCol = new PrimitiveDataFrameColumn<double>("u", uBuf);
        var uStarCol = new PrimitiveDataFrameColumn<double>("u*", uStarBuf);
        var absTolCol = new PrimitiveDataFrameColumn<double>("|u*-u|", absTolBuf);

        var table = new DataFrame(xCol, uCol, uStarCol, absTolCol);
        Console.WriteLine(table);
        DataFrame.WriteCsv(table, "res.csv", separator: ' ');
    }
}