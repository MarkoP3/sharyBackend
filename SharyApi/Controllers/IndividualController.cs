using AutoMapper;
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
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace SharyApi.Controllers
{
    [Route("api/individual")]
    [ApiController]
    public class IndividualController : ControllerBase
    {
        public IndividualController(IIndividualRepository individualRepository, IMapper mapper, LinkGenerator linkGenerator, IAuthenticationHelper authenticationHelper)
        {
            IndividualRepository = individualRepository;
            Mapper = mapper;
            LinkGenerator = linkGenerator;
            AuthenticationHelper = authenticationHelper;
        }

        public IIndividualRepository IndividualRepository { get; }
        public IMapper Mapper { get; }
        public LinkGenerator LinkGenerator { get; }
        public IAuthenticationHelper AuthenticationHelper { get; }


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
            var individual = IndividualRepository.GetIndividualByID(ID);
            if (individual == null)
                return NoContent();
            return Ok(Mapper.Map<IndividualDto>(individual));
        }
        [HttpGet("getAccountData")]
        public ActionResult<IndividualDto> GetAccountData()
        {
            var individualID="";
            if (Request.Cookies["token"] != null)
            {
                 individualID = new JwtSecurityToken(Request.Cookies["token"]).Claims.First(c => c.Type == "aud").Value;
            }
            else {
                return NoContent();
            }
            var individual = IndividualRepository.GetIndividualByID(Guid.Parse(individualID));

            if (individual == null)
                return NoContent();
            return Ok(Mapper.Map<IndividualDto>(individual));
        }
        [HttpGet("donations/money")]
        public ActionResult<MoneyDonationDto> GetYourMoneyDonations()
        {
            var individualID = Guid.Parse(new JwtSecurityToken(Request.Cookies["token"]).Claims.First(c => c.Type == "aud").Value);
            if (IndividualRepository.GetIndividualByID(individualID) != null)
            {
                var donations = IndividualRepository.GetAllMoneyDonationsOfIndividual(individualID);
                if (donations.Count > 0)
                    return Ok(Mapper.Map<List<MoneyDonationDto>>(donations));
                return NoContent();
            }
            return BadRequest();
        }
        [HttpGet("donations/solidarity")]
        public ActionResult<SolidarityDinnerDonationDto> GetSolidarityDonations()
        {
            var individualID = Guid.Parse(new JwtSecurityToken(Request.Cookies["token"]).Claims.First(c => c.Type == "aud").Value);
            if (IndividualRepository.GetIndividualByID(individualID) != null)
            {
                var donations = IndividualRepository.GetSolidarityDinners(individualID);
                if (donations.Count > 0)
                    return Ok(Mapper.Map<List<SolidarityDinnerDonationDto>>(donations));
                return NoContent();
            }
            return BadRequest();
        }
        [HttpGet("{ID}/donations/money")]
        public ActionResult<MoneyDonationDto> GetMoneyDonationsOfIndividual(Guid ID)
        {
            if(IndividualRepository.GetIndividualByID(ID)!=null)
            {
                var donations = IndividualRepository.GetAllMoneyDonationsOfIndividual(ID);
                if (donations.Count > 0)
                    return Ok(Mapper.Map<List<MoneyDonationDto>>(donations));
                return NoContent();
            }
            return BadRequest();
        }
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
                Response.Cookies.Append("token", createdToken, new CookieOptions() { HttpOnly =true,IsEssential=true,Expires=DateTime.Now.AddDays(10) }) ;
                return Ok(new { token =createdToken });
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


    }
}
