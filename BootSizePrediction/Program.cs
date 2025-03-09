// Data: https://msmdotnet.github.io/machinelearningintro/content/resources/harnessandboots.txt
using BootSizePrediction.Entities;
using MatPlot;
using StatsModels.Models;
using StatsModels.Readers;

double[] HarnessSize = [ 58, 58, 52, 58, 57, 52, 55, 53, 49, 54,
                         59, 56, 53, 58, 57, 58, 56, 51, 50, 59,
                         59, 59, 55, 50, 55, 52, 53, 54, 61, 56,
                         55, 60, 57, 56, 61, 58, 53, 57, 57, 55,
                         60, 51, 52, 56, 55, 57, 58, 57, 51, 59 ];

double[] BootSize = [ 39, 38, 37, 39, 38, 35, 37, 36, 35, 40,
                      40, 36, 38, 39, 42, 42, 36, 36, 35, 41,
                      42, 38, 37, 35, 40, 36, 35, 39, 41, 37,
                      35, 41, 39, 41, 42, 42, 36, 37, 37, 39,
                      42, 35, 36, 41, 41, 41, 39, 39, 35, 39 ];

Ols Model = new(HarnessSize, BootSize);

// Fit the model
Model.Fit();

Console.WriteLine($"Slop: {Model.Slop}");
Console.WriteLine($"Intercept: {Model.Intercept}");

Console.WriteLine($"La ecuación de la línea es: y = {Model.Slop}x + {Model.Intercept}");

double HarnessValue = 55;
Console.WriteLine($"Tamaño aproximado de bota para el harnes {HarnessValue} es {Model.Predict(HarnessValue)}");

double[] Predictiondata = [52.5, 53, 58, 61, 57, 60, 61];
double[] Predictions = Model.Predict(Predictiondata);

for (int i = 0; i < Predictiondata.Length; i++)
    Console.WriteLine($"Tamaño aproximado de bota para el harnes {Predictiondata[i]} es {Predictions[i]}");

// Hasta aquí el código de la predicción de tallas de botas en la versión 1

//Leemos los datos ahora de un archivo de texto separado por comas adjunto al proyecto
string DataFile = "Resources\\doggy-boot-harness.csv";
IEnumerable<DoggyBootHarness> Data = CsvReader.Read<DoggyBootHarness>(DataFile);

var HarnessSizeFromFile = Data.Select(x => x.HarnessSize).ToArray();
var BootSizeFromFile = Data.Select(x => x.BootSize).ToArray();

// Create Model
Ols NewModel = new(HarnessSizeFromFile, BootSizeFromFile);

// Train the model
NewModel.Fit();

// Save the model
NewModel.SaveModel("Resources\\DoggyBootHarnessModel.json");

// Load the model
Ols Model2 = Ols.LoadModel("Resources\\DoggyBootHarnessModel.json");

string Imagepath = "Resources\\DoggyBootHarness.png";
LinearRegressionPlot Plot = new() { Title = "Linear Regression", XValuesTitle = "Harness Size", YValuesTitle = "Boot Size" };
Plot.CreatePlotPng(HarnessSize, BootSize, 1024, 768, Imagepath, Model2.Slop, Model2.Intercept);

Console.WriteLine("Hello, World!");
Console.ReadLine();