using Newtonsoft.Json;
using System;

namespace StatusPage.Data.Entity
{
    /// <summary>
    /// Contains data from a check result
    /// </summary>
    public class CheckResult
    {
        /// <summary>
        /// Id of the CheckResult
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The status code of the check result
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// The UNIX timestamp time of the check result
        /// </summary>
        public long Time { get; set; }

        /// <summary>
        /// Human readable timestamp. Converted to the timezone of the account
        /// </summary>
        public DateTime Human { get; set; }

        /// <summary>
        /// The location from where the check was performed from
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// The response headers from the test taken
        /// </summary>
        public string Headers { get; set; }

        /// <summary>
        /// The execution time of the test in ms 
        /// </summary>
        public string Performance { get; set; }

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
