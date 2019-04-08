using System;
using Newtonsoft.Json;
using StatusCake.Client.Enumerators;

namespace StatusPage.Data.Entity
{
    /// <summary>
    /// TODO
    /// </summary>
    public class Period
    {
        /// <summary>
        /// Id of the Period
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The start of the period
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// The end of the period. If this is 0000 00 00 it means the period is still ongoing.
        /// </summary>
        public DateTime End { get; set; }

        /// <summary>
        /// The status type
        /// </summary>
        public TestStatus Status { get; set; }

        /// <summary>
        /// Additional information
        /// </summary>
        public string Additional { get; set; }

        /// <summary>
        /// Period time in text
        /// </summary>
        [JsonProperty("Period")]
        public string PeriodText { get; set; }

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
