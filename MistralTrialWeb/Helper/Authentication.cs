using Microsoft.AspNetCore.Http;
using MistralTrialAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MistralTrialWeb.Helper
{
    public static class Authentication
    {
        private const string loggedUser = "loggedUser";

        public static void SetLoggedUser(this HttpContext context, User user)
        {
            context.Session.SetJson(loggedUser, user);
        }

        public static User GetLoggedUser(this HttpContext context)
        {
            return context.Session.GetJson<User>(loggedUser);
        }

        public static void AuthorizeApi(HttpClient client,HttpResponseMessage response)
        {

            client.DefaultRequestHeaders.Add("username", "admin");
            client.DefaultRequestHeaders.Add("password", "admin");
            response = client.GetAsync("token/createtoken").Result;

            string accessToken = response.Content.ReadAsStringAsync().Result;

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
        }
    }
}
