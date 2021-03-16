using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlackExample.Utilities
{
    public static class Consts
    {
        public static string AUTH_BEARER_TOKEN = "xoxp-1832356203655-1847332698291-1853428774244-9dea4520999cf6e4b2ad78468eb4d1f7";
        public static string SLACK_API_GET_USERS = "https://slack.com/api/users.list";
        public static string SLACK_API_POST_MESSAGE = "https://slack.com/api/chat.postMessage";
    }
}
