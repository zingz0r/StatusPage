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
using Microsoft.EntityFrameworkCore;
using StatusCake.Client.Interfaces;

namespace StatusPage.HostedServices
{
    public class DatabaseHostedService : IHostedService, IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly IStatusCakeClient _statusCakeClient;
        private readonly StatusPageContext _context;

        private Timer _timer;

        public DatabaseHostedService(IStatusCakeClient statusCakeClient, IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            _statusCakeClient = statusCakeClient;
            _configuration = configuration;

            var scope = scopeFactory.CreateScope();

            _context = scope.ServiceProvider.GetRequiredService<StatusPageContext>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(InsertNewDataToDb, null, TimeSpan.Zero,
                TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private void InsertNewDataToDb(object state)
        {
            IEnumerable<Test> allTests = _statusCakeClient.GetTestsAsync().Result;

            foreach (var test in allTests)
            {
                // Insert test
                InsertTest(test);

                //// Insert Uptimes
                var availabilityData = _statusCakeClient.GetUptimesAsync(test.TestID).Result;
                InsertUptime(test, availabilityData);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
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

            var efTest = new Data.Entity.Test
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

            // remove today's data because it can change until midnight
            availabilityData.Remove(DateTime.Now.Date);

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

                if (_context.Availabilities.ContainsAsync(availabilityEntity).Result)
                    continue;

                _context.Add(availabilityEntity);
                _context.SaveChanges();
            }
        }
    }
}
