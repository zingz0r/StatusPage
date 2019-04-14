using System;
using System.Collections.Generic;
using StatusCake.Client.Models;
using StatusPage.Api.Interfaces;

namespace StatusPage.Api.Models
{
    public class AvailabilityModel : IAvailabilityModel
    {
        public Dictionary<int, SortedDictionary<DateTime, Availability>> Availabilities { get; }

        public AvailabilityModel()
        {
            Availabilities = new Dictionary<int, SortedDictionary<DateTime, Availability>>();
        }

        public Dictionary<int, SortedDictionary<DateTime, Availability>> GetAvailabilities()
        {
            return Availabilities;

        }

        public void UpdateAvailability(int testId, SortedDictionary<DateTime, Availability> availabilities)
        {
            Availabilities[testId] = availabilities;
        }
    }
}
