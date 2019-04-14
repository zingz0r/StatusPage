using System;
using System.Collections.Generic;
using StatusCake.Client.Models;

namespace StatusPage.Api.Interfaces
{
    public interface IAvailabilityModel
    {
        /// <summary>
        /// GetAvailabilities
        /// </summary>
        /// <list type="int">TestID</list>
        /// <returns>Return the availabilities data</returns>
        Dictionary<int, SortedDictionary<DateTime, Availability>> GetAvailabilities();
        void UpdateAvailability(int testId, SortedDictionary<DateTime, Availability> availabilities);
    }
}
