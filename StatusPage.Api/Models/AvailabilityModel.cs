using System;
using System.Collections.Generic;
using StatusCake.Client.Models;
using StatusPage.Api.Interfaces;

namespace StatusPage.Api.Models
{
    public class AvailabilityModel : IAvailabilityModel
    {
        private Dictionary<int, SortedDictionary<DateTime, Availability>> _availabilities;
        private readonly Dictionary<int, SortedDictionary<DateTime, Availability>> _updateAvailabilities;

        public bool IsLocked { get; private set; }

        public AvailabilityModel()
        {
            _availabilities = new Dictionary<int, SortedDictionary<DateTime, Availability>>();
            _updateAvailabilities = new Dictionary<int, SortedDictionary<DateTime, Availability>>();
        }

        public Dictionary<int, SortedDictionary<DateTime, Availability>> GetAvailabilities()
        {
            return _availabilities;
        }
        public void UpdateAvailability(int testId, SortedDictionary<DateTime, Availability> availabilities)
        {
            if(!IsLocked)
                throw new LockException("You have to lock the model before updating it!");

            _updateAvailabilities[testId] = availabilities;
        }

        public void Lock()
        {
            IsLocked = true;
        }

        public void Unlock()
        {
            IsLocked = false;
            _availabilities = _updateAvailabilities;
        }
    }
}
