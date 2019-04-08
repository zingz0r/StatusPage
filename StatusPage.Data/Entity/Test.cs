using StatusCake.Client.Enumerators;
using System.Collections.Generic;

namespace StatusPage.Data.Entity
{
    /// <summary>
    /// StatusCake test details
    /// </summary>
    public class Test
    {
        /// <summary>
        /// Id of the Test
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
        /// The contact ID the test is tied to
        /// </summary>
        public int ContactID { get; set; }

        /// <summary>
        /// Current status of the test. Possible values: Up, Down
        /// </summary>
        public TestStatus Status { get; set; }

        /// <summary>
        /// The uptime percentage for the last 7 days
        /// </summary>
        public double? Uptime { get; set; }
    }
}
