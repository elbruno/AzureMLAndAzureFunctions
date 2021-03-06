// This file was auto-generated by ML.NET Model Builder. 
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
namespace FunctionsCsNet6
{
    public partial class MlnetSquirrels
    {
        /// <summary>
        /// model input class for MlnetSquirrels.
        /// </summary>
        #region model input class
        public class ModelInput
        {
            [ColumnName(@"Label")]
            public string Label { get; set; }

            [ColumnName(@"ImageSource")]
            public byte[] ImageSource { get; set; }

        }

        #endregion

        /// <summary>
        /// model output class for MlnetSquirrels.
        /// </summary>
        #region model output class
        public class ModelOutput
        {
            [ColumnName(@"Label")]
            public uint Label { get; set; }

            [ColumnName(@"ImageSource")]
            public byte[] ImageSource { get; set; }

            [ColumnName(@"PredictedLabel")]
            public string PredictedLabel { get; set; }

            [ColumnName(@"Score")]
            public float[] Score { get; set; }

        }

        #endregion

        //private static string MLNetModelPath = Path.GetFullPath("mlnetSquirrels.zip");

        public static readonly Lazy<PredictionEngine<ModelInput, ModelOutput>> PredictEngine = new Lazy<PredictionEngine<ModelInput, ModelOutput>>(() => CreatePredictEngine(), true);

        /// <summary>
        /// Use this method to predict on <see cref="ModelInput"/>.
        /// </summary>
        /// <param name="input">model input.</param>
        /// <returns><seealso cref=" ModelOutput"/></returns>
        public static ModelOutput Predict(ModelInput input)
        {
            var predEngine = PredictEngine.Value;
            return predEngine.Predict(input);
        }

        public static string GetModelPath()
        {
            var modelName = "mlnetSquirrels.zip";
            string currentAssemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var modelPath = Path.Combine(currentAssemblyPath, modelName);

            // check if model exists
            if (!File.Exists(modelPath))
            {
                // get parent directory from current assembly path
                var parentDirectory = Directory.GetParent(currentAssemblyPath).ToString();                               
                modelPath = Path.Combine(parentDirectory, modelName);
            }

            Console.WriteLine($"Model path: {modelPath}");

            return modelPath;
        }

        private static PredictionEngine<ModelInput, ModelOutput> CreatePredictEngine()
        {
            var mlContext = new MLContext();
            var modelPath = GetModelPath();
            ITransformer mlModel = mlContext.Model.Load(modelPath, out var _);
            return mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
        }
    }
}
