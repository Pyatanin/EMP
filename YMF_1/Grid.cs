namespace YMF_1;

public class Grid
{
    public double L;
    public double G;
    public class Node
    {
        public double X;
        public double Y;
        public bool IsFictive;
        public bool IsOnBorder;
        public string BorderType;

        public Node(double x, double y, bool isFictive, bool isOnBorder, string borderType)
        {
            X = x;
            Y = y;
            IsFictive = isFictive;
            IsOnBorder = isOnBorder;
            BorderType = borderType;
        }
    }
    public double Hx { get; set; }
    public double Hy { get; set; }
    public double[] X { get; set; }
    public double[] Y { get; set; }

    public readonly Node[] Nodes;

    private bool IsFictiveCheck(InputModel input, double x, double y)
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
    
    public bool IsOnBorderCheck(InputModel input, double x, double y)
    {
        if (input.OmegaX[0] == x || input.OmegaY[0] == y || input.OmegaX[2] == x || input.OmegaY[2] == y) 
        {
            return true;
        }

        if (input.OmegaX[1] == x && input.OmegaY[1] < y || input.OmegaX[1] < x && input.OmegaY[1] == y) 
        {
            return true;
        }

        return false;
    }
    
    public string GetBorderType(InputModel input, double x, double y)
    {
        if (input.OmegaX[0] == x) return "Left";

        if (input.OmegaY[2] == y) return "UpperLeft";

        if (input.OmegaX[1] == x && y >= input.OmegaY[1]) return "RightUpper";

        if (input.OmegaY[1] == y && x >= input.OmegaY[1]) return "UpperRight";

        if (input.OmegaX[2] == x) return "RightLower";

        if (input.OmegaY[0] == y) return "Lower";

        return "None";
    }

    //    GRID TYPE
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
    public Grid(InputModel input)
    {
        L = input.Lambda;
        G = input.Gamma;
        
        if (Math.Abs(input.DischargeRatioX - 1) > 1e-10)
        {
            var sumKx = (1 - Math.Pow(input.DischargeRatioX, input.AmountPointsX - 1)) / (1 - input.DischargeRatioX);
            Hx = (input.OmegaX[2] - input.OmegaX[0]) / sumKx;
            var x = new double[input.AmountPointsX];
            for (var i = 0; i < input.AmountPointsX; i++)
            {
                x[i] = input.OmegaX[0] + Hx * (1 - Math.Pow(input.DischargeRatioX, i)) / (1 - input.DischargeRatioX);
            }
            X = x;
        }
        else
        {
            var x = new double[input.AmountPointsX+1];
            Hx = (input.OmegaX[2] - input.OmegaX[0]) / input.AmountPointsX;
            for (var i = 0; i <= input.AmountPointsX; i++)
            {
                x[i] = input.OmegaX[0] + i * Hx;
            }
            X = x;
        }

        if (Math.Abs(input.DischargeRatioY - 1) > 1e-10) 
        {
            var sumKy = (1 - Math.Pow(input.DischargeRatioY, input.AmountPointsY - 1)) /
                          (1 - input.DischargeRatioY);
            Hy = (input.OmegaY[2] - input.OmegaY[0]) / sumKy;
            var y = new double[input.AmountPointsY];
            for (var i = 0; i < input.AmountPointsY; i++)
            {
                y[i] = input.OmegaY[0] + Hy * (1 - Math.Pow(input.DischargeRatioY, i)) / (1 - input.DischargeRatioY);
            }
            Y = y;
        }
        else
        {
            var y = new double[input.AmountPointsY+1];
            Hy = (input.OmegaY[2] - input.OmegaY[0]) / input.AmountPointsY;
            for (var i = 0; i <= input.AmountPointsY; i++)
            {
                y[i] = input.OmegaY[0] + i * Hy;
            }
            Y = y;
        }

        var nodes = new Node[(X.Length)*(Y.Length)];
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

