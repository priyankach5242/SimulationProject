using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using MvcClient.Models.ViewModel;
using Newtonsoft.Json;

namespace MvcClient.Controllers
{
    public class LoginController : Controller
    {

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:44356");
                var jsonString = JsonConvert.SerializeObject(model);
                var message = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:44356/api/Login/Login", message);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //LoggedIn = true;
                    HttpContext.Session.SetString("LoggedIn", "1");
                    return RedirectToAction("GetList", "BookList");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ModelState.AddModelError("", "Something Went Wrong");
                    return View(model);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ModelState.AddModelError("", "Invalid Username or Password");
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError("", "No response from Server");
                    return View(model);
                }
            }
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }
    }
}

