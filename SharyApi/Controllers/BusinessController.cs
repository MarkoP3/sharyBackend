using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SharyApi.Data;
using SharyApi.Entities;
using SharyApi.Helpers;
using SharyApi.Models;
using SharyApi.Models.Business;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace SharyApi.Controllers
{
    [Route("api/business")]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        public BusinessController(IBusinessRepository businessRepository, IMapper mapper, LinkGenerator linkGenerator, IAuthenticationHelper authenticationHelper)
        {
            BusinessRepository = businessRepository;
            Mapper = mapper;
            LinkGenerator = linkGenerator;
            AuthenticationHelper = authenticationHelper;
        }

        public IBusinessRepository BusinessRepository { get; }
        public IMapper Mapper { get; }
        public LinkGenerator LinkGenerator { get; }
        public IAuthenticationHelper AuthenticationHelper { get; }

        [HttpGet]
        public ActionResult<List<BusinessDto>> GetAllBusinesses()
        {
            var businesses = BusinessRepository.GetAllBusinesses();
            if (businesses.Count > 0)
            {
                return Ok(Mapper.Map<List<BusinessDto>>(businesses));
            }
            return NoContent();
        }
        [HttpGet("{ID}")]
        public ActionResult<BusinessDto> GetBusinessByID(Guid ID) => BusinessRepository.GetBusinessByID(ID).Match<ActionResult<BusinessDto>>(None: () => NoContent(), Some: (business) => Ok(Mapper.Map<BusinessDto>(business)));
        [Authorize]
        [HttpGet("accountData")]
        [HttpHead("accountData")]
        public ActionResult<BusinessDto> GetAccountData()
        {
            var businessID = "";
            if (Request.Cookies["token"] != null)
            {
                businessID = new JwtSecurityToken(Request.Cookies["token"]).Claims.First(c => c.Type == "aud").Value;
            }
            else
            {
                return Unauthorized();
            }
            var business = BusinessRepository.GetBusinessByID(Guid.Parse(businessID));
            if (business == null)
                return NoContent();
            return Ok(Mapper.Map<BusinessDto>(business));
        }
        [HttpGet("logout")]
        public IActionResult LogOut()
        {
            Response.Cookies.Delete("token");
            return Ok();
        }
        [HttpPost]
        public ActionResult<BusinessConfirmationDto> CreateBusiness(BusinessCreationDto businessDto)
        {
            try
            {
                var business = Mapper.Map<Business>(businessDto);
                var hashedPassword = AuthenticationHelper.HashPassword(business.Password);
                business.Password = hashedPassword.Item1;
                business.Salt = hashedPassword.Item2;
                Business businessConfirmation = BusinessRepository.CreateBusiness(business);
                BusinessRepository.SaveChanges();
                string location = LinkGenerator.GetPathByAction("GetBusinessByID", "Business", new { ID = businessConfirmation.Id });
                return Created(location, Mapper.Map<BusinessConfirmationDto>(businessConfirmation));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("authenticate")]
        public IActionResult Authenticate(Credentials credentials)
        {
            if (AuthenticationHelper.AuthenticateBusiness(credentials))
            {
                var createdToken = AuthenticationHelper.GenerateJwt(Mapper.Map<Principal>(BusinessRepository.GetBusinessCredentialsByUsername(credentials.Username)));
                Response.Cookies.Append("token", createdToken, new CookieOptions() { HttpOnly = true });
                return Ok(new { token = createdToken });
            }
            return Unauthorized();
        }
        [HttpDelete("{ID}")]
        public IActionResult DeleteBusiness(Guid ID)
        {
            return BusinessRepository.GetBusinessByID(ID).Match<IActionResult>(None: () => NotFound(), Some: (data) =>
            {
                BusinessRepository.DeleteBusiness(ID);
                BusinessRepository.SaveChanges();
                return NoContent();
            });
        }
        [HttpPut]
        public ActionResult<BusinessDto> UpdateBusiness(BusinessUpdateDto businessDto)
        {

            return BusinessRepository.GetBusinessByID(businessDto.Id).Match<ActionResult<BusinessDto>>(None: () => NotFound(), Some: (data) =>
                 {
                     Business oldBusiness = data;
                     Console.WriteLine(data.GetType());
                     Business business = Mapper.Map<Business>(businessDto);

                     Mapper.Map(business, oldBusiness);
                     BusinessRepository.SaveChanges();
                     return Ok(Mapper.Map<BusinessDto>(oldBusiness));
                 });

        }

        [HttpGet("solidarity/countries")]
        public ActionResult<ICollection<CountryDto>> GetCountriesWithSolidarityDinners()
        {
            return Ok(Mapper.Map<ICollection<CountryDto>>(BusinessRepository.GetCountriesSolidarityDinner()));
        }
        [HttpGet("solidarity/cities")]
        public ActionResult<ICollection<CityDto>> GetCitiesWithSolidarityDinners(Guid country)
        {
            return Ok(Mapper.Map<ICollection<CityDto>>(BusinessRepository.GetCitiesOfSolidarityBusinesses(country)));
        }
        [HttpGet("solidarity")]
        public ActionResult<ICollection<BusinessDto>> GetSolidarityBusinesses(Guid? city)
        {
            if (city == null)
            {
                return Ok(Mapper.Map<ICollection<BusinessDto>>(BusinessRepository.GetAllBusinessWithSolidarityDinners()));
            }
            return Ok(Mapper.Map<ICollection<BusinessDto>>(BusinessRepository.GetBusinessWithSolidarityDinners((Guid)city)));
        }
        [HttpGet("{id}/mealPrice")]
        public ActionResult<SolidarityMealPriceDto> GetSolidarityMealPriceOfBusiness(Guid id)
        {
            return Mapper.Map<SolidarityMealPriceDto>(BusinessRepository.GetMealPriceOfBusiness(id));
        }

    }
}
