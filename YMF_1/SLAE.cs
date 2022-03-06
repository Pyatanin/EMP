namespace YMF_1;

public class Slae
{
    public void Solve(InputModel input)
    {
        Result = Methods.IterateGs(Result, Matrix, 1.7, RightSideVector);
        var residual = Methods.RelativeResidual(Matrix, Result, RightSideVector);
        var iter = 1;
        var resultPred = new double[Result.Length];
        while (iter <= input.MaxIter && input.Eps < residual &&
               !Methods.CheckIsStagnate(resultPred, Result, input.Delta))
        {
            Result.AsSpan().CopyTo(resultPred);
            Result = Methods.IterateGs(Result, Matrix, 1.7, RightSideVector);
            residual = Methods.RelativeResidual(Matrix, Result, RightSideVector);
            iter++;
        }
    }

    public Slae(InputModel input, Grid grid, Dictionary<string, string> boundaryConditions)
    {
        var shift = grid.X.Length;
        
        var diag = new double[grid.Nodes.Length];

        var l0 = new double[grid.Nodes.Length - 1];
        var l1 = new double[grid.Nodes.Length - shift];

        var u0 = new double[grid.Nodes.Length - 1];
        var u1 = new double[grid.Nodes.Length - shift];

        var result = new double[grid.Nodes.Length];
        var rightSideVector = new double[grid.Nodes.Length];
        for (var i = 0; i < grid.Nodes.Length; i++)
        {
            if (!grid.Nodes[i].IsFictive)
            {
                if (grid.Nodes[i].IsOnBorder)
                {
                    if (boundaryConditions[grid.Nodes[i].BorderType] == "First")
                    {
                        diag[i] = 1.0;
                        rightSideVector[i] =
                            BoundaryFunc.First[grid.Nodes[i].BorderType](grid.Nodes[i].X, grid.Nodes[i].Y);
                    }
                    else
                    {
                        if (grid.Nodes[i].BorderType == "Left")
                        {
                            var hX = grid.Nodes[i + 1].X - grid.Nodes[i].X;
                            diag[i] = input.Lambda / hX;
                            u0[i] = -input.Lambda / hX;
                            rightSideVector[i] =
                                BoundaryFunc.Second[grid.Nodes[i].BorderType](grid.Nodes[i].X, grid.Nodes[i].Y);
                        }

                        if (grid.Nodes[i].BorderType == "Upper")
                        {
                            var hY = grid.Nodes[i].Y - grid.Nodes[i - shift].Y;
                            diag[i] = input.Lambda / hY;
                            l1[i - shift] = -input.Lambda / hY;
                            rightSideVector[i] =
                                BoundaryFunc.Second[grid.Nodes[i].BorderType](grid.Nodes[i].X, grid.Nodes[i].Y);
                        }

                        if (grid.Nodes[i].BorderType == "RightUpper" || grid.Nodes[i].BorderType == "RightLower")
                        {
                            var hX = grid.Nodes[i].X - grid.Nodes[i - 1].X;
                            diag[i] = input.Lambda / hX;
                            l0[i - 1] = -input.Lambda / hX;
                            rightSideVector[i] =
                                BoundaryFunc.Second[grid.Nodes[i].BorderType](grid.Nodes[i].X, grid.Nodes[i].Y);
                        }

                        if (grid.Nodes[i].BorderType == "LowerRight" || grid.Nodes[i].BorderType == "LowerLeft")
                        {
                            var hY = grid.Nodes[i + shift].Y - grid.Nodes[i].Y;
                            diag[i] = input.Lambda / hY;
                            u1[i] = -input.Lambda / hY;
                            rightSideVector[i] =
                                BoundaryFunc.Second[grid.Nodes[i].BorderType](grid.Nodes[i].X, grid.Nodes[i].Y);
                        }
                    }
                }
                else
                {
                    var hXLeft = grid.Nodes[i].X - grid.Nodes[i - 1].X;
                    var hXRight = grid.Nodes[i + 1].X - grid.Nodes[i].X;

                    var hYLower = grid.Nodes[i].Y - grid.Nodes[i - shift].Y;
                    var hYUpper = grid.Nodes[i + shift].Y - grid.Nodes[i].Y;

                    diag[i] = -input.Lambda* 2.0 * (1.0 / (hXLeft * hXRight) + 1.0 / (hYLower * hYUpper)) + input.Gamma;

                    u0[i] = input.Lambda * 2.0 / (hXRight * (hXRight + hXLeft));
                    l0[i - 1] = input.Lambda * 2.0 / (hXLeft * (hXRight + hXLeft));

                    l1[i - shift] = input.Lambda * 2.0 / (hYUpper * (hYUpper + hYLower));
                    u1[i] = input.Lambda * 2.0 / (hYLower * (hYUpper + hYLower));

                    rightSideVector[i] = RightSideFunc.Eval(grid.Nodes[i].X, grid.Nodes[i].Y);
                }
            }
            else
            {
                diag[i] = 1.0;
                rightSideVector[i] = 0;
            }
        }

        Matrix = new Matrix(diag, l0, l1, u0, u1, shift);
        RightSideVector = rightSideVector;
        Result = result;
    }

    public double[] RightSideVector;
    public Matrix Matrix;
    public double[] Result;
}