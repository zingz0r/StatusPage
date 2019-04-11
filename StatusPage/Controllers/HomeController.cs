using System;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StatusCake.Client;
using StatusCake.Client.Interfaces;
using StatusPage.Data;
using StatusPage.Models;
using StatusPage.ViewModels;

namespace StatusPage.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStatusCakeClient _statusCakeClient;
        private readonly StatusPageContext _context;
        public HomeController(IStatusCakeClient statusCakeClient, StatusPageContext context)
        {
            _statusCakeClient = statusCakeClient;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var tests = await _statusCakeClient.GetTestsAsync();
            var viewModel = new IndexViewModel();

            foreach (var test in tests)
            {
                var uptimes = await _context.Availabilities.Where(x => x.TestID == test.TestID && x.Date.Date >= DateTime.Now.Date.AddDays(-90)).ToDictionaryAsync(t => t.Date, t => t.UptimePercent);

                // append today's uptime to the uptimes
                uptimes[DateTime.Now.Date] = test.Uptime ?? 0;

                var advancedTest = new AdvancedTest(test, uptimes);

                if (test.TestType == StatusCake.Client.Enumerators.TestType.Http)
                {
                    viewModel.DomainTests.Add(advancedTest);
                }
                else
                {
                    viewModel.ServiceTests.Add(advancedTest);
                }

            }

            //// average uptime in last 7 days
            var lastMonthData = await _context.Availabilities
                .Where(x => x.Date.Date >= DateTime.Now.Date.AddMonths(-1))
                .GroupBy(y => y.Date).ToDictionaryAsync(x => x.Key, x => x.ToList());

            foreach (var (key, value) in lastMonthData)
            {
                viewModel.LastMonthAverageUptime.Add(key, value.Average(x => x.UptimePercent));
            }

            // append today's uptime to the average uptimes

            var todayAverage = tests.Average(x => x.Uptime);
            viewModel.LastMonthAverageUptime[DateTime.Now.Date] = todayAverage ?? 0;

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
