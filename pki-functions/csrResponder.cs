using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MyFunctionProj
{
    public static class csrResponder
    {
        [FunctionName("csrResponder")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("csr Responder function triggered");

            //Getting the body of the request..
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            if( string.IsNullOrEmpty(requestBody))
            {
                return new BadRequestObjectResult("CSR wasn't found in teh body of the request.");
            }

            //do the magic stuff calling open ssl with process start..

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "openssl";
            process.StartInfo.Arguments = "version";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string err = process.StandardError.ReadToEnd();
            process.WaitForExit();



            //reply with the certificate...   just for this test return the version of openssl

            return new OkObjectResult($"Certificate can be signed with Open SSL version {output}");
        }
    }
}
