using System.Diagnostics;
using YMF_2.JsonModels;
using YMF_2.LinAlg;

namespace YMF_2.Fem;

/// <summary>
/// The class implements the FEM functionality including boundary conditions,
/// the SLAE solution method (simple iteration method)
/// and the use of the relaxation coefficient to improve the method
/// </summary>
public static class Solver
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="grid">One-dimensional mesh with nodes to create SLAE</param>
    /// <param name="inputFuncs">JSON containing the statement of the boundary elliptic problem</param>
    /// <param name="area">JSON containing information about grid parameters</param>
    /// <param name="boundaryConds">JSON containing information about edge conditions</param>
    /// <param name="accuracy">JSON containing information about edge conditions</param>
    /// <returns></returns>
    public static (double[], ResultStats) SolveWithSimpleIteration(Grid grid, InputFuncs inputFuncs, Area area,
        BoundaryConditions boundaryConds, Accuracy accuracy)
    {
        var initApprox = new double[grid.X.Length];
        var relaxRatio = accuracy.RelaxRatio;
        var iter = 0;
        Slae slae;
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        do
        {
            slae = new Slae(grid, inputFuncs, initApprox, false);
            ApplyBoundaryConditions(slae.Matrix!, slae.RhsVec!, area, boundaryConds);
            slae.Solve(accuracy);
            if (accuracy.AutoRelax)
            {
                relaxRatio = GetRelaxRatio(slae.ResVec!, grid, inputFuncs, initApprox, accuracy, area, boundaryConds, false);
            }

            initApprox = UpdateApprox(slae.ResVec!, initApprox, relaxRatio);
            iter++;
        } while (SlaeSolver.RelResidual(slae) > accuracy.Eps &&
                 !SlaeSolver.CheckIsStagnate(slae.ResVec!, initApprox, accuracy.Delta) && iter < accuracy.MaxIter);
        stopWatch.Stop();
        var resultStats = new ResultStats
        {
            IterationsCount = iter,
            RelResidual = SlaeSolver.RelResidual(slae),
            RelTolerance = SlaeSolver.RelTolerance(slae, inputFuncs, grid),
            ElapsedTime = stopWatch.ElapsedMilliseconds
        };
        return (slae.ResVec!, resultStats);
    }

    /// <summary>
    /// Method for taking into account boundary conditions for a one-dimensional problem
    /// </summary>
    /// <param name="m">Matrix instance</param>
    /// <param name="rhs">Instance of the right side of the SLAE</param>
    /// <param name="area">JSON containing information about grid parameters</param>
    /// <param name="boundaryConds">JSON containing information about edge conditions</param>
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

    /// <summary>
    /// search for the minimum of the functional by the golden section method
    /// </summary>
    /// <param name="resVec">solution vector</param>
    /// <param name="grid">One-dimensional mesh with nodes to create SLAE</param>
    /// <param name="inputFuncs">JSON containing the statement of the boundary elliptic problem</param>
    /// <param name="prevRes">vector K-1 stagnation tracking solutions</param>
    /// <param name="accuracy">JSON containing information about edge conditions</param>
    /// <param name="area">JSON containing information about grid parameters</param>
    /// <param name="boundaryConds">JSON containing information about edge conditions</param>
    /// <returns></returns>
    private static double GetRelaxRatio(double[] resVec, Grid grid, InputFuncs inputFuncs, double[] prevRes,
        Accuracy accuracy, Area area, BoundaryConditions boundaryConds, bool withNewton)
    {
        var gold = (Math.Pow(5, 0.5) - 1.0) / 2.0;
        var left = 0.0;
        var right = 1.0;
        var xLeft = (1 - gold);
        var xRight = gold;
        var funcLeft = GetResidualFunc(resVec, grid, inputFuncs, xLeft, prevRes, area, boundaryConds, withNewton);
        var funcRight = GetResidualFunc(resVec, grid, inputFuncs, xRight, prevRes, area, boundaryConds, withNewton);
        while (Math.Abs(right - left) > accuracy.Eps)
        {
            if (funcLeft > funcRight)
            {
                left = xLeft;
                xLeft = xRight;
                funcLeft = funcRight;
                xRight = left + gold * (right - left);
                funcRight = GetResidualFunc(resVec, grid, inputFuncs, xRight, prevRes, area, boundaryConds, withNewton);
            }
            else
            {
                right = xRight;
                xRight = xLeft;
                funcRight = funcLeft;
                xLeft = left + (1.0 - gold) * (right - left);
                funcLeft = GetResidualFunc(resVec, grid, inputFuncs, xLeft, prevRes, area, boundaryConds, withNewton);
            }
        }

        return (left + right) / 2.0;
    }

    /// <summary>
    /// getting the value of the residual function at a point
    /// </summary>
    /// <param name="resVec">solution vector</param>
    /// <param name="x">K-1 relaxation coefficient</param>
    /// <param name="prevRes">solution vector vector</param>
    /// <returns></returns>
    private static double GetResidualFunc(double[] resVec, Grid grid, InputFuncs inputFuncs, double x, double[] prevRes,
        Area area, BoundaryConditions boundaryConds, bool withNewton)
    {
        var approx = new double[prevRes.Length];
        for (int i = 0; i < prevRes.Length; i++)
        {
            approx[i] = x * resVec[i] + (1.0 - x) * prevRes[i];
        }

        var slae = new Slae(grid, inputFuncs, approx, withNewton);
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

    public static (double[], ResultStats) SolveWithNewton(Grid grid, InputFuncs inputFuncs, Area area,
        BoundaryConditions boundaryConds, Accuracy accuracy)
    {
        var initApprox = new double[grid.X.Length];
        var relaxRatio = accuracy.RelaxRatio;
        var iter = 0;
        Slae slae;
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        do
        {
            slae = new Slae(grid, inputFuncs, initApprox, true);
            ApplyBoundaryConditions(slae.Matrix!, slae.RhsVec!, area, boundaryConds);
            slae.Solve(accuracy);
            if (accuracy.AutoRelax)
            {
                relaxRatio = GetRelaxRatio(slae.ResVec!, grid, inputFuncs, initApprox, accuracy, area, boundaryConds, true);
            }

            initApprox = UpdateApprox(slae.ResVec!, initApprox, relaxRatio);
            iter++;
        } while (SlaeSolver.RelResidual(slae.NonLinMatrix, initApprox, slae.NonLinRhsVec) > accuracy.Eps &&
                 !SlaeSolver.CheckIsStagnate(slae.ResVec!, initApprox, accuracy.Delta) && iter < accuracy.MaxIter);
        stopWatch.Stop();
        var resultStats = new ResultStats
        {
            IterationsCount = iter,
            RelResidual = SlaeSolver.RelResidual(slae),
            RelTolerance = SlaeSolver.RelTolerance(slae, inputFuncs, grid),
            ElapsedTime = stopWatch.ElapsedMilliseconds
        };
        return (slae.ResVec!, resultStats);
    }
}