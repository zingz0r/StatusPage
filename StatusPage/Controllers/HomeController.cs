using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StatusCake.Client;
using StatusPage.Models;

namespace StatusPage.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {

            var statusCakeClient = new StatusCakeClient();

            return View(await statusCakeClient.GetTestsAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
