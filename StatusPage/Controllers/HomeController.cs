using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StatusCake.Client;
using StatusCake.Client.Models;
using StatusPage.Models;
using StatusPage.ViewModels;

namespace StatusPage.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {

            var statusCakeClient = new StatusCakeClient();

            IEnumerable<Test> allTests = await statusCakeClient.GetTestsAsync();

            IndexViewModel model = new IndexViewModel();

            foreach(var item in allTests)
            {
                Uri uriResult;
                bool isUri = false;

                try
                {
                    isUri = Uri.TryCreate(item.WebsiteName.StartsWith("http") ? item.WebsiteName : "https://" + item.WebsiteName, UriKind.Absolute, out uriResult)
                    && uriResult.Scheme == Uri.UriSchemeHttps || uriResult.Scheme == Uri.UriSchemeHttp;
                }
                catch
                {
                   //...
                }

                AdvancedTest advancedTest = new AdvancedTest(item);

                if (isUri)
                {
                    model.DomainTests.Add(advancedTest);
                }
                else
                {
                    model.ServiceTests.Add(advancedTest);
                }

            }


            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
