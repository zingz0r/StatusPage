using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StatusPage.Data.Entity
{
    /// <summary>
    /// Availability Class
    /// </summary>
    public class Availability
    {
        /// <summary>
        /// Id of the Availability
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Date for the uptime
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Downtime in percent
        /// </summary>
        public double DowntimePercent { get; set; }

        /// <summary>
        /// Availability in percent
        /// </summary>
        public double UptimePercent { get; set; }

        /// <summary>
        /// The ID of the test that has this uptime
        /// </summary>
        public int TestID { get; set; }

        /// <summary>
        /// Foreign key
        /// </summary>
        public Test Test { get; set; }
    }
}
