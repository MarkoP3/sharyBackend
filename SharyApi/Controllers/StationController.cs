using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SharyApi.Data;
using SharyApi.Entities;
using SharyApi.Models.Individual;
using SharyApi.Models.Station;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharyApi.Controllers
{
    [Route("api/station")]
    [ApiController]
    public class StationController : ControllerBase
    {
        public StationController(IStationRepository stationRepository, IMapper mapper, LinkGenerator linkGenerator)
        {
            StationRepository = stationRepository;
            Mapper = mapper;
            LinkGenerator = linkGenerator;
        }

        public IStationRepository StationRepository { get; }
        public IMapper Mapper { get; }
        public LinkGenerator LinkGenerator { get; }

        [HttpGet("{ID}")]
        public ActionResult<StationDto> GetStationByID(Guid ID)
        {
            var station = StationRepository.GetStationByID(ID);
            if (station == null)
                return NoContent();
            return Ok(Mapper.Map<StationDto>(station));
        }
        [HttpGet]
        public ActionResult<ICollection<StationDto>> GetStations(Guid ID)
        {
            var stations = StationRepository.GetStations();
            if (stations.Count > 0)
                return Mapper.Map<List<StationDto>>(stations);
            return NoContent();
        }
        [HttpGet("{ID}/shareMeal")]
        public ActionResult<SharedMealDto> ShareMeal(Guid ID)
        {
                var station = StationRepository.GetStationByID(ID);
                if( station == null || (station.ReceivedMeals.Sum(x=>x.Quantity)-station.SharedMeals.Count)<=0)
                    return BadRequest("No available meals for sharing");
                var sharedMeal = StationRepository.ShareMeal(ID);
                StationRepository.SaveChanges();
                string location = LinkGenerator.GetPathByAction("GetBusinessByID", "Business", new { ID = sharedMeal.Id });
                return Created(location, Mapper.Map<SharedMealDto>(sharedMeal));
        }
        [HttpGet("mealPrice")]
        public ActionResult<MealPriceDto> GetActiveMealPrice()
        {
            return Ok(Mapper.Map<MealPriceDto>(StationRepository.GetActiveMealPrice()));
        }

    }
}
