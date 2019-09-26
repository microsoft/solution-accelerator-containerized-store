using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Microsoft.Cognitive.CustomVision.Models;

using Newtonsoft.Json;

namespace Microsoft.Cognitive.CustomVision.Prediction
{
    public class CustomVision
    {
        public static async Task<ImagePrediction> Predict(Stream stream, string PredictionKey, string PredictionUri)
        {
            var client = new HttpClient();

            // Request headers - replace this example key with your valid subscription key.
            client.DefaultRequestHeaders.Add("Prediction-Key", PredictionKey);
            

            // Prediction URL - replace this example URL with your valid prediction URL.
            string url = PredictionUri;

            HttpResponseMessage response;

            // Request body. Try this sample with a locally stored image.
            byte[] byteData = GetImageAsByteArray(stream);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(url, content);

                return JsonConvert.DeserializeObject<ImagePrediction>(await response.Content.ReadAsStringAsync());
            }
        }

        static byte[] GetImageAsByteArray(Stream stream)
        {
            BinaryReader binaryReader = new BinaryReader(stream);
            return binaryReader.ReadBytes((int)stream.Length);
        }
    }
}
