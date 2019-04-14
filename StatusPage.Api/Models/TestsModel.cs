using System.Collections.Generic;
using StatusCake.Client.Models;
using StatusPage.Api.Interfaces;

namespace StatusPage.Api.Models
{
    public class TestsModel : ITestsModel
    {
        public SortedList<int, Test> Tests { get; }

        public TestsModel()
        {
            Tests = new SortedList<int, Test>();
        }

        public SortedList<int, Test> GetTests()
        {
            return Tests;
        }

        public void UpdateTest(Test test)
        {
            Tests[test.TestID] = test;
        }
    }
}
