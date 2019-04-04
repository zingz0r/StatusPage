using StatusCake.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatusPage.Models
{
    public class AdvancedTest : Test
    {
        public bool IsDown { get; set; }
        public AdvancedTest(Test other)
        {
            ContactGroup = other.ContactGroup;
            ContactID = other.ContactID;
            IsDown = other.Status == "Down";
            Paused = other.Paused;
            Status = other.Status == "Up" ? "Operational" : "Down";
            TestID = other.TestID;
            TestType = other.TestType;
            WebsiteName = other.WebsiteName;
            Uptime = other.Uptime;
        }
    }
}
