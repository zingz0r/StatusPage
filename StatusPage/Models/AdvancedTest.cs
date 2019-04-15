using StatusCake.Client.Enumerators;
using StatusCake.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using StatusCake.Client;

namespace StatusPage.Models
{
    public class AdvancedTest : Test
    {
        public bool IsDown { get; set; }
        public string StatusText { get; set; }

        public SortedDictionary<DateTime, Availability> Availabilities { get; set; }

        public AdvancedTest(Test other, SortedDictionary<DateTime, Availability> availabilities)
        {
            ContactGroup = null;
            ContactID = other.ContactID;
            IsDown = other.Status == TestStatus.Down;
            Paused = other.Paused;
            Status = other.Status;
            StatusText = other.Status == TestStatus.Up ? "Operational" : "Down";
            TestID = other.TestID;
            TestType = other.TestType;
            WebsiteName = other.WebsiteName;
            Uptime = other.Uptime;
            Availabilities = availabilities;
        }
    }
}
