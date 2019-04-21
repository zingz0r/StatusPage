using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using StatusCake.Client.Interfaces;
using StatusPage.Api.Interfaces;

namespace StatusPage.Api.HostedServices
{

    internal class CakeUpdaterHostedService : IHostedService, IDisposable
    {
        private readonly IStatusCakeClient _statusCakeClient;
        private readonly ITestsModel _testsModel;
        private readonly IAvailabilityModel _availabilityModel;
        private Timer _timer;

        public CakeUpdaterHostedService(IStatusCakeClient statusCakeClient, ITestsModel testsModel, IAvailabilityModel availabilityModel)
        {
            _statusCakeClient = statusCakeClient;
            _testsModel = testsModel;
            _availabilityModel = availabilityModel;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(UpdateTestsModel, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(2));

            return Task.CompletedTask;
        }

        private void UpdateTestsModel(object state)
        {
            var tests = _statusCakeClient.GetTestsAsync().Result;

            _availabilityModel.Lock();

            foreach (var test in tests)
            {
                _testsModel.UpdateTest(test);
                var availabilities = _statusCakeClient.GetUptimesAsync(test.TestID).Result;

                _availabilityModel.UpdateAvailability(test.TestID, availabilities);
            }

            _availabilityModel.Unlock();
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
