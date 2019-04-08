using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StatusPage.Data.Entity
{
    /// <summary>
    /// Uptime Class
    /// </summary>
    public class Uptime
    {
        /// <summary>
        /// Id of the Uptime
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Date for the uptime
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Uptime in precentage
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
