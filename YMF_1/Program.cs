using System.Text.Json;

namespace YMF_1;

public static class Program
{
    public static void Main()
    {
        var data = File.ReadAllText("input.json");
        var input = JsonSerializer.Deserialize<InputModel>(data);
        var grid = new Grid(input);
        var matrix = new Matrix(grid); 
        var bom = 0;
    }
}

