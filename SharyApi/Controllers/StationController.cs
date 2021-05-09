using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SharyApi.Data;
using SharyApi.Entities;
using SharyApi.Helpers;
using SharyApi.Models;
using SharyApi.Models.Station;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace SharyApi.Controllers
{
    [Route("api/station")]
    [ApiController]
    public class StationController : ControllerBase
    {
        public StationController(IAuthenticationHelper authenticationHelper, IStationRepository stationRepository, IMapper mapper, LinkGenerator linkGenerator)
        {
            AuthenticationHelper = authenticationHelper;
            StationRepository = stationRepository;
            Mapper = mapper;
            LinkGenerator = linkGenerator;
        }

        public IAuthenticationHelper AuthenticationHelper { get; }
        public IStationRepository StationRepository { get; }
        public IMapper Mapper { get; }
        public LinkGenerator LinkGenerator { get; }

        [HttpGet("{ID}")]
        public ActionResult<StationDto> GetStationByID(Guid ID)
        {
            var station = StationRepository.GetStationByID(ID);
            if (station == null)
                return NoContent();
            return Ok(Mapper.Map<StationDto>(station.Value));
        }
        [HttpGet]
        public ActionResult<ICollection<StationDto>> GetStations(Guid ID)
        {
            var stations = StationRepository.GetStations();
            if (stations.Count > 0)
                return Mapper.Map<List<StationDto>>(stations);
            return NoContent();
        }
        [Authorize]
        [HttpGet("shareMeal")]
        public ActionResult<SharedMealDto> ShareMeal()
        {
            Guid ID = Guid.Parse(new JwtSecurityToken(Request.Cookies["token"]).Claims.First(c => c.Type == "aud").Value);
            var station = StationRepository.GetStationByID(ID);
            if (station.HasNoValue || (station.Value.ReceivedMeals.Sum(x => x.Quantity) - station.Value.SharedMeals.Count) <= 0)
                return BadRequest("No available meals for sharing");
            var sharedMeal = StationRepository.ShareMeal(ID);
            StationRepository.SaveChanges();
            string location = LinkGenerator.GetPathByAction("GetBusinessByID", "Business", new { ID = sharedMeal.Id });
            return Created(location, Mapper.Map<SharedMealDto>(sharedMeal));
        }
        [Authorize]
        [HttpPost("receiveMeals")]
        public ActionResult<ReceivedMealDto> ReceiveMeals(ReceivedMealDto receivedMealDto)
        {
            Guid ID = Guid.Parse(new JwtSecurityToken(Request.Cookies["token"]).Claims.First(c => c.Type == "aud").Value);
            var station = StationRepository.GetStationByID(ID);
            if (station.HasNoValue)
                return BadRequest();
            receivedMealDto.StationId = ID;
            receivedMealDto.Id = Guid.NewGuid();
            var receivedMeal = StationRepository.ReceiveMeal(Mapper.Map<ReceivedMeal>(receivedMealDto));
            StationRepository.SaveChanges();
            return Ok(Mapper.Map<ReceivedMeal>(receivedMeal));
        }
        [HttpPost]
        public ActionResult<StationDto> CreateStation(StationCreationDto stationDto)
        {
            var station = Mapper.Map<Station>(stationDto);
            var hashedPassword = AuthenticationHelper.HashPassword(station.Password);
            station.Password = hashedPassword.Item1;
            station.Salt = hashedPassword.Item2;
            Station stationConfirmation = StationRepository.CreateStation(station);
            StationRepository.SaveChanges();
            return Created("", stationConfirmation.Id);
        }
        [HttpPost("authenticate")]
        public IActionResult Authenticate(Credentials credentials)
        {
            if (AuthenticationHelper.AuthenticateStation(credentials))
            {
                var station = StationRepository.GetStationByUsername(credentials.Username);
                if (station.HasNoValue)
                    return NotFound();
                var createdToken = AuthenticationHelper.GenerateJwt(Mapper.Map<Principal>(station.Value));
                Response.Cookies.Append("token", createdToken, new CookieOptions() { HttpOnly = true, IsEssential = true, Expires = DateTime.Now.AddDays(10) });
                return Ok(new { token = createdToken });
            }
            return Unauthorized();
        }

    }
}
