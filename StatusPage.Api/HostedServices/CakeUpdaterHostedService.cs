using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StatusCake.Client;
using StatusCake.Client.Interfaces;
using StatusPage.Api.Config;
using StatusPage.Api.Interfaces;

namespace StatusPage.Api.HostedServices
{

    internal class CakeUpdaterHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<CakeUpdaterHostedService> _logger;
        private readonly ITestsModel _testsModel;
        private readonly IAvailabilityModel _availabilityModel;
        private readonly StatusCakeConfig _config;
        private Timer _timer;

        public CakeUpdaterHostedService(ILogger<CakeUpdaterHostedService> logger, ITestsModel testsModel, IAvailabilityModel availabilityModel, StatusCakeConfig config)
        {
            _logger = logger;
            _testsModel = testsModel;
            _availabilityModel = availabilityModel;
            _config = config;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(UpdateTestsModel, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(2));

            return Task.CompletedTask;
        }

        private void UpdateTestsModel(object state)
        {
            try
            {
                using (var statusCakeClient = new StatusCakeClient(_config.UserName, _config.ApiKey))
                {
                    _logger.LogInformation("Started to update availabilities");
                    var watch = new Stopwatch();
                    watch.Start();
                    var tests = statusCakeClient.GetTestsAsync().Result;

                    _availabilityModel.Lock();

                    foreach (var test in tests)
                    {
                        _testsModel.UpdateTest(test);
                        var availabilities = statusCakeClient.GetUptimesAsync(test.TestID).Result;

                        _availabilityModel.UpdateAvailability(test.TestID, availabilities);
                    }

                    _availabilityModel.Unlock();
                    watch.Stop();
                    _logger.LogInformation($"Finished to update availabilities. It took {watch.Elapsed.TotalMilliseconds} ms.");

                }
            }
            catch
            {
                _logger.LogError("No internet connection?");
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
    }
}
