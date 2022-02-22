namespace YMF_1;

public class Matrix
{
    public readonly double[] Diag;

    public readonly double[][] LowPart = new double[2][];

    public readonly int M;
    public readonly int N;

    public readonly double[][] UpperPart = new double[2][];

    public Matrix()
    {
        M = 0;
        N = 0;
    }

    public Matrix(Grid grid)
    {
        var diag = new double[grid.Nodes.Length];
        var l0 = new double[grid.Nodes.Length-1];
        var l1 = new double[grid.Nodes.Length-grid.X.Length];
        var u0 = new double[grid.Nodes.Length-1];
        var u1 = new double[grid.Nodes.Length-grid.X.Length];

        foreach (var node in grid.Nodes)
        {
            
        }

        Diag = diag;
        LowPart[1] = l1;
        LowPart[0] = l0;
        UpperPart[1] = u1;
        UpperPart[0] = u0;
    }


}