using System.Text.Json;

namespace YMF_1;

public static class Program
{
    public static void Main()
    {
        var input = JsonSerializer.Deserialize<InputJsonModel>(File.ReadAllText("input.json"));
        var grid = new Grid(input);
        var boundaryConditions =
            JsonSerializer.Deserialize<BoundaryConditionsJsonModel>(File.ReadAllText("BoundaryConditions.json"));
        var matrix = new Matrix(grid); 
        var bom = 0;
    }
}

