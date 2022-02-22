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
    public double h_x { get; set; }
    public double h_y { get; set; }
    public double[] X { get; set; }
    public double[] Y { get; set; }

    public Node[] Nodes;

    private bool IsFictive(InputModel input, double x, double y)
    {
        // +-----+
        // |     |
        // |     |
        // |     |
        // |-----|------------+
        // |                  |
        // +-----+------------+
        if (input.Omega_X[0] > x || input.Omega_Y[0] > y)
        {
            return true;
        }
        if (input.Omega_X[1] < x && input.Omega_Y[1] < y)
        {
            return true;
        }
        if (input.Omega_X[2] < x || input.Omega_Y[2] < y)
        {
            return true;
        }

        return false;
    }

    public Grid(InputModel input)
    {
        var x = new double[input.Amount_points_X];
        var y = new double[input.Amount_points_Y];
        if (input.Discharge_ratio_X != 1){
            var sum_k_x = (1 - Math.Pow(input.Discharge_ratio_X, input.Amount_points_X - 1)) /
                                                         (1 - input.Discharge_ratio_X);
            h_x = (input.Grid_X[1] - input.Grid_X[0]) / sum_k_x;

            for (int i = 0; i < input.Amount_points_X; i++)
                x[i] = input.Grid_X[0] +
                       h_x * (1 - Math.Pow(input.Discharge_ratio_X, i)) / (1 - input.Discharge_ratio_X);
        }
        else
        {
            h_x = (input.Grid_X[1] - input.Grid_X[0]) / input.Amount_points_X;
            for (int i = 0; i < input.Amount_points_X; i++)
                x[i] = input.Grid_X[0] + i*h_x;
        }

        if (input.Discharge_ratio_Y != 1)
        {
            var sum_k_y = (1 - Math.Pow(input.Discharge_ratio_Y, input.Amount_points_Y - 1)) /
                          (1 - input.Discharge_ratio_Y);
            h_y = (input.Grid_Y[1] - input.Grid_Y[0]) / sum_k_y;
            
            for (int i = 0; i < input.Amount_points_Y; i++)
                y[i] = input.Grid_Y[0] +
                       h_y * (1 - Math.Pow(input.Discharge_ratio_Y, i)) / (1 - input.Discharge_ratio_Y);
        }
        else
        {
            h_y = (input.Grid_Y[1] - input.Grid_Y[0]) / input.Amount_points_Y;
            for (int i = 0; i < input.Amount_points_Y; i++)
                y[i] = input.Grid_Y[0] + i*h_y;
        }

        X = x;
        Y = y;

        var nodes = new Node[(X.Length)*(Y.Length)];
        var num = 0;
        for (int i = 0; i < Y.Length; i++)
        {
            for (int j = 0; j < X.Length; j++){
                nodes[num] = new Node(X[j], Y[i], IsFictive(input, X[j], Y[i]));
                num++;
            }
        }
        Nodes = nodes;
    }
}

