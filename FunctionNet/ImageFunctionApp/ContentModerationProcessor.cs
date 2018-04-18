using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ImageFunctionApp {

    public class ContentModerationProcessor {

        private static readonly string SUBSCRIPTION_KEY;
        private const string SUBSCRIPTION_URL = "https://westus2.api.cognitive.microsoft.com/contentmoderator/moderate/v1.0/ProcessImage/Evaluate?CasheImage=false";

        static ContentModerationProcessor() {
            SUBSCRIPTION_KEY = AppSettings.GetEnvironmentVariable("ContentModerationApiKey");
        }

        public async Task<string> ProcessAsync(byte[] imageData) {

            using (var client = new HttpClient()) {

                // Set the request header for the Subscription Key, which is required
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SUBSCRIPTION_KEY);
                
                string result = String.Empty;

                using (var content = new ByteArrayContent(imageData)) {

                    // Set the content-type header into the HTTP request
                    content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

                    // Execute the REST API call to the Computer Vision API
                    var response = await client.PostAsync(SUBSCRIPTION_URL, content);

                    // Get the JSON response
                    result = await response.Content.ReadAsStringAsync();
                }

                // Return the JSON value as a string
                return result;               
            }
        }
    }
}