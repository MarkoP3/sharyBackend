using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SharyApi.Data;
using SharyApi.Entities;
using SharyApi.Helpers;
using SharyApi.Models;
using SharyApi.Models.Individual;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace SharyApi.Controllers
{
    [Route("api/individual")]
    [ApiController]
    public class IndividualController : ControllerBase
    {
        public IndividualController(IIndividualRepository individualRepository, IMapper mapper, LinkGenerator linkGenerator, IAuthenticationHelper authenticationHelper, IStationRepository stationRepository)
        {
            IndividualRepository = individualRepository;
            Mapper = mapper;
            LinkGenerator = linkGenerator;
            AuthenticationHelper = authenticationHelper;
            StationRepository = stationRepository;
        }

        public IIndividualRepository IndividualRepository { get; }
        public IMapper Mapper { get; }
        public LinkGenerator LinkGenerator { get; }
        public IAuthenticationHelper AuthenticationHelper { get; }
        public IStationRepository StationRepository { get; }

        [HttpGet]
        public ActionResult<List<IndividualDto>> GetAllIndividuals()
        {
            var individuals = IndividualRepository.GetAllIndividuals();
            if (individuals.Count > 0)
            {
                return Ok(Mapper.Map<List<IndividualDto>>(individuals));
            }
            return NoContent();
        }
        [HttpGet("{ID}")]
        public ActionResult<IndividualDto> GetIndividualByID(Guid ID)
        {
            return (IndividualRepository.GetIndividualByID(ID).HasNoValue ? NotFound() : Ok(Mapper.Map<IndividualDto>(IndividualRepository.GetIndividualByID(ID).Value)));
        }
        [Authorize]
        [HttpGet("accountData")]
        [HttpHead("accountData")]
        public ActionResult<IndividualDto> GetAccountData()
        {
            var individualID = new JwtSecurityToken(Request.Cookies["token"]).Claims.First(c => c.Type == "aud").Value;
            var individual = IndividualRepository.GetIndividualByID(Guid.Parse(individualID));

            if (individual.HasNoValue)
                return NoContent();
            return Ok(Mapper.Map<IndividualDto>(individual.Value));
        }

        /*[HttpGet("{ID}/donations/money")]
        public ActionResult<MoneyDonationDto> GetMoneyDonationsOfIndividual(Guid ID)
        {
            if (IndividualRepository.GetIndividualByID(ID) != null)
            {
                var donations = IndividualRepository.GetAllMoneyDonationsOfIndividual(ID);
                if (donations.Count > 0)
                    return Ok(Mapper.Map<List<MoneyDonationDto>>(donations));
                return NoContent();
            }
            return BadRequest();
        }*/
        [Authorize]
        [HttpGet("logout")]
        public IActionResult LogOut()
        {
            Response.Cookies.Delete("token");
            return Ok();
        }
        [HttpPost]
        public ActionResult<IndividualConfirmationDto> CreateIndividual(IndividualCreationDto businessDto)
        {
            try
            {
                var individual = Mapper.Map<Individual>(businessDto);
                var hashedPassword = AuthenticationHelper.HashPassword(individual.Password);
                individual.Password = hashedPassword.Item1;
                individual.Salt = hashedPassword.Item2;
                IndividualConfirmationDto individualConfirmation = IndividualRepository.CreateIndividual(individual);
                IndividualRepository.SaveChanges();
                string location = LinkGenerator.GetPathByAction("GetIndividualByID", "Individual", new { ID = individualConfirmation.Id });
                return Created(location, individualConfirmation);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("authenticate")]
        public IActionResult Authenticate(Credentials credentials)
        {
            if (AuthenticationHelper.AuthenticateIndividual(credentials))
            {
                var createdToken = AuthenticationHelper.GenerateJwt(Mapper.Map<Principal>(IndividualRepository.GetIndividualCredentialsByUsername(credentials.Username)));
                Response.Cookies.Append("token", createdToken, new CookieOptions() { HttpOnly = true, IsEssential = true, Expires = DateTime.Now.AddDays(10) });
                return Ok(new { token = createdToken });
            }
            return Unauthorized();
        }
        [HttpDelete("{ID}")]
        public IActionResult DeleteIndividual(Guid ID)
        {
            try
            {
                var individual = IndividualRepository.GetIndividualByID(ID);
                if (individual == null)
                {
                    return NotFound();
                }
                IndividualRepository.DeleteIndividual(ID);
                IndividualRepository.SaveChanges();
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while deleting an object");
            }
        }
        [HttpPut]
        public ActionResult<IndividualDto> UpdateIndividual(IndividualUpdateDto individualDto)
        {
            try
            {
                var oldIndividual = IndividualRepository.GetIndividualByID(individualDto.Id);
                if (oldIndividual == null)
                    return NotFound();
                Individual individual = Mapper.Map<Individual>(individualDto);
                Mapper.Map(individual, oldIndividual);
                IndividualRepository.SaveChanges();
                return Ok(Mapper.Map<IndividualDto>(oldIndividual));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update error");
            }
        }
        [HttpGet("mealPrice")]
        public ActionResult<MealPriceDto> GetActiveMealPrice()
        {
            return Ok(Mapper.Map<MealPriceDto>(StationRepository.GetActiveMealPrice()));
        }


    }
}
