﻿using StatusCake.Client.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusPage.Data.Entity
{
    /// <summary>
    /// The details of a test
    /// </summary>
    public class TestDetails
    {
        /// <summary>
        /// Id of the TestDetails
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// True if the test is paused
        /// </summary>
        public bool Paused { get; set; }

        /// <summary>
        /// The type of the test. Possible values: HTTP or TCP
        /// </summary>
        public TestType TestType { get; set; }

        /// <summary>
        /// The name of the test
        /// </summary>
        public string WebsiteName { get; set; }

        /// <summary>
        /// Contains the contact group or null if the group is not set
        /// </summary>
        public string ContactGroup { get; set; }

        /// <summary>
        /// The contact ID the test is tied to
        /// </summary>
        public int ContactID { get; set; }

        /// <summary>
        /// Current status of the test. Possible values: Up, Down
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The uptime percentage for the last day
        /// </summary>
        public double Uptime { get; set; }

        /// <summary>
        /// The check rate in seconds
        /// </summary>
        public int CheckRate { get; set; }

        /// <summary>
        /// The test check timeout in seconds
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// The URL of the logo image for public reporting
        /// </summary>
        public string LogoImage { get; set; }

        /// <summary>
        /// User entered value for Website Host
        /// </summary>
        public string WebsiteHost { get; set; }

        /// <summary>
        /// The string the test will look for in the response
        /// </summary>
        public string FindString { get; set; }

        /// <summary>
        /// If a string should be found or not be ound in the result
        /// </summary>
        public string DoNotFind { get; set; }

        /// <summary>
        /// The last time the test was ran
        /// </summary>
        public DateTime LastTested { get; set; }

        /// <summary>
        /// The next location where the test will be ran from
        /// </summary>
        public string NextLocation { get; set; }

        /// <summary>
        /// True if the website is currently being tested
        /// </summary>
        public bool Processing { get; set; }

        /// <summary>
        /// The current test status
        /// </summary>
        public string ProcessingState { get; set; }

        /// <summary>
        /// The server that is processing the test
        /// </summary>
        public string ProcessingOn { get; set; }

        /// <summary>
        /// The amount of consecutive downtimes recorded for this test
        /// </summary>
        public int DownTimes { get; set; }

        /// <summary>
        /// The ID of the test that triggered the alert
        /// </summary>
        public int TestID { get; set; }

        /// <summary>
        /// Foreign key
        /// </summary>
        public Test Test { get; set; }
    }
}
