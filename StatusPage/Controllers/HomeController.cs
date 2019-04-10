using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StatusPage.Data;
using StatusPage.Data.Entity;
using StatusPage.Models;
using StatusPage.ViewModels;

namespace StatusPage.Controllers
{
    public class HomeController : Controller
    {
        private readonly StatusPageContext _context;
        public HomeController(StatusPageContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var viewModel = new IndexViewModel();

            var tests = _context.Tests.Select(x => x);

            foreach (var test in tests)
            {
                var uptimes = await _context.Availabilities.Where(x => x.TestID == test.Id && x.Date.Date >= DateTime.Now.Date.AddDays(-90)).ToDictionaryAsync(t => t.Date, t => t.UptimePercent);
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
            var last7DaysData = await _context.Availabilities
                .Where(x => x.Date.Date >= DateTime.Now.Date.AddMonths(-1))
                .GroupBy(y => y.Date).ToDictionaryAsync(x=>x.Key, x=>x.ToList());
            
            foreach (var item in last7DaysData)
            {
                viewModel.LastMonthAverageUptime.Add(item.Key, item.Value.Average(x => x.UptimePercent));
            }

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
