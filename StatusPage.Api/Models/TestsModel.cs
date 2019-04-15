using System.Collections.Generic;
using StatusCake.Client.Models;
using StatusPage.Api.Interfaces;

namespace StatusPage.Api.Models
{
    public class TestsModel : ITestsModel
    {
        private readonly SortedList<int, Test> _tests;

        public TestsModel()
        {
            _tests = new SortedList<int, Test>();
        }

        public SortedList<int, Test> GetTests()
        {
            return _tests;
        }

        public void UpdateTest(Test test)
        {
            _tests[test.TestID] = test;
        }
    }
}
