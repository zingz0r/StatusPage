using StatusCake.Client.Enumerators;
using System;

namespace StatusPage.Data.Entity
{
    /// <summary>
    /// Alert class
    /// </summary>
    public class Alert
    {
        /// <summary>
        /// Id of the Alert
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The date and time when the alert was sent in GMT.
        /// </summary>
        public DateTime Triggered { get; set; }

        /// <summary>
        /// The status code
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// The linux timestamp
        /// </summary>
        public long Unix { get; set; }

        /// <summary>
        /// The status logged that triggered the alert
        /// </summary>
        public TestStatus Status { get; set; }

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
