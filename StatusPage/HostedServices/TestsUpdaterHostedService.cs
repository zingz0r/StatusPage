using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StatusCake.Client.Interfaces;
using StatusPage.Interfaces;

namespace StatusPage.HostedServices
{

    internal class TestsUpdaterHostedService : IHostedService, IDisposable
    {
        private readonly IStatusCakeClient _statusCakeClient;
        private readonly ITestsModel _testsModel;
        private Timer _timer;

        public TestsUpdaterHostedService(IStatusCakeClient statusCakeClient, ITestsModel testsModel)
        {
            _statusCakeClient = statusCakeClient;
            _testsModel = testsModel;
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
            foreach (var test in tests)
            {
                _testsModel.UpdateTest(test);
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
