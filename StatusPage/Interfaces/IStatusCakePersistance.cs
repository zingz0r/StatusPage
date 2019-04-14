using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StatusCake.Client.Models;

namespace StatusPage.Interfaces
{
    public interface IStatusCakePersistence
    {
        Task<IDictionary<int, SortedDictionary<DateTime, Availability>>> ReadAvailabilitiesAsync();
        Task<IEnumerable<Test>> ReadTestsAsync();
    }
}
