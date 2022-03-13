using System.Text.Json;

namespace YMF_1;

public static class Program
{
    public static void Main()
    {
        // Read data from Input.json, get parameters
        /*
        Lambda: 1
        Gamma: 1
        DischargeRatioX: [1,1], Discharge ratio for X for every part
        DischargeRatioY: [1,1], Discharge ratio for Y for every part
        AmountPointsX: [5,5], Amount of points by X for every part
        AmountPointsY: [5,5], Number of points in Y for every part
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
        Utils.WriteTable(slae.Result, grid);
        // For breakpoint
        var bom = 0;
    }
}