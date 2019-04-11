using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StatusCake.Client;
using StatusCake.Client.Models;
using StatusPage.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StatusCake.Client.Interfaces;

namespace StatusPage.HostedServices
{
    public class DatabaseHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IStatusCakeClient _statusCakeClient;
        private readonly StatusPageContext _context;

        private Timer _timer;

        public DatabaseHostedService(ILogger<DatabaseHostedService> logger, IStatusCakeClient statusCakeClient, IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _statusCakeClient = statusCakeClient;

            var scope = scopeFactory.CreateScope();

            _context = scope.ServiceProvider.GetRequiredService<StatusPageContext>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            _timer = new Timer(InsertNewDataToDb, null, TimeSpan.Zero,
                TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private void InsertNewDataToDb(object state)
        {
            _logger.LogInformation("Fetching tests from StatusCake API.");

            IEnumerable<Test> allTests = _statusCakeClient.GetTestsAsync().Result;

            foreach (var test in allTests)
            {
                _logger.LogInformation("Fetching uptimes data from StatusCake API.");

                // Insert test
                InsertTest(test);

                //// Insert Uptimes
                var uptimes = _statusCakeClient.GetUptimesAsync(test.TestID).Result;

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
        private void InsertUptime(Test test, SortedDictionary<DateTime, Availability> availabilityData)
        {
            if (_context == null)
                return;

            if (test == null)
                return;

            if (availabilityData == null)
                return;

            foreach (var (date, availability) in availabilityData)
            {
                var availabilityEntity = new Data.Entity.Availability
                {
                    Id = _context.Availabilities.Count() + 1,
                    TestID = test.TestID,
                    Date = date,
                    DowntimePercent = availability.Downtime,
                    UptimePercent = availability.Uptime
                };

                if (!_context.Availabilities.Any(x => x.TestID == availabilityEntity.TestID && x.Date == availabilityEntity.Date) && availabilityEntity.Date.Date < DateTime.Now.Date)
                {
                    _context.Add(availabilityEntity);
                    _context.SaveChanges();
                }
            }
        }
    }
}
