using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StatusCake.Client.Models;

namespace StatusPage.Interfaces
{
    public interface ITestsModel
    {
        SortedList<double, Test> GetTests();
        void UpdateTest(Test test);
    }
}
