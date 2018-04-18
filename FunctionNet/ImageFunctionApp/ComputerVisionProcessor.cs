using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ImageFunctionApp {

    public class ComputerVisionProcessor {

        private static readonly string SUBSCRIPTION_KEY;
        private const string SUBSCRIPTION_URL = "https://westus2.api.cognitive.microsoft.com/vision/v1.0";

        static ComputerVisionProcessor() {
            SUBSCRIPTION_KEY = AppSettings.GetEnvironmentVariable("ComputerVisionApiKey");
        }
        
        public async Task<string> ProcessAsync(byte[] imageData) {

            using (var client = new HttpClient()) {

                // Set the request header for the Subscription Key, which is required
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SUBSCRIPTION_KEY);

                // Request parameters. A third optional parameter is "details".
                var requestParameters = "visualFeatures=Categories,Description,Color,Tags,Faces,ImageType,Adult&language=en";

                // Assemble the URI for the REST API Call.
                var uri = $"{SUBSCRIPTION_URL}/analyze?{requestParameters}";

                string result = String.Empty;

                using (var content = new ByteArrayContent(imageData)) {

                    // Set the content-type header into the HTTP request
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    // Execute the REST API call to the Computer Vision API
                    var response = await client.PostAsync(uri, content);

                    // Get the JSON response
                    result = await response.Content.ReadAsStringAsync();
                }

                // Return the JSON value as a string
                return result;               
            }
        }
    }
}