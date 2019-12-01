using System;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Adrienne.Functions
{
    public static class SpinDreidel
    {
        [FunctionName("SpinDreidel")]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Dreidel has been spinned...");

            // Choose random dreidel result
            Random random = new Random();
            int dreidelResult = random.Next(0, 4);

            // Grab corresponding dreidel result gif
            MemoryStream stream = new MemoryStream();

            using (FileStream fileStream = File.OpenRead($"./DreidelResults/{dreidelResult}.gif")) {
                stream.SetLength(fileStream.Length);
                fileStream.Read(stream.GetBuffer(), 0, (int)fileStream.Length);
            };

            // Return dreidel spin result to user!
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/gif");

            return response;
        }
    }
}
