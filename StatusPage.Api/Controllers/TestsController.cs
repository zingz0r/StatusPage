using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StatusCake.Client.Models;
using StatusPage.Api.Interfaces;

namespace StatusPage.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly ITestsModel _testsModel;

        public TestsController(ITestsModel testsModel)
        {
            _testsModel = testsModel;
        }

        // GET: api/Tests
        [HttpGet]
        public List<Test> Get()
        {
            return _testsModel.GetTests().Select(x => x.Value).ToList();
        }
    }
}
