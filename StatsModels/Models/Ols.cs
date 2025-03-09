using System.Text.Json;
using System.Text.Json.Serialization;

namespace StatsModels.Models
{
    /// <summary>
    /// Ordinary Least Squares (OLS) is a method for estimating the unknown parameters in a linear regression model.
    /// </summary>
    /// <param name="xValues"></param>
    /// <param name="yValues"></param>
    public class Ols(double[] xValues, double[] yValues)
    {
        public double Slop { get; private set; }
        public double Intercept { get; private set; }

        private double[] XValues { get; set; } = xValues;
        private double[] YValues { get; set; } = yValues;

        [JsonConstructor]
        public Ols(double slop, double intercept) : this([], [])
        {
            Slop = slop;
            Intercept = intercept;
        }

        /// <summary>
        /// Fit the model
        /// </summary>
        public void Fit()
        {
            // Fit the model
            int N = XValues.Length;
            int N2 = YValues.Length;
            if (N != N2)
                throw new ArgumentException("The number of elements in xValues and yValues must be equal.");

            double SumXY = 0;
            double SumX = 0;
            double SumY = 0;
            double SumXX = 0;

            for (int i = 0; i < N; i++)
            {
                SumXY += XValues[i] * YValues[i];
                SumX += XValues[i];
                SumY += YValues[i];
                SumXX += XValues[i] * XValues[i];
            }

            Slop = (N * SumXY - SumX * SumY) / (N * SumXX - SumX * SumX);

            double MeanX = SumX / N;
            double MeanY = SumY / N;
            Intercept = MeanY - Slop * MeanX;
        }

        public double Predict(double xValue)
            => Slop * xValue + Intercept;

        public double[] Predict(double[] xValues)
            => [.. xValues.Select(x => Predict(x))];

        //Save the model
        public void SaveModel(string filePath)
        {
            var JsonString = JsonSerializer.Serialize(this);
            File.WriteAllText(filePath, JsonString);
        }

        public static Ols LoadModel(string filePath)
        {
            var JsonString = File.ReadAllText(filePath);
            return LoadModelfromJson(JsonString);
        }

        public static Ols LoadModelfromJson(string jsonModel)
            => JsonSerializer.Deserialize<Ols>(jsonModel)!;
    }
}