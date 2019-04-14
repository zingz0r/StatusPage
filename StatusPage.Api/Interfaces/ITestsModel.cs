using System.Collections.Generic;
using StatusCake.Client.Models;

namespace StatusPage.Api.Interfaces
{
    public interface ITestsModel
    {
        SortedList<int, Test> GetTests();
        void UpdateTest(Test test);
    }
}
