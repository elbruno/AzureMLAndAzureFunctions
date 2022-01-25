using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionsCsNet6
{
    public static class AzAzureML21
    {
        [FunctionName("AzAzureML21")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var predictionResult = new MlnetSquirrels.ModelOutput();
            predictionResult.Label = 0;
            predictionResult.PredictedLabel = "not processed";
            predictionResult.ImageSource = null;
            predictionResult.Score = new float[2];
            predictionResult.Score[0] = 0;
            predictionResult.Score[1] = 0;

            try
            {

                log.LogInformation($"ML.NET model loaded from {MlnetSquirrels.GetModelPath()}");

                using var imageInStream = new MemoryStream();
                req.Body.CopyTo(imageInStream);

                // var imageBytes = File.ReadAllBytes(imagePath);
                var imageBytes = imageInStream.ToArray();
                var sampleData = new MlnetSquirrels.ModelInput()
                {
                    ImageSource = imageBytes,
                };
                // Make a single prediction on the sample data and print results
                predictionResult = MlnetSquirrels.Predict(sampleData);
                var logData = $"\n\nPredicted Label value: {predictionResult.PredictedLabel} \nPredicted Label scores: [{string.Join(",", predictionResult.Score)}]\n\n";
                log.LogInformation(logData);

                predictionResult.ImageSource = null;
            }
            catch (Exception ex)
            {
                var message = $"Exc: " + ex.Message;
                predictionResult.PredictedLabel = message;
            }

            var result = JsonConvert.SerializeObject(predictionResult, Formatting.Indented);

            return new OkObjectResult(result);
        }
    }
}
