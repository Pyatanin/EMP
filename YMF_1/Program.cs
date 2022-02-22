using System.Text.Json;

namespace YMF_1;

public static class Program
{
    public static void Main()
    {
        var data = File.ReadAllText("input.json");
        var input = JsonSerializer.Deserialize<InputModel>(data);
        var grid = new Grid(input);
        Matrix matrix = new Matrix(grid);
        var bom = 0;
    }
}

public class InputModel
{
    public int Test { get; set; }
    public bool Uniform { get; set; }
    public double[] Grid_X { get; set; }
    public double[] Grid_Y { get; set; }
    public double Discharge_ratio_X { get; set; }
    public double Discharge_ratio_Y { get; set; }
    public int Amount_points_X { get; set; }
    public int Amount_points_Y { get; set; }
    public double[] Omega_X { get; set; }
    public double[] Omega_Y { get; set; }
}