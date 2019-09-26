using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace ObjectDetection
{
    class Program
    {
        private const string Endpoint = "https://westus2.api.cognitive.microsoft.com/";
        // Add your training & prediction key from the settings page of the portal
        private const string TrainingKey = "211a18db946f4df791e0c6e84863c6cd";
        private const string PredictionKey = "72e398e5fea343abb0632bd1a31dc1a0";
        private const string ResourceId = "/subscriptions/504ff7b2-2d23-4878-8777-66044673165d/resourceGroups/AzureCognitive.CustomVisionArmTemplate/providers/Microsoft.CognitiveServices/accounts/myCustomVisionServ_Prediction";
        private static CustomVisionTrainingClient TrainingApi;

        static void CreateProject(string ProjectName)
        {
            // Find the object detection domain
            var domains = TrainingApi.GetDomains();
            var objDetectionDomain = domains.FirstOrDefault(d => d.Type == "ObjectDetection");

            // Create a new project
            Console.WriteLine("Creating new project:");
            var project = TrainingApi.CreateProject(ProjectName, null, objDetectionDomain.Id);
            PersistProjectId(project);
        }

        static void Train()
        {
            var settings = RetrieveSettings();
            var projectId = new Guid(settings["projectId"]);

            // Now there are images with tags start training the project
            Console.WriteLine("\tTraining");

            var iteration = TrainingApi.TrainProject(projectId);

            //The returned iteration will be in progress, and can be queried periodically to see when it has completed
            while (iteration.Status == "Training")
            {
                Thread.Sleep(1000);

                // Re-query the iteration to get its updated status
                iteration = TrainingApi.GetIteration(projectId, iteration.Id);
            }
            PersistIterationId(iteration);
        }

        static void Publish(string pubModelName)
        {
            var settings = RetrieveSettings();

            //The iteration is now trained. Publish it to the prediction end point.
            var predictionResourceId = ResourceId;
            TrainingApi.PublishIteration(new Guid(settings["projectId"]), new Guid(settings["iterationId"]), pubModelName, predictionResourceId);

            PersistModelName(pubModelName);
            Console.WriteLine("Done!\n");
        }

        static void Predict(string imageFile)
        {
            var settings = RetrieveSettings();
            // Now there is a trained endpoint, it can be used to make a prediction

            // Create a prediction endpoint, passing in the obtained prediction key
            CustomVisionPredictionClient endpoint = new CustomVisionPredictionClient()
            {
                ApiKey = PredictionKey,
                Endpoint = Endpoint
            };

            // Make a prediction against the new project
            Console.WriteLine("Making a prediction:");
            using (var stream = File.OpenRead(imageFile))
            {
                var result = endpoint.DetectImage(new Guid(settings["projectId"]), settings["modelName"], File.OpenRead(imageFile));

                //Loop over each prediction and write out the results
                foreach (var c in result.Predictions)
                {
                    Console.WriteLine($"\t{c.TagName}: {c.Probability:P1} [ {c.BoundingBox.Left}, {c.BoundingBox.Top}, {c.BoundingBox.Width}, {c.BoundingBox.Height} ]");
                }
            }
            Console.ReadKey();
        }

        // For testing purposes
        private static void RetrieveTaggedImages()
        {
            var settings = RetrieveSettings();
            var tagImages = TrainingApi.GetTaggedImages(new Guid(settings["projectId"]));
        }


        static void Main(string[] args)
        {

            // Create the Api, passing in the training key
            TrainingApi = new CustomVisionTrainingClient()
            {
                ApiKey = TrainingKey,
                Endpoint = Endpoint
            };

            switch (args[0])
            {
                case "-c":
                    CreateProject(args[1]);
                    break;
                case "-t":
                    Train();
                    break;
                case "-pub":
                    Publish(args[1]);
                    break;
                case "-pred":
                    Predict(args[1]);
                    break;
                case "-imp":
                    RetrieveTaggedImages();
                    break;
                default:
                    Console.WriteLine("Use -c <project name> to create a project");
                    Console.WriteLine("Use -t to train the existing project");
                    Console.WriteLine("Use -pub <published model name> to create a prediction service");
                    Console.WriteLine("Use -pred <image file> to find objects in the specified image");
                    break;
            }
        }

        private static Dictionary<string,string> RetrieveSettings()
        {
            var rtrn = new Dictionary<string, string>();
            List<string> lines;
            try
            {
                lines = File.ReadLines("CustomVision.dat").ToList<string>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to retrieve the project id");
                Console.WriteLine(ex.Message);
                return rtrn;
            }

            foreach(var line in lines)
            {
                var strings = line.Split(",");
                rtrn.Add(strings[0], strings[1]);
            }

            return rtrn;
        }

        private static void PersistProjectId(Project project)
        {
            File.WriteAllText("CustomVision.dat", "projectId," + project.Id.ToString());
        }

        private static void PersistIterationId(Iteration it)
        {
            File.AppendAllText("CustomVision.dat", "iterationId," + it.Id.ToString());
        }

        private static void PersistModelName(string modelName)
        {
            File.AppendAllText("CustomVision.dat", "modelName," + modelName);
        }
    }
}
