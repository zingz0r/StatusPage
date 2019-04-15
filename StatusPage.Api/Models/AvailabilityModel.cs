using System;
using System.Collections.Generic;
using StatusCake.Client.Models;
using StatusPage.Api.Interfaces;

namespace StatusPage.Api.Models
{
    public class AvailabilityModel : IAvailabilityModel
    {
        private readonly Dictionary<int, SortedDictionary<DateTime, Availability>> _availabilities;

        public AvailabilityModel()
        {
            _availabilities = new Dictionary<int, SortedDictionary<DateTime, Availability>>();
        }

        public Dictionary<int, SortedDictionary<DateTime, Availability>> GetAvailabilities()
        {
            return _availabilities;
        }
        public void UpdateAvailability(int testId, SortedDictionary<DateTime, Availability> availabilities)
        {
            _availabilities[testId] = availabilities;
        }
    }
}
