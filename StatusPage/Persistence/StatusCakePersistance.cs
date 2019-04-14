using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using StatusCake.Client.Models;
using StatusPage.Interfaces;

namespace StatusPage.Persistence
{
    public class StatusCakePersistence : IStatusCakePersistence
    {
        private readonly HttpClient _client;

        public StatusCakePersistence(string baseAddress)
        {
            _client = new HttpClient { BaseAddress = new Uri(baseAddress) };
        }
        public async Task<IDictionary<int, SortedDictionary<DateTime, Availability>>> ReadAvailabilitiesAsync()
        {
            try
            {
                var response = await _client.GetAsync("api/availabilities/");
                if (response.IsSuccessStatusCode)
                {
                    // Microsoft.Extensions.Identity.Stores - has to be installed through nuget
                    var availabilities = await response.Content.ReadAsAsync<IDictionary<int, SortedDictionary<DateTime, Availability>>>();

                    return availabilities;
                }
                else
                {
                    throw new PersistenceUnavailableException("Service returned response: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);

            }
        }

        public async Task<IEnumerable<Test>> ReadTestsAsync()
        {
            try
            {
                var response = await _client.GetAsync("api/tests/");
                if (response.IsSuccessStatusCode)
                {
                    // Microsoft.Extensions.Identity.Stores - has to be installed through nuget
                    var tests = await response.Content.ReadAsAsync<IEnumerable<Test>>();

                    return tests;
                }
                else
                {
                    throw new PersistenceUnavailableException("Service returned response: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);

            }
        }
    }
}
