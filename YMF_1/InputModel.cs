namespace YMF_1;

public class InputModel
{
    public int Lambda { get; set; }
    public int Gamma { get; set; }

    public double[] DischargeRatioX { get; set; }
    public double[] DischargeRatioY { get; set; }

    public int[] AmountPointsX { get; set; }
    public int[] AmountPointsY { get; set; }

    public int MaxIter { get; set; }
    public double Eps { get; set; }
    public double Delta { get; set; }

    public double[] OmegaX { get; set; }
    public double[] OmegaY { get; set; }
}