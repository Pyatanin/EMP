﻿using System.Text.Json;

namespace YMF_1;

public static class Program
{
    public static void Main()
    {
        var input = JsonSerializer.Deserialize<InputModel>(File.ReadAllText("input.json"));
        var grid = new Grid(input);
        var boundaryConditions =
            JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText("BoundaryConditions.json"));
        // var matrix = new Matrix(grid); 
        var bom = 0;
    }
}
