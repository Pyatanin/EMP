using Microsoft.Data.Analysis;

namespace YMF_1;

public static class Utils
{
    private static readonly Func<double, double, double> Eval = (x, y) => Math.Pow(x, 4) + Math.Pow(y, 4);

    public static bool CheckGridConsistency(double[] grid, double[] anchor)
    {
        foreach (var point in anchor)
        {
            if (Array.FindIndex(grid, x => Math.Abs(x - point) < 1e-10) < 0)
            {
                return false;
            }
        }

        return true;
    }

    public static double[] ExcludeFictive(double[] rawResultVec, Grid grid)
    {
        var result = new List<double>();
        for (var i = 0; i < grid.Nodes.Length; i++)
        {
            if (!grid.Nodes[i].IsFictive)
            {
                result.Add(rawResultVec[i]);
            }
        }

        return result.ToArray();
    }

    public static void WriteTable(double[] resultVec, Grid grid)
    {
        var xBuf = new List<double>();
        var yBuf = new List<double>();
        var uBuf = new List<double>();
        var uStarBuf = new List<double>();
        var absTolBuf = new List<double>();

        for (var i = 0; i < grid.Nodes.Length; i++)
        {
            if (!grid.Nodes[i].IsFictive)
            {
                xBuf.Add(grid.Nodes[i].X);
                yBuf.Add(grid.Nodes[i].Y);
                var uStar = Eval(grid.Nodes[i].X, grid.Nodes[i].Y);
                uBuf.Add(resultVec[i]);
                uStarBuf.Add(uStar);
                absTolBuf.Add(Math.Abs(resultVec[i] - uStar));
            }
        }

        var xCol = new PrimitiveDataFrameColumn<double>("x", xBuf);
        var yCol = new PrimitiveDataFrameColumn<double>("y", yBuf);
        var uCol = new PrimitiveDataFrameColumn<double>("u", uBuf);
        var uStarCol = new PrimitiveDataFrameColumn<double>("u*", uStarBuf);
        var absTolCol = new PrimitiveDataFrameColumn<double>("|u*-u|", absTolBuf);

        var table = new DataFrame(xCol, yCol, uCol, uStarCol, absTolCol);
        Console.WriteLine(table);
        DataFrame.WriteCsv(table, "res.csv", separator: ' ');
    }
}