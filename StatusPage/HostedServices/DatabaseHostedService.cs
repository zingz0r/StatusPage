using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StatusCake.Client;
using StatusCake.Client.Models;
using StatusPage.Data;
using StatusPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StatusPage.HostedServices
{
    public class DatabaseHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly StatusCakeClient _statusCakeClient;
        private readonly StatusPageContext _context;

        private Timer _timer;

        public DatabaseHostedService(ILogger<DatabaseHostedService> logger, IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _configuration = configuration;

            _scopeFactory = scopeFactory;
            var scope = _scopeFactory.CreateScope();

            _context = scope.ServiceProvider.GetRequiredService<StatusPageContext>();

            string statusCakeUsername = _configuration.GetSection("StatusCake").GetValue<string>("Username");
            string statusCakeApiKey = _configuration.GetSection("StatusCake").GetValue<string>("ApiKey");

            _statusCakeClient = new StatusCakeClient(statusCakeUsername, statusCakeApiKey);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            _timer = new Timer(InsertNewDataToDB, null, TimeSpan.Zero,
                TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private void InsertNewDataToDB(object state)
        {
            _logger.LogInformation("Fetching tests from StatusCake API.");

            IEnumerable<Test> allTests = _statusCakeClient.GetTestsAsync().Result;

            foreach (var test in allTests)
            {
                _logger.LogInformation("Fetching uptimes data from StatusCake API.");

                // Insert test
                InsertTest(test);

                //// Insert Uptimes
                int daysToShow = _configuration.GetSection("StatusCake").GetValue<int>("DaysToShowOnMetrics");

                IDictionary<DateTime, double> uptimes;
                uptimes = _statusCakeClient.GetUptimesAsync(test.TestID).Result;

                InsertUptime(test, uptimes);

                _logger.LogInformation("Checking if there are new data.");

            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void InsertTest(Test test)
        {
            if (_context == null)
                return;

            if (test == null)
                return;

            Data.Entity.Test efTest = new Data.Entity.Test
            {
                Id = test.TestID,
                ContactID = test.ContactID,
                Paused = test.Paused,
                Status = test.Status,
                TestType = test.TestType,
                Uptime = test.Uptime,
                WebsiteName = test.WebsiteName
            };

            if (!_context.Tests.Contains(efTest))
            {
                _context.Add(efTest);
                _context.SaveChanges();
            }
        }
        private void InsertUptime(Test test, IDictionary<DateTime, double> uptimeData)
        {
            if (_context == null)
                return;

            if (test == null)
                return;

            if (uptimeData == null)
                return;

            foreach (var uptime in uptimeData)
            {
                Data.Entity.Uptime efuptime = new Data.Entity.Uptime
                {
                    Id = _context.Uptimes.Count() + 1,
                    TestID = test.TestID,
                    Date = uptime.Key,
                    UptimePercent = uptime.Value
                };

                if (!_context.Uptimes.Any(x => x.TestID == efuptime.TestID && x.Date == efuptime.Date) && efuptime.Date.Date != DateTime.Now.Date)
                {
                    _context.Add(efuptime);
                    _context.SaveChanges();
                }
            }
        }
    }
}
