using StatusCake.Client.Enumerators;
using StatusCake.Client.Models;
using System;
using System.Collections.Generic;
using StatusCake.Client;

namespace StatusPage.Models
{
    public class AdvancedTest : Test
    {
        public bool IsDown { get; set; }
        public string StatusText { get; set; }

        public Dictionary<DateTime, double> UptimeDictionary { get; set; }

        public AdvancedTest(Test other, Dictionary<DateTime, double> uptimeDictionary)
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
            UptimeDictionary = uptimeDictionary;
        }
    }
}
