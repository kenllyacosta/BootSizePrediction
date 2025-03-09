using ScottPlot;

namespace MatPlot
{
    /// <summary>
    /// Class to create a plot with a linear regression line. Install ScottPlot package to use this class.
    /// </summary>
    public class LinearRegressionPlot
    {
        // Title of the plot
        public string Title { get; set; } = "Regression Line Title";

        // Leyend of the plot
        public Alignment LegendsAligment { get; set; } = Alignment.UpperLeft;

        // Marker color of the plot
        public Color MarkerColor { get; set; } = Color.FromHex("#1F77B4");

        // X axis label
        public string XValuesTitle { get; set; } = "Regression X Line Title";

        // Y axis label
        public string YValuesTitle { get; set; } = "Regression Y Line Title";

        // Legend text for the regression line
        public string RegressionLineLegendText { get; set; } = "Regression Line";
        public Color RegressionLineColor { get; set; } = Color.FromHex("#FF7F0E");
        public float RegessionLineWidth { get; set; } = 2;

        public Plot CreatePlot(double[] xValues, double[] yValue, double slop, double intercept)
        {
            Plot MyPlot = new();
            MyPlot.Axes.Title.Label.Text = Title;
            MyPlot.Axes.Bottom.Label.Text = XValuesTitle;
            MyPlot.Axes.Left.Label.Text = YValuesTitle;
            MyPlot.ShowLegend().Alignment = LegendsAligment;

            MyPlot.Add.Markers(xValues, yValue).Color = MarkerColor;

            var MinX = xValues.Min();
            var MaxX = xValues.Max();
            var MinY = slop * MinX + intercept;
            var MaxY = slop * MaxX + intercept;

            var RegressionLine = MyPlot.Add.Line(MinX, MinY, MaxX, MaxY);
            RegressionLine.Color = RegressionLineColor;
            RegressionLine.LineWidth = RegessionLineWidth;
            RegressionLine.LegendText = RegressionLineLegendText;

            return MyPlot;
        }

        public void CreatePlotPng(double[] xValues, double[] yValue, int width, int height, string filePath, double slop, double intercept)
        {
            string directoryPath = Path.GetDirectoryName(filePath)!;
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            Plot MyPlot = CreatePlot(xValues, yValue, slop, intercept);
            MyPlot.SavePng(filePath, width, height);
        }
    }
}