using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ImageFunctionApp {

    public static class CheckHealth {

        [FunctionName("CheckHealth")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequestMessage req,
            ILogger log
        ) {

            log.LogInformation("C# HTTP trigger function processed a request.");

            return await Task.FromResult<HttpResponseMessage>(
                new HttpResponseMessage(System.Net.HttpStatusCode.OK) {
                    Content = new StringContent("Status: Healthy")
                }
            );
        }
    }
}
