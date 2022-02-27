namespace YMF_1;

public static class Utils
{
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
        for (var i = 0; i<grid.Nodes.Length; i++)
        {
            if (!grid.Nodes[i].IsFictive)
            {
                result.Add(rawResultVec[i]);
            }
        }

        return result.ToArray();
    }
}