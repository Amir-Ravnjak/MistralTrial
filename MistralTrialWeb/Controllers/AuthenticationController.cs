using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MistralTrialAPI.Data;
using MistralTrialWeb.Helper;

namespace MistralTrialWeb.Controllers
{
    public class AuthenticationController : Controller
    {
        [HttpGet]
        public IActionResult Login(string message)
        {
            TempData["message"] = message;
            return View();
        }
        [HttpPost]
        public IActionResult Login(string username, string password)
        {

            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri("https://mistraltrialapi.azurewebsites.net/");
            HttpResponseMessage response = new HttpResponseMessage();

            Authentication.AuthorizeApi(client, response);
            response = client.GetAsync("api/users/searchByUsername/" + username).Result;
            User u = response.Content.ReadAsAsync<User>().Result;

            
            if (u.PasswordHash == generateHash(password, u.PasswordSalt))
            {
                HttpContext.SetLoggedUser(u);
                return Redirect("/");
            }

            

            return Redirect("/Authentication/Login");
        }

        [HttpGet]
        public IActionResult Register(string message)
        {
            TempData["errorMsg"] = message;
            return View();
        }
        [HttpPost]
        public IActionResult Register(string username, string password, string email)
        {

            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri("https://mistraltrialapi.azurewebsites.net/");
            HttpResponseMessage response = new HttpResponseMessage();

            Authentication.AuthorizeApi(client, response);

            response = client.GetAsync("api/users/searchByUsername/" + username).Result;
            User us = response.Content.ReadAsAsync<User>().Result;

            if (us!=null)
                return Redirect("/Authentication/Register?message=username already exists");

            User u = new User();

            u.Username = username;
            u.email = email;
            u.PasswordSalt = generateSalt();
            u.PasswordHash = generateHash(password,u.PasswordSalt);

            HttpResponseMessage response2 = client.PostAsJsonAsync("api/users", u).Result;


            return Redirect("/");
        }
        public IActionResult LogOut()
        {
            HttpContext.SetLoggedUser(null);

            return Redirect("/");
        }

        private string generateSalt()
        {
            byte[] arr = new byte[16];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetBytes(arr);
            return Convert.ToBase64String(arr);             
        }

        private string generateHash(string password, string salt)
        {
            byte[] bytePassword = Encoding.Unicode.GetBytes(password);
            byte[] byteSalt = Convert.FromBase64String(salt);

            byte[] forHashing = new byte[bytePassword.Length + byteSalt.Length];
            Buffer.BlockCopy(bytePassword, 0, forHashing, 0, bytePassword.Length);
            Buffer.BlockCopy(byteSalt, 0, forHashing, bytePassword.Length, byteSalt.Length);

            HashAlgorithm alg = HashAlgorithm.Create("SHA1");

            return Convert.ToBase64String(alg.ComputeHash(forHashing));
        }



    }
}