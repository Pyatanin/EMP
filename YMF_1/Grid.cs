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

    private bool IsFictiveCheck(InputJsonModel inputJson, double x, double y)
    {
        // +-----+
        // |     |
        // |     |
        // |     |
        // |-----|------------+
        // |                  |
        // +-----+------------+
        if (inputJson.OmegaX[0] > x || inputJson.OmegaY[0] > y)
        {
            return true;
        }
        if (inputJson.OmegaX[1] < x && inputJson.OmegaY[1] < y)
        {
            return true;
        }
        if (inputJson.OmegaX[2] < x || inputJson.OmegaY[2] < y)
        {
            return true;
        }

        return false;
    }
    
    public bool IsOnBorderCheck(InputJsonModel inputJson, double x, double y)
    {
        if (inputJson.OmegaX[0] == x || inputJson.OmegaY[0] == y || inputJson.OmegaX[2] == x || inputJson.OmegaY[2] == y) 
        {
            return true;
        }

        if (inputJson.OmegaX[1] == x && inputJson.OmegaY[1] < y || inputJson.OmegaX[1] < x && inputJson.OmegaY[1] == y) 
        {
            return true;
        }

        return false;
    }
    
    public string GetBorderType(InputJsonModel inputJson, double x, double y)
    {
        if (inputJson.OmegaX[0] == x) return "Left";

        if (inputJson.OmegaY[2] == y) return "UpperLeft";

        if (inputJson.OmegaX[1] == x && y >= inputJson.OmegaY[1]) return "RightUpper";

        if (inputJson.OmegaY[1] == y && x >= inputJson.OmegaY[1]) return "UpperRight";

        if (inputJson.OmegaX[2] == x) return "RightLower";

        if (inputJson.OmegaY[0] == y) return "Lower";

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
    public Grid(InputJsonModel inputJson)
    {
        L = inputJson.Lambda;
        G = inputJson.Gamma;
        
        if (Math.Abs(inputJson.DischargeRatioX - 1) > 1e-10)
        {
            var sumKx = (1 - Math.Pow(inputJson.DischargeRatioX, inputJson.AmountPointsX - 1)) / (1 - inputJson.DischargeRatioX);
            Hx = (inputJson.OmegaX[2] - inputJson.OmegaX[0]) / sumKx;
            var x = new double[inputJson.AmountPointsX];
            for (var i = 0; i < inputJson.AmountPointsX; i++)
            {
                x[i] = inputJson.OmegaX[0] + Hx * (1 - Math.Pow(inputJson.DischargeRatioX, i)) / (1 - inputJson.DischargeRatioX);
            }
            X = x;
        }
        else
        {
            var x = new double[inputJson.AmountPointsX+1];
            Hx = (inputJson.OmegaX[2] - inputJson.OmegaX[0]) / inputJson.AmountPointsX;
            for (var i = 0; i <= inputJson.AmountPointsX; i++)
            {
                x[i] = inputJson.OmegaX[0] + i * Hx;
            }
            X = x;
        }

        if (Math.Abs(inputJson.DischargeRatioY - 1) > 1e-10) 
        {
            var sumKy = (1 - Math.Pow(inputJson.DischargeRatioY, inputJson.AmountPointsY - 1)) /
                          (1 - inputJson.DischargeRatioY);
            Hy = (inputJson.OmegaY[2] - inputJson.OmegaY[0]) / sumKy;
            var y = new double[inputJson.AmountPointsY];
            for (var i = 0; i < inputJson.AmountPointsY; i++)
            {
                y[i] = inputJson.OmegaY[0] + Hy * (1 - Math.Pow(inputJson.DischargeRatioY, i)) / (1 - inputJson.DischargeRatioY);
            }
            Y = y;
        }
        else
        {
            var y = new double[inputJson.AmountPointsY+1];
            Hy = (inputJson.OmegaY[2] - inputJson.OmegaY[0]) / inputJson.AmountPointsY;
            for (var i = 0; i <= inputJson.AmountPointsY; i++)
            {
                y[i] = inputJson.OmegaY[0] + i * Hy;
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
                    IsFictiveCheck(inputJson, j, i), 
                    IsOnBorderCheck(inputJson, j, i), 
                    GetBorderType(inputJson, j, i)
                    );
                num++;
            }
        }
        Nodes = nodes;
    }
}

