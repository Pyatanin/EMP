using YMF_2.Fem;
using YMF_2.JsonModels;

namespace YMF_2.LinAlg;

public static class SlaeSolver
{
    public static double[] Iterate(double[] x, Matrix matrix, double w, double[] f)
    {
        for (var i = 0; i < x.Length; i++)
        {
            if (matrix.Center[i] != 0.0)
            {
                var sum = GeneralOperations.Dot(i, matrix, x);
                x[i] += w * (f[i] - sum) / matrix.Center[i];
            }
        }

        return x;
    }

    public static bool CheckIsStagnate(double[] prevVec, double[] x, double delta)
    {
        var difVec = new double[x.Length];

        for (var i = 0; i < x.Length; i++)
        {
            difVec[i] = prevVec[i] - x[i];
        }

        return Math.Abs(GeneralOperations.Norm(difVec)) < delta;
    }

    public static double RelResidual(Matrix matrix, double[] x, double[] f)
    {
        var diff = new double[f.Length];

        var innerProd = GeneralOperations.MatMul(matrix, x);

        for (var i = 0; i < f.Length; i++)
        {
            diff[i] = f[i] - innerProd[i];
        }

        return GeneralOperations.Norm(diff) / GeneralOperations.Norm(f);
    }

    public static double RelResidual(Slae slae)
    {
        var diff = new double[slae.RhsVec.Length];

        var innerProd = GeneralOperations.MatMul(slae.Matrix, slae.ResVec);

        for (var i = 0; i < slae.RhsVec.Length; i++)
        {
            diff[i] = slae.RhsVec[i] - innerProd[i];
        }

        return GeneralOperations.Norm(diff) / GeneralOperations.Norm(slae.RhsVec);
    }
    
    public static double Residual(Matrix matrix, double[] x, double[] f)
    {
        var diff = new double[f.Length];

        var innerProd = GeneralOperations.MatMul(matrix, x);

        for (var i = 0; i < f.Length; i++)
        {
            diff[i] = f[i] - innerProd[i];
        }

        return GeneralOperations.Norm(diff);
    }

    public static double RelTolerance(Slae slae, InputFuncs inputFuncs, Grid grid)
    {
        var calc = new Sprache.Calc.XtensibleCalculator();
        var toEvalUStar = calc.ParseFunction(inputFuncs.UStar).Compile();
        var diff = new double[slae.ResVec.Length];
        var uStarVec = new double[slae.ResVec.Length];
        for (var i = 0; i < slae.ResVec.Length; i++)
        {
            uStarVec[i] = toEvalUStar(Utils.MakeDict1D(grid.X[i]));
            diff[i] = Math.Abs(slae.ResVec[i] - uStarVec[i]);
        }
        return GeneralOperations.Norm(diff)/ GeneralOperations.Norm(uStarVec);
    }
}