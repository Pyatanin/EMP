using System.Text.Json;

namespace YMF_1;

public static class Program
{
    public static void Main()
    {
        // Read data from Input.json, get parameters
        /*
        Test: 0, Test number
        Lambda: 1
        Gamma: 1
        DischargeRatioX: 1, Discharge ratio for X
        DischargeRatioY: 1, Discharge ratio for Y
        AmountPointsX: 5, Amount of points by X
        AmountPointsY: 5, Number of points in Y
        MaxIter": 1000
        Eps: 1.0e-7,
        Delta: 1.0e-7,
        OmegaX: [0, 2, 4], X pivot points
        OmegaY: [0, 2, 4] Y anchor points
        */
        var input = JsonSerializer.Deserialize<InputModel>(File.ReadAllText("Input.json"));
        // Assembling a grid with numbered points
        var grid = new Grid(input!);
        // eading data from BoundaryConditions.json for information about boundary conditions
        var boundaryConditions =
            JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText("BoundaryConditions.json"));
        var slae = new Slae(input!, grid, boundaryConditions!);
        // SLAE solution subroutine
        slae.Solve(input!);
        var result = Utils.ExcludeFictive(slae.Result, grid);
        // For breakpoint
        var bom = 0;
    }
}