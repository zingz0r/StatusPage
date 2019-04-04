using StatusPage.Models;
using System.Collections.Generic;

namespace StatusPage.ViewModels
{
    public class IndexViewModel
    {
        public List<AdvancedTest> DomainTests { get; set; }
        public List<AdvancedTest> ServiceTests { get; set; }
        public IndexViewModel()
        {
            DomainTests = new List<AdvancedTest>();
            ServiceTests = new List<AdvancedTest>();
        }
    }
}
