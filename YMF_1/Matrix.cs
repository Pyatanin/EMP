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
        var l1 = new double[grid.Nodes.Length-grid.X.Length+2];
        var u0 = new double[grid.Nodes.Length-1];
        var u1 = new double[grid.Nodes.Length-grid.X.Length+2];

        for (var i = 0; i < grid.Nodes.Length; i++)
        {
            if (!grid.Nodes[i].IsFictive)
            {
                if (grid.Nodes[i].IsOnBorder)
                {

                }
                else
                {
                    var shift = grid.X.Length;
                    
                    var hXLeft = grid.Nodes[i].X - grid.Nodes[i - 1].X;
                    var hXRight = grid.Nodes[i + 1].X - grid.Nodes[i].X;
                    
                    var hYLower = grid.Nodes[i].Y - grid.Nodes[i - shift].Y;
                    var hYUpper = grid.Nodes[i + shift].Y - grid.Nodes[i].Y;

                    diag[i] = -grid.L * (2 / (hXLeft * hXRight) + 2 / (hYLower * hYUpper)) + grid.G;

                    u0[i] = grid.L * 2 / (hXRight * (hXRight + hXLeft));
                    l0[i - 1] = grid.L * 2 / (hXLeft * (hXRight + hXLeft));

                    l1[i - shift] = grid.L * 2 / (hYUpper * (hYUpper + hYLower));
                    u1[i] = grid.L * 2 / (hYLower * (hYUpper + hYLower));
                }
            }
            
            
        }

        Diag = diag;
        LowPart[1] = l1;
        LowPart[0] = l0;
        UpperPart[1] = u1;
        UpperPart[0] = u0;
    }


}