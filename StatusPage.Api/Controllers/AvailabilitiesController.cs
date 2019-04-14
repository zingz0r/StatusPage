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
    public class AvailabilitiesController : ControllerBase
    {
        private readonly IAvailabilityModel _availabilityModel;

        public AvailabilitiesController(IAvailabilityModel availabilityModel)
        {
            _availabilityModel = availabilityModel;
        }
        // GET: api/StatusCake
        [HttpGet]
        public Dictionary<int, SortedDictionary<DateTime,Availability>> Get()
        {
            return _availabilityModel.GetAvailabilities();
        }

        // GET: api/StatusCake/5
        [HttpGet("{testId}", Name = "Get")]
        public IEnumerable<KeyValuePair<int, SortedDictionary<DateTime, Availability>>> Get(int testId)
        {
            return _availabilityModel.GetAvailabilities().Where(x=>x.Key == testId);
        }
    }
}
