using System;
using StatusPage.Models;
using System.Collections.Generic;

namespace StatusPage.ViewModels
{
    public class IndexViewModel
    {
        public List<AdvancedTest> DomainTests { get; set; }
        public List<AdvancedTest> ServiceTests { get; set; }
        public SortedDictionary<DateTime, double> LastMonthAverageUptime { get; set; }
        public IndexViewModel()
        {
            DomainTests = new List<AdvancedTest>();
            ServiceTests = new List<AdvancedTest>();
            LastMonthAverageUptime = new SortedDictionary<DateTime, double>();
        }
    }
}
