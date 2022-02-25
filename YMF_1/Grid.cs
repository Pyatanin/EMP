namespace YMF_1;

public class Grid
{
    public class Node
    {
        public double X { get; private set; }
        public double Y { get; private set; }

        public bool IsFictive { get; private set; }

        public bool IsOnBorder { get; private set; }
        public string BorderType { get; private set; }

        public Node(double x, double y, bool isFictive, bool isOnBorder, string borderType)
        {
            X = x;
            Y = y;
            IsFictive = isFictive;
            IsOnBorder = isOnBorder;
            BorderType = borderType;
        }
    }

    public double[] X { get; private set; }
    public double[] Y { get; private set; }

    public readonly Node[] Nodes;

    private bool IsFictiveCheck(InputModel input, double x, double y)
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

        if (x > input.OmegaX[1] && y < input.OmegaY[1])
        {
            return true;
        }

        return false;
    }

    private bool IsOnBorderCheck(InputModel input, double x, double y)
    {
        if (x == input.OmegaX[0] || x == input.OmegaX[2] || y == input.OmegaY[0] || y == input.OmegaY[2] ||
            y == input.OmegaY[1] && x >= input.OmegaX[1] || x == input.OmegaX[1] && y <= input.OmegaY[1])
        {
            return true;
        }

        return false;
    }

    private string GetBorderType(InputModel input, double x, double y)
    {
        if (x == input.OmegaX[0]) return "Left";

        if (y == input.OmegaY[2]) return "Upper";

        if (x == input.OmegaX[2] && y >= input.OmegaY[1]) return "RightUpper";

        if (y == input.OmegaY[1] && x >= input.OmegaX[1]) return "LowerRight";

        if (x == input.OmegaX[1] && y <= input.OmegaY[1]) return "RightLower";

        if (y == input.OmegaY[0] && x <= input.OmegaX[1]) return "LowerLeft";

        return "None";
    }

    public Grid(InputModel input)
    {
        if (Math.Abs(input.DischargeRatioX - 1) > 1e-10)
        {
            var sumKx = (1 - Math.Pow(input.DischargeRatioX, input.AmountPointsX - 1)) / (1 - input.DischargeRatioX);
            var hX = (input.OmegaX[2] - input.OmegaX[0]) / sumKx;
            var x = new double[input.AmountPointsX];
            for (var i = 0; i < input.AmountPointsX; i++)
            {
                x[i] = input.OmegaX[0] + hX * (1 - Math.Pow(input.DischargeRatioX, i)) / (1 - input.DischargeRatioX);
            }

            X = x;
        }
        else
        {
            var x = new double[input.AmountPointsX];
            var hX = (input.OmegaX[2] - input.OmegaX[0]) / (input.AmountPointsX - 1);
            for (var i = 0; i < input.AmountPointsX; i++)
            {
                x[i] = input.OmegaX[0] + i * hX;
            }

            X = x;
        }

        if (Math.Abs(input.DischargeRatioY - 1) > 1e-10)
        {
            var sumKy = (1 - Math.Pow(input.DischargeRatioY, input.AmountPointsY - 1)) / (1 - input.DischargeRatioY);
            var hY = (input.OmegaY[2] - input.OmegaY[0]) / sumKy;
            var y = new double[input.AmountPointsY];
            for (var i = 0; i < input.AmountPointsY; i++)
            {
                y[i] = input.OmegaY[0] + hY * (1 - Math.Pow(input.DischargeRatioY, i)) / (1 - input.DischargeRatioY);
            }

            Y = y;
        }
        else
        {
            var y = new double[input.AmountPointsY];
            var hY = (input.OmegaY[2] - input.OmegaY[0]) / (input.AmountPointsY - 1);
            for (var i = 0; i < input.AmountPointsY; i++)
            {
                y[i] = input.OmegaY[0] + i * hY;
            }

            Y = y;
        }

        if (!Utils.CheckGridConsistency(X, input.OmegaX) || !Utils.CheckGridConsistency(Y, input.OmegaY))
        {
            throw new Exception("Non consistent data");
        }

        var nodes = new Node[X.Length * Y.Length];
        var num = 0;
        foreach (var i in Y)
        {
            foreach (var j in X)
            {
                nodes[num] = new Node(
                    j,
                    i,
                    IsFictiveCheck(input, j, i),
                    IsOnBorderCheck(input, j, i),
                    GetBorderType(input, j, i)
                );
                num++;
            }
        }

        Nodes = nodes;
    }
}