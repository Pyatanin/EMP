namespace YMF_1;

public class Utils
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
}