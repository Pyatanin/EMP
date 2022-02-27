using System.Text.Json;

namespace YMF_1;

public static class Program
{
    public static void Main()
    {
        var input = JsonSerializer.Deserialize<InputModel>(File.ReadAllText("Input.json"));
        var grid = new Grid(input!);
        var boundaryConditions =
            JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText("BoundaryConditions.json"));
        var slae = new Slae(input!, grid, boundaryConditions!);

        slae.Solve(input!);
        var result = Utils.ExcludeFictive(slae.Result, grid);
        // For breakpoint
        var bom = 0;
    }
}