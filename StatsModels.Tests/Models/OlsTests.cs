using StatsModels.Models;
using System.Text.Json;

namespace StatsModels.Tests.Models
{
    public class OlsTests
    {
        [Fact]
        public void Fit_ShouldCalculateCorrectSlopAndIntercept()
        {
            // Arrange
            double[] xValues = { 1, 2, 3, 4, 5 };
            double[] yValues = { 2, 4, 6, 8, 10 };
            var ols = new Ols(xValues, yValues);

            // Act
            ols.Fit();

            // Assert
            Assert.Equal(2, ols.Slop, 5);
            Assert.Equal(0, ols.Intercept, 5);
        }

        [Fact]
        public void Fit_ShouldThrowArgumentException_WhenXValuesAndYValuesLengthMismatch()
        {
            // Arrange
            double[] xValues = { 1, 2, 3 };
            double[] yValues = { 2, 4 };
            var ols = new Ols(xValues, yValues);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => ols.Fit());
        }

        [Fact]
        public void Predict_ShouldReturnCorrectPrediction()
        {
            // Arrange
            double[] xValues = { 1, 2, 3, 4, 5 };
            double[] yValues = { 2, 4, 6, 8, 10 };
            var ols = new Ols(xValues, yValues);
            ols.Fit();

            // Act
            double prediction = ols.Predict(6);

            // Assert
            Assert.Equal(12, prediction, 5);
        }

        [Fact]
        public void PredictArray_ShouldReturnCorrectPredictions()
        {
            // Arrange
            double[] xValues = { 1, 2, 3, 4, 5 };
            double[] yValues = { 2, 4, 6, 8, 10 };
            var ols = new Ols(xValues, yValues);
            ols.Fit();

            // Act
            double[] predictions = ols.Predict(new double[] { 6, 7 });

            // Assert
            Assert.Equal(new double[] { 12, 14 }, predictions);
        }

        [Fact]
        public void SaveModel_ShouldSaveModelToFile()
        {
            // Arrange
            double[] xValues = { 1, 2, 3, 4, 5 };
            double[] yValues = { 2, 4, 6, 8, 10 };
            var ols = new Ols(xValues, yValues);
            ols.Fit();
            string filePath = "test_model.json";

            // Act
            ols.SaveModel(filePath);

            // Assert
            Assert.True(File.Exists(filePath));

            // Cleanup
            File.Delete(filePath);
        }

        [Fact]
        public void LoadModel_ShouldLoadModelFromFile()
        {
            // Arrange
            double[] xValues = { 1, 2, 3, 4, 5 };
            double[] yValues = { 2, 4, 6, 8, 10 };
            var ols = new Ols(xValues, yValues);
            ols.Fit();
            string filePath = "test_model.json";
            ols.SaveModel(filePath);

            // Act
            var loadedOls = Ols.LoadModel(filePath);

            // Assert
            Assert.Equal(ols.Slop, loadedOls.Slop, 5);
            Assert.Equal(ols.Intercept, loadedOls.Intercept, 5);

            // Cleanup
            File.Delete(filePath);
        }

        [Fact]
        public void LoadModelfromJson_ShouldLoadModelFromJsonString()
        {
            // Arrange
            double[] xValues = { 1, 2, 3, 4, 5 };
            double[] yValues = { 2, 4, 6, 8, 10 };
            var ols = new Ols(xValues, yValues);
            ols.Fit();
            string jsonModel = JsonSerializer.Serialize(ols);

            // Act
            var loadedOls = Ols.LoadModelfromJson(jsonModel);

            // Assert
            Assert.Equal(ols.Slop, loadedOls.Slop, 5);
            Assert.Equal(ols.Intercept, loadedOls.Intercept, 5);
        }
    }
}