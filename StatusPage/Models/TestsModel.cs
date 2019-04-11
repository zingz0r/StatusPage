using System;
using System.Collections.Generic;
using System.Linq;
using StatusCake.Client.Models;
using StatusPage.Interfaces;

namespace StatusPage.Models
{
    public class TestsModel : ITestsModel
    {
        public SortedList<double, Test> Tests { get; }

        public TestsModel()
        {
            Tests = new SortedList<double, Test>();
        }

        public SortedList<double, Test> GetTests()
        {
            return Tests;
        }

        public void UpdateTest(Test test)
        {
            Tests[test.TestID] = test;
        }
    }
}
