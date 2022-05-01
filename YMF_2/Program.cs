using System.Text.Json;
using YMF_2.Fem;
using YMF_2.JsonModels;

namespace YMF_2;

public static class Program
{
    public static void Main()
    {
        //Serializing data in JSON format
        var area = JsonSerializer.Deserialize<Area>(File.ReadAllText("input/area.json"))!;
        var inputFuncs =
            JsonSerializer.Deserialize<InputFuncs>(File.ReadAllText("input/inputFuncs.json"))!;
        var boundaryConditions =
            JsonSerializer.Deserialize<BoundaryConditions>(
                File.ReadAllText("input/boundaryConditions.json"))!;
        var accuracy = JsonSerializer.Deserialize<Accuracy>(File.ReadAllText("input/accuracy.json"))!;
        var grid = new Grid(area);
        // var (result, resultStats) = Solver.SolveWithSimpleIteration(grid, inputFuncs, area, boundaryConditions, accuracy);
        var (result, resultStats) = Solver.SolveWithNewton(grid, inputFuncs, area, boundaryConditions, accuracy);
        Utils.WriteTable(result, grid, inputFuncs);
        Utils.WriteJson(resultStats);
    }
}