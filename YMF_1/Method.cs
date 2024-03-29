﻿namespace YMF_1;

public class Methods
{
    public static double Norm(double[] x) => Math.Sqrt(x.Sum(t => t * t));

    public static double[] MatMul(Matrix matrix, double[] vec)
    {
        var res = new double[vec.Length];

        for (var i = 0; i < vec.Length; i++)
        {
            res[i] = ScalarProd(i, matrix, vec);
        }

        return res;
    }

    public static double ScalarProd(int i, Matrix matrix, double[] vec)
    {
        var res = 0.0;
        res += matrix.Diag[i] * vec[i];
        
        if (i >= 0 && i < matrix.Size - 1)
        {
            res +=  matrix.UpperPart[0][i] * vec[i + 1];
        }

        if (i >= 0 && i < matrix.Size - matrix.Shift)
        {
            res += matrix.UpperPart[1][i] * vec[i + matrix.Shift];
        }

        if (i <= matrix.Size - 1 && i >= matrix.Shift)
        {
            res += matrix.LowPart[1][i - matrix.Shift] * vec[i - matrix.Shift];
        }

        if (i > 0 && i <= matrix.Size - 1)
        {
            res += matrix.LowPart[0][i - 1] * vec[i - 1];
        }
        
        return res;
    }

    public static double RelativeResidual(Matrix matrix, double[] x, double[] f)
    {
        var diff = new double[f.Length];

        var innerProd = MatMul(matrix, x);

        for (var i = 0; i < f.Length; i++)
        {
            diff[i] = f[i] - innerProd[i];
        }

        return Norm(diff) / Norm(f);
    }

    public static double[] IterateGs(double[] x, Matrix matrix, double w, double[] f)
    {
        for (var i = 0; i < x.Length; i++)
        {
            var sum = ScalarProd(i, matrix, x);
            x[i] += w * (f[i] - sum) / matrix.Diag[i];
        }

        return x;
    }

    public static bool CheckIsStagnate(double[] xPred, double[] x, double delta)
    {
        var vecDif = new double[x.Length];
        for (var i = 0; i < x.Length; i++)
        {
            vecDif[i] = xPred[i] - x[i];
        }

        return Math.Abs(Norm(vecDif)) < delta;
    }
}