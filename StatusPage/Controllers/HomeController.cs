using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StatusCake.Client.Models;
using StatusPage.Interfaces;
using StatusPage.Models;
using StatusPage.ViewModels;

namespace StatusPage.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStatusCakePersistence _statusCakePersistence;

        public HomeController(IStatusCakePersistence statusCakePersistence)
        {
            _statusCakePersistence = statusCakePersistence;
        }
        public async Task<IActionResult> Index()
        {
            var tests = await _statusCakePersistence.ReadTestsAsync();
            var testsArray = tests as Test[] ?? tests.ToArray();

            var availabilities = await _statusCakePersistence.ReadAvailabilitiesAsync();

            var viewModel = new IndexViewModel();

            foreach (var test in testsArray)
            {
                if (!availabilities.ContainsKey(test.TestID))
                {
                    availabilities[test.TestID] = new SortedDictionary<DateTime, Availability>();
                }

                var lastElementIndex = availabilities[test.TestID].Count - 1;

                // update last date 
                availabilities[test.TestID].ElementAt(lastElementIndex).Value.Uptime = test.Uptime ?? 100;

                var last90Days = new SortedDictionary<DateTime, Availability>(availabilities
                    .FirstOrDefault(x => x.Key == test.TestID)
                    .Value.Where(x => x.Key >= DateTime.Now.Date.AddDays(-90))
                    .ToDictionary(x => x.Key, x => x.Value));

                var advancedTest = new AdvancedTest(test, last90Days);

                if (test.TestType == StatusCake.Client.Enumerators.TestType.Http)
                {
                    viewModel.DomainTests.Add(advancedTest);
                }
                else
                {
                    viewModel.ServiceTests.Add(advancedTest);
                }

            }

            // average uptime in last 7 days
            var lastMonthData = availabilities.SelectMany(x => x.Value)
                .Where(x => x.Key >= DateTime.Now.Date.AddMonths(-1))
                .GroupBy(y => y.Key);

            foreach (var data in lastMonthData)
            {
                var averageUptime = data.Select(x => x.Value).Average(y => y.Uptime);
                viewModel.LastMonthAverageUptime.Add(data.Key, averageUptime);
            }

            // append today's uptime to the average uptimes

            var todayAverage = testsArray.Average(x => x.Uptime);
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
