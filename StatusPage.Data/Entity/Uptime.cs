using System;
using System.Collections.Generic;
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
    }
}
