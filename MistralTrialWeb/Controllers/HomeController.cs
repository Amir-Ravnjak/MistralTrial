using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MistralTrialAPI.Data;
using MistralTrialWeb.Models;
using MistralTrialWeb.ViewModels;
using MistralTrialWeb.Helper;
using Newtonsoft.Json;

namespace MistralTrialWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult get10Titles(TitleTypes titleType, int position=0,string query="")
        {
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri("https://mistraltrialapi.azurewebsites.net/");
            HttpResponseMessage response = new HttpResponseMessage();

            Authentication.AuthorizeApi(client, response);

            response = client.GetAsync("api/titles").Result;

            

            List<Title> titles = response.Content.ReadAsAsync<List<Title>>().Result;

            HomeTitlesVM model = new HomeTitlesVM();
            model.rows = titles.Where(z=>z.Type==titleType).Where(g=>lowercase(g.Name).Contains(query.ToLower())).OrderByDescending(q => AverageRate(q.Id)).Skip(position).Take(10).Select(x => new HomeTitlesVM.Row
            {
                Id = x.Id,
                AverageGrade = AverageRate(x.Id),
                Description = x.Description,
                Name = x.Name,
                ImageId = x.ImgFileId,
                actors = getActorsByMovie(x.Id)
            }).ToList();

            return View(model);
        }

        private string lowercase(string s)
        {
            return s.ToLower();
        }

        public IActionResult TitleList(TitleTypes titleType)
        {
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri("https://mistraltrialapi.azurewebsites.net/");
            HttpResponseMessage response = new HttpResponseMessage();

            Authentication.AuthorizeApi(client, response);

            response = client.GetAsync("api/titles").Result;

            List<Title> titles = response.Content.ReadAsAsync<List<Title>>().Result;

            HomeTitleListVM model = new HomeTitleListVM();
            model.titleType = titleType;
            model.rows = titles.Where(z => z.Type == titleType).OrderBy(w => w.Name).OrderByDescending(q => AverageRate(q.Id)).Select(x => new HomeTitleListVM.Row
            {
                Id = x.Id,
                AverageGrade = AverageRate(x.Id),
                Name = x.Name,
                ImageId = x.ImgFileId,
                Rating = getRateForUserAndTitle(x.Id)
            }).ToList();


            return View(model);
        }

        private float getRateForUserAndTitle(int titleId)
        {
            User u = HttpContext.GetLoggedUser();
            if (u == null)
                return 0;


            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri("https://mistraltrialapi.azurewebsites.net/");
            HttpResponseMessage response = new HttpResponseMessage();

            Authentication.AuthorizeApi(client, response);
            response = client.GetAsync("api/rates/"+titleId+"/"+u.id).Result;
            Rate r = response.Content.ReadAsAsync<Rate>().Result;

            if (r != null)
                return r.Grade;
            else
                return 0;

        }

        public IActionResult RateTitle(int titleId,int rate,TitleTypes type)
        {
            User u = HttpContext.GetLoggedUser();
            if (u == null)
                return Redirect("/Authentication/Login?message='Please log in to rate titles.'");

            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri("https://mistraltrialapi.azurewebsites.net/");
            HttpResponseMessage response = new HttpResponseMessage();

            Authentication.AuthorizeApi(client, response);

            response = client.GetAsync("api/rates").Result;
            List<Rate> allRates = response.Content.ReadAsAsync<List<Rate>>().Result;

            Rate r = allRates.Where(x => x.TitleId == titleId && x.UserId == u.id).FirstOrDefault();

            if (r != null) {
                r.Grade = rate;
                response = client.PutAsJsonAsync("api/rates/"+r.Id,r).Result;
            }
            else
            {
                response = client.PostAsJsonAsync("api/rates", new Rate { TitleId = titleId, UserId = u.id, Grade = rate }).Result;
            }


            return Redirect("/Home/TitleList?titleType="+type);
        }

        private float AverageRate(int titleId)
        {
            float averageRate = 0;

            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri("https://mistraltrialapi.azurewebsites.net/");
            HttpResponseMessage response = new HttpResponseMessage();

            Authentication.AuthorizeApi(client, response);

            response = client.GetAsync("api/rates").Result;
            List<Rate> allRates = response.Content.ReadAsAsync<List<Rate>>().Result;

            List<Rate> titleRates = allRates.Where(x => x.TitleId == titleId).ToList();

            if (titleRates.Count != 0)
                averageRate = (float)titleRates.Average(w => w.Grade);


            return averageRate;
        }
        

        private List<Actors> getActorsByMovie(int titleId)
        {
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri("https://mistraltrialapi.azurewebsites.net/");
            HttpResponseMessage response = new HttpResponseMessage();

            Authentication.AuthorizeApi(client, response);

            response = client.GetAsync("api/TitleActors/" + titleId).Result;
            List<TitleActors> titleactors = response.Content.ReadAsAsync<List<TitleActors>>().Result;

            List<Actors> actors = new List<Actors>();

            foreach (var w in titleactors)
            {
                response = client.GetAsync("api/Actors/" + w.ActorsId).Result;
                actors.Add(response.Content.ReadAsAsync<Actors>().Result);
            }
            
            return actors;
            
        }

        [HttpGet]
        public FileStreamResult ViewImage(int ImageId)
        {
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri("https://mistraltrialapi.azurewebsites.net/");
            HttpResponseMessage response = new HttpResponseMessage();

            Authentication.AuthorizeApi(client, response);

            response = client.GetAsync("api/ImgFiles/"+ImageId).Result;
            

            ImgFile image = response.Content.ReadAsAsync<ImgFile>().Result;
            MemoryStream ms = new MemoryStream(image.Podaci);
            return new FileStreamResult(ms, image.Tip);
        }
        


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
