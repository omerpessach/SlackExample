using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SlackExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentScheduler;
using SlackExample.Utilities;

namespace SlackExample.Controllers
{
    [Route("")]
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulerMessageController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SchedulerMessage msg)
        {
            TimeSpan ts;
            IActionResult response = Ok();

            JobManager.Initialize();

            if (!TimeSpan.TryParse(msg.Time, out ts))
            {
                response = BadRequest("Time span is not correct");
            }
            else
            {
                // Send DM for all user every day at x:y (x:Hours, y:Minutes).
                JobManager.AddJob(() => SendDirectMessageToEveryone(msg.Message), s => s.ToRunEvery(1).Days().At(ts.Hours, ts.Minutes));
            }

            return response;
        }

        private async Task SendDirectMessageToEveryone(string text)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Consts.AUTH_BEARER_TOKEN);
                var getUsersResult = await client.GetAsync(Consts.SLACK_API_GET_USERS);

                var getUsersResultAsString = await getUsersResult.Content.ReadAsStringAsync();
                var parsedObject = JObject.Parse(getUsersResultAsString);

                // Send direct message to all the users of the workspace
                foreach (var member in parsedObject["members"].AsParallel())
                {
                    await SendDirectMessage(client, text, member["id"].ToString());
                }
            }
        }

        private async Task SendDirectMessage(HttpClient client, string text, string user)
        {
            SlackMessage sceduleMsg = new SlackMessage
            {
                Text = text,
                UserID = user
            };

            var content = new StringContent(JsonConvert.SerializeObject(sceduleMsg), Encoding.UTF8, "application/json");
            client.PostAsync(Consts.SLACK_API_POST_MESSAGE, content);
        }
    }
}