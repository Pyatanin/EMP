using YMF_2.JsonModels;
using YMF_2.LinAlg;

namespace YMF_2.Fem;

public static class Solver
{
    public static double[] SolveWithSimpleIteration(Grid grid, InputFuncs inputFuncs, Area area,
        BoundaryConditions boundaryConds, Accuracy accuracy)
    {
        var initApprox = new double[grid.X.Length];
        var slae = new Slae(grid, inputFuncs, initApprox);
        double relaxRatio;
        ApplyBoundaryConditions(slae.Matrix, slae.RhsVec, area, boundaryConds);
        slae.Solve(accuracy);
        slae.ResVec.AsSpan().CopyTo(initApprox);
        while (SlaeSolver.RelResidual(slae) > accuracy.Eps &&
               !SlaeSolver.CheckIsStagnate(slae.ResVec, initApprox, accuracy.Delta))
        {
            slae = new Slae(grid, inputFuncs, initApprox);
            ApplyBoundaryConditions(slae.Matrix, slae.RhsVec, area, boundaryConds);
            slae.Solve(accuracy);
            relaxRatio = GetRelaxRatio(slae.ResVec, grid, inputFuncs, initApprox, accuracy, area, boundaryConds);
            initApprox = UpdateApprox(slae.ResVec, initApprox, relaxRatio);
        }

        return slae.ResVec;
    }

    private static void ApplyBoundaryConditions(
        Matrix m,
        double[] rhs,
        Area area,
        BoundaryConditions boundaryConds
    )
    {
        switch (boundaryConds.Left)
        {
            case "First":
                m.Center[0] = 1.0;
                m.Upper[0] = 0.0;
                m.Lower[0] = 0.0;
                rhs[0] = Utils.EvalFunc(boundaryConds.LeftFunc, area.LeftBorder);
                break;
            case "Second":
                rhs[0] += Utils.EvalFunc(boundaryConds.LeftFunc, area.LeftBorder);
                break;
            case "Third":
                m.Center[0] += boundaryConds.Beta;
                rhs[0] += boundaryConds.Beta * Utils.EvalFunc(boundaryConds.LeftFunc, area.LeftBorder);
                break;
        }

        switch (boundaryConds.Right)
        {
            case "First":
                m.Center[^1] = 1.0;
                m.Upper[^1] = 0.0;
                m.Lower[^1] = 0.0;
                rhs[^1] = Utils.EvalFunc(boundaryConds.RightFunc, area.RightBorder);
                break;
            case "Second":
                rhs[^1] += Utils.EvalFunc(boundaryConds.RightFunc, area.RightBorder);
                break;
            case "Third":
                m.Center[^1] += boundaryConds.Beta;
                rhs[^1] += boundaryConds.Beta * Utils.EvalFunc(boundaryConds.RightFunc, area.RightBorder);
                break;
        }
    }

    private static double GetRelaxRatio(double[] resVec, Grid grid, InputFuncs inputFuncs, double[] prevRes,
        Accuracy accuracy, Area area, BoundaryConditions boundaryConds)
    {
        double gold = (Math.Pow(5, 0.5) - 1.0) / 2.0;
        double xLeft, xRight, funcLeft, funcRight, left, right;
        left = 0.0;
        right = 1.0;
        xLeft = (1 - gold);
        xRight = gold;
        funcLeft = GetResidualFunc(resVec, grid, inputFuncs, xLeft, prevRes, area, boundaryConds);
        funcRight = GetResidualFunc(resVec, grid, inputFuncs, xRight, prevRes, area, boundaryConds);
        while (Math.Abs(right - left) > accuracy.Eps)
        {
            if (funcLeft > funcRight)
            {
                left = xLeft;
                xLeft = xRight;
                funcLeft = funcRight;
                xRight = left + gold * (right - left);
                funcRight = GetResidualFunc(resVec, grid, inputFuncs, xRight, prevRes, area, boundaryConds);
            }
            else
            {
                right = xRight;
                xRight = xLeft;
                funcRight = funcLeft;
                xLeft = left + (1.0 - gold) * (right - left);
                funcLeft = GetResidualFunc(resVec, grid, inputFuncs, xLeft, prevRes, area, boundaryConds);
            }
        }

        return (left + right) / 2.0;
    }

    private static double GetResidualFunc(double[] resVec, Grid grid, InputFuncs inputFuncs, double x, double[] prevRes,
        Area area, BoundaryConditions boundaryConds)
    {
        var approx = new double[prevRes.Length];
        for (int i = 0; i < prevRes.Length; i++)
        {
            approx[i] = x * resVec[i] + (1.0 - x) * prevRes[i];
        }

        var slae = new Slae(grid, inputFuncs, approx);
        ApplyBoundaryConditions(slae.Matrix, slae.RhsVec, area, boundaryConds);
        return SlaeSolver.Residual(slae.Matrix, approx, slae.RhsVec);
    }

    private static double[] UpdateApprox(double[] resVec, double[] approx, double relaxRatio)
    {
        var newApprox = new double[resVec.Length];
        for (int i = 0; i < resVec.Length; i++)
        {
            newApprox[i] = relaxRatio * resVec[i] + (1.0 - relaxRatio) * approx[i];
        }

        return newApprox;
    }
    
    public static double[] SolveWithNewton(Slae slae)
    {
        throw new NotImplementedException();
    }
}