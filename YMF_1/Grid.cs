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
        var x = new List<double>();
        x.Add(input.OmegaX[0]);
        var y = new List<double>();
        y.Add(input.OmegaY[0]);
        for (int block = 0; block < 2; block++)
        {

            if (Math.Abs(input.DischargeRatioX[block] - 1) > 1e-10)
            {
                var sumKx = (1 - Math.Pow(input.DischargeRatioX[block], input.AmountPointsX[block] - 1)) /
                            (1 - input.DischargeRatioX[block]);
                var hX = (input.OmegaX[block+1] - input.OmegaX[block]) / sumKx;
                for (var i = 1; i < input.AmountPointsX[block]; i++)
                {
                    x.Add(input.OmegaX[block] +
                           hX * (1 - Math.Pow(input.DischargeRatioX[block], i)) / (1 - input.DischargeRatioX[block]));
                }
            }
            else
            {
                var hX = (input.OmegaX[block+1] - input.OmegaX[block]) / (input.AmountPointsX[block] - 1);
                for (var i = 1; i < input.AmountPointsX[block]; i++)
                {
                    x.Add(input.OmegaX[block] + i * hX);
                }
            }

            if (Math.Abs(input.DischargeRatioY[block] - 1) > 1e-10)
            {
                var sumKy = (1 - Math.Pow(input.DischargeRatioY[block], input.AmountPointsY[block] - 1)) /
                            (1 - input.DischargeRatioY[block]);
                var hY = (input.OmegaY[block+1] - input.OmegaY[block]) / sumKy;
                for (var i = 1; i < input.AmountPointsY[block]; i++)
                {
                    y.Add(input.OmegaY[block] +
                           hY * (1 - Math.Pow(input.DischargeRatioY[block], i)) / (1 - input.DischargeRatioY[block]));
                }

            }
            else
            {
                var hY = (input.OmegaY[block+1] - input.OmegaY[block]) / (input.AmountPointsY[block] - 1);
                for (var i = 1; i < input.AmountPointsY[block]; i++)
                {
                    y.Add(input.OmegaY[block] + i * hY);
                }

            }

            X = x.ToArray();
            Y = y.ToArray();
        }

        // if (!Utils.CheckGridConsistency(X, input.OmegaX) || !Utils.CheckGridConsistency(Y, input.OmegaY))
        // {
        //     throw new Exception("Non consistent data");
        // }

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