using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StatusPage.Data;
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
            var model = new IndexViewModel();

            var tests = _context.Tests.Select(x => x);
            foreach (var test in tests)
            {
                var uptimes = await _context.Uptimes.Where(x => x.TestID == test.Id).ToDictionaryAsync(t => t.Date, t => t.UptimePercent);
                var advancedTest = new AdvancedTest(test, uptimes);

                if (test.TestType == StatusCake.Client.Enumerators.TestType.Http)
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
