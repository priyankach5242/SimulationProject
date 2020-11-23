using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using MvcClient.Models;
using MvcClient.Models.ViewModel;
using Newtonsoft.Json;

namespace MvcClient.Controllers
{
    public class BookListController : Controller
    {
        LoginController lc = new LoginController();
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            if (HttpContext.Session.GetString("LoggedIn") == null)
            {
                return RedirectToAction("Login", "Login");
            }
            List<BookListModel> booklist = new List<BookListModel>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:44306");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync("http://localhost:44306/api/BookList/GetList");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    booklist = JsonConvert.DeserializeObject<List<BookListModel>>(jsonContent);
                    return View(booklist);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ViewBag.message = "There is no book available.";
                    return View();
                }
                else
                {
                    ViewBag.message = "No response from server";
                    return View();
                }
            }
        }

        [HttpGet]
        public IActionResult AddBook()
        {
            if (HttpContext.Session.GetString("LoggedIn") == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddBook(BookListViewModel model)
        {
            if (HttpContext.Session.GetString("LoggedIn") == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:44353");
            var jsonString = JsonConvert.SerializeObject(model);
            var message = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://localhost:44353/api/BookList/AddBook", message);
            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                ModelState.AddModelError("", "You cannot add more than one request");
                return View(model);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return RedirectToAction("GetList", "Book");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                ModelState.AddModelError("", "Something went wrong. Your Details have not been saved");
                return View(model);
            }
            else
            {
                ModelState.AddModelError("", "No Response from the server");
                return View(model);
            }

        }
    }
}

