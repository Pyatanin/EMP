namespace YMF_1;

public class Grid
{
    public class Node
    {
        public double X;
        public double Y;
        public bool IsFictive;
        public Node(double x, double y, bool isFictive)
        {
            X = x;
            Y = y;
            IsFictive = isFictive;
        }
    }
    public double Hx { get; set; }
    public double Hy { get; set; }
    public double[] X { get; set; }
    public double[] Y { get; set; }

    public readonly Node[] Nodes;

    private bool IsFictive(InputModel input, double x, double y)
    {
        // +-----+
        // |     |
        // |     |
        // |     |
        // |-----|------------+
        // |                  |
        // +-----+------------+
        if (input.OmegaX[0] > x || input.OmegaY[0] > y)
        {
            return true;
        }
        if (input.OmegaX[1] < x && input.OmegaY[1] < y)
        {
            return true;
        }
        if (input.OmegaX[2] < x || input.OmegaY[2] < y)
        {
            return true;
        }

        return false;
    }

    public Grid(InputModel input)
    {
        
        if (Math.Abs(input.DischargeRatioX - 1) > 1e-10){
            var sumKx = (1 - Math.Pow(input.DischargeRatioX, input.AmountPointsX - 1)) /
                                                         (1 - input.DischargeRatioX);
            Hx = (input.GridX[1] - input.GridX[0]) / sumKx;
            var x = new double[input.AmountPointsX];
            for (int i = 0; i < input.AmountPointsX; i++)
                x[i] = input.GridX[0] +
                       Hx * (1 - Math.Pow(input.DischargeRatioX, i)) / (1 - input.DischargeRatioX);
            X = x;
        }
        else
        {
            var x = new double[input.AmountPointsX+1];
            Hx = (input.GridX[1] - input.GridX[0]) / input.AmountPointsX;
            for (int i = 0; i <= input.AmountPointsX; i++)
                x[i] = input.GridX[0] + i*Hx;
            X = x;
        }

        if (Math.Abs(input.DischargeRatioY - 1) > 1e-10)
        {
            var sumKy = (1 - Math.Pow(input.DischargeRatioY, input.AmountPointsY - 1)) /
                          (1 - input.DischargeRatioY);
            Hy = (input.GridY[1] - input.GridY[0]) / sumKy;
            var y = new double[input.AmountPointsX];
            for (int i = 0; i < input.AmountPointsY; i++)
                y[i] = input.GridY[0] +
                       Hy * (1 - Math.Pow(input.DischargeRatioY, i)) / (1 - input.DischargeRatioY);
            Y = y;
        }
        else
        {
            var y = new double[input.AmountPointsX+1];
            Hy = (input.GridY[1] - input.GridY[0]) / input.AmountPointsY;
            for (int i = 0; i <= input.AmountPointsY; i++)
                y[i] = input.GridY[0] + i*Hy;
            Y = y;
        }

        var nodes = new Node[(X.Length)*(Y.Length)];
        var num = 0;
        foreach (var i in Y)
        {
            foreach (var j in X)
            {
                nodes[num] = new Node(j, i, IsFictive(input, j, i));
                num++;
            }
        }
        Nodes = nodes;
    }
}

