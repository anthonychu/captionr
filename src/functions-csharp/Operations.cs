using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.AspNetCore.Mvc;
using CaptionR.Common;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Extensions.Primitives;

namespace CaptionR
{
    public static class Operations
    {
        [FunctionName(nameof(Negotiate))]
        public static IActionResult Negotiate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "{userId}/negotiate")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "captions", UserId = "{userId}")] SignalRConnectionInfo connectionInfo)
        {
            return new OkObjectResult(connectionInfo);
        }

        [FunctionName(nameof(Languages))]
        public static IActionResult Languages(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET")] HttpRequest req)
        {
            return new OkObjectResult(Constants.LANGUAGES);
        }

        [FunctionName(nameof(SelectLanguage))]
        public static async Task SelectLanguage(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST")] dynamic payload,
            [SignalR(HubName = "captions")] IAsyncCollector<SignalRGroupAction> signalRGroupActions)
        {
            var languageCode = payload.languageCode.ToString();

            var groupActionsTasks = 
                Constants.LANGUAGE_CODES.Select(lc => 
                    signalRGroupActions.AddAsync(new SignalRGroupAction
                    {
                        UserId = payload.userId,
                        GroupName = lc,
                        Action = lc == languageCode ? GroupAction.Add : GroupAction.Remove
                    }));

            await Task.WhenAll(groupActionsTasks);
        }

        [FunctionName(nameof(Captions))]
        public static async Task Captions(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST")] HttpRequest req,
            [SignalR(HubName = "captions")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            if (!IsAuthorized(req))
            {
                throw new Exception("Unauthorized"); // TODO: should return an HTTP response
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic payload = JsonConvert.DeserializeObject(requestBody);

            var languageCaptionsTasks = new List<Task>();
            var languages = payload.languages.ToObject<Dictionary<string, string>>();

            foreach (var language in languages)
            {
                var caption = new
                {
                    language = language.Key,
                    offset = payload.offset,
                    text = language.Value
                };

                languageCaptionsTasks.Add(signalRMessages.AddAsync(new SignalRMessage
                {
                    Target = "newCaption",
                    GroupName = language.Key,
                    Arguments = new[] { caption }
                }));
            }

            await Task.WhenAll(languageCaptionsTasks);
        }

        private static bool IsAuthorized(HttpRequest req)
        {
            var authorizedUser = Environment.GetEnvironmentVariable("AUTHORIZED_USER");
            if (string.IsNullOrEmpty(authorizedUser))
            {
                return true;
            }

            req.Headers.TryGetValue("x-ms-client-principal-name", out StringValues requestUser);
            return authorizedUser.ToLowerInvariant() == requestUser.First().ToLowerInvariant();
        }
    }
}