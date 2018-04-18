using System.IO;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ImageFunctionApp {

    public static class ProcessImage {

        private static ComputerVisionProcessor ComputerVisionProcessor = new ComputerVisionProcessor();
        private static ContentModerationProcessor ContentModerationProcessor = new ContentModerationProcessor();

        [FunctionName("ProcessImage")]
        public static async Task Run(
            [BlobTrigger("image-intake-function/{name}", Connection = "ImageStorageBlobConnection")]Stream myBlob, 
            string name,
            //[DocumentDB("ToDoList", "Items", Id = "id", ConnectionStringSetting = "myCosmosDB")] out dynamic document,
            ILogger log
        ) {

            log.LogInformation($"C# Blob trigger function processing blob\n Name:{name} \n Size: {myBlob.Length} bytes...");

            // Read in all of the data into a byte-array
            var memoryStream = new MemoryStream();
            await myBlob.CopyToAsync(memoryStream);
            var imageContent = memoryStream.ToArray();

            // Execute the call to the Computer Vision API
            var computerVisionTask = ComputerVisionProcessor.ProcessAsync(imageContent);
            var contentModerationTask = ContentModerationProcessor.ProcessAsync(imageContent);

            // Format the raw JSON string into a pretty, formatted JSON string
            var computerVisionJson = CleanJson(await computerVisionTask);
            log.LogInformation($"C# Blob trigger function done processing blob\n Name:{name} with JSON {computerVisionJson}!");

            // Format the raw JSON string into a pretty, formatted JSON string
            var contentModerationJson = CleanJson(await contentModerationTask);
            log.LogInformation($"C# Blob trigger function done processing blob\n Name:{name} with JSON {contentModerationJson}!");

            log.LogInformation(computerVisionJson);
            log.LogInformation(contentModerationJson);
        }

        private static string CleanJson(string dirtyJson) {
            
            var jsonObj = JsonConvert.DeserializeObject<JObject>(dirtyJson);
            var cleanJson = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            return cleanJson;
        }
    }
}