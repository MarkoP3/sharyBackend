using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using SharyApi.Data;
using SharyApi.Entities;
using SharyApi.Models;
using SharyApi.Models.Business;
using SharyApi.Models.Individual;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using SessionCreateOptions = Stripe.Checkout.SessionCreateOptions;

namespace SharyApi.Controllers
{
    [Route("api/donate")]
    [ApiController]
    public class DonationController : ControllerBase
    {
        public DonationController(IConfiguration configuraiton, IStationRepository stationRepository, IMapper mapper, IIndividualRepository individualRepository, IDonationRepository donationRepository, IBusinessRepository businessRepository, LinkGenerator linkGenerator)
        {
            Configuraiton = configuraiton;
            StationRepository = stationRepository;
            Mapper = mapper;
            IndividualRepository = individualRepository;
            DonationRepository = donationRepository;
            BusinessRepository = businessRepository;
            LinkGenerator = linkGenerator;
            StripeConfiguration.ApiKey = Configuraiton["Stripe:Test:SecretKey"];

        }

        public IConfiguration Configuraiton { get; }
        public IStationRepository StationRepository { get; }
        public IMapper Mapper { get; }
        public IIndividualRepository IndividualRepository { get; }
        public IDonationRepository DonationRepository { get; }
        public IBusinessRepository BusinessRepository { get; }
        public LinkGenerator LinkGenerator { get; }

        [Authorize]
        [HttpPost("individual/money")]
        public IActionResult CreateCheckoutSession(MoneyDonationCreationDto moneyDonation)
        {
            Guid individualID;
            if (Request.Cookies["token"] != null)
            {
                individualID = Guid.Parse(new JwtSecurityToken(Request.Cookies["token"]).Claims.First(c => c.Type == "aud").Value);
            }
            else
            {
                return NoContent();
            }
            var mealPrice = Mapper.Map<MealPriceDto>(StationRepository.GetActiveMealPrice());
            var individual = IndividualRepository.GetIndividualByID(individualID);
            if (individual.HasNoValue)
                return NotFound();
            var options = new SessionCreateOptions
            {
                ClientReferenceId = individualID.ToString(),
                CustomerEmail = individual.Value.Email,
                SubmitType = "donate",
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long?)mealPrice.Price * 100,
                            Currency = mealPrice.Code,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Meal donation"
                            }

                        },
                        Quantity = moneyDonation.Quantity
                    },
                },
                Mode = "payment",
                SuccessUrl = "http://localhost:3000/individual/donation/money/success?individualID=" + individualID + "&quantity=" + moneyDonation.Quantity + "&mealPriceID=" + mealPrice.Id + "&sessionId={CHECKOUT_SESSION_ID}",
                CancelUrl = "http://localhost:3000/individual/donation/money/cancel/",
            };

            var service = new Stripe.Checkout.SessionService();
            Stripe.Checkout.Session session = service.Create(options);
            return Ok(new { id = session.Id });
        }
        [Authorize]
        [HttpPost("individual/solidarity")]
        public IActionResult CreateSolidarityCheckoutSession(SolidarityDonationCreationDto solidarityDonation)
        {
            Guid individualID;
            if (Request.Cookies["token"] != null)
            {
                individualID = Guid.Parse(new JwtSecurityToken(Request.Cookies["token"]).Claims.First(c => c.Type == "aud").Value);
            }
            else
            {
                return NoContent();
            }
            var mealPrice = BusinessRepository.GetMealPriceOfBusiness(solidarityDonation.BusinessId);
            var individual = IndividualRepository.GetIndividualByID(individualID);
            if (individual.HasNoValue)
                return NotFound();
            var options = new SessionCreateOptions
            {
                ClientReferenceId = individualID.ToString(),
                CustomerEmail = individual.Value.Email,
                SubmitType = "donate",
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long?)mealPrice.Price * 100,
                            Currency = mealPrice.Currency.Code,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Meal donation"
                            }

                        },
                        Quantity = solidarityDonation.Quantity
                    },
                },
                Mode = "payment",
                SuccessUrl = "http://localhost:3000/individual/donation/solidarity/success?individualID=" + individualID + "&quantity=" + solidarityDonation.Quantity + "&mealPriceID=" + mealPrice.Id + "&sessionId={CHECKOUT_SESSION_ID}&businessID=" + solidarityDonation.BusinessId,
                CancelUrl = "http://localhost:3000/individual/donation/solidarity/cancel/",
            };

            var service = new Stripe.Checkout.SessionService();
            Stripe.Checkout.Session session = service.Create(options);
            return Ok(new { id = session.Id });
        }
        [Authorize]
        [HttpPost("individual/money/confirmation")]
        public IActionResult ConfirmMoneyDonation(MoneyDonationConfirmationDto moneyDonationDto)
        {
            Guid individualID;
            if (Request.Cookies["token"] != null)
            {
                individualID = Guid.Parse(new JwtSecurityToken(Request.Cookies["token"]).Claims.First(c => c.Type == "aud").Value);
            }
            else
            {
                return NoContent();
            }
            Console.WriteLine(individualID);
            try
            {
                var service = new Stripe.Checkout.SessionService();
                var response = service.Get(
                  moneyDonationDto.StripeSessionID
                );
                if (individualID.ToString() == response.ClientReferenceId && response.PaymentStatus == "paid")
                {
                    var moneyDonation = Mapper.Map<MoneyDonation>(moneyDonationDto);
                    moneyDonation.Id = Guid.NewGuid();
                    moneyDonation.DonationDateTime = DateTime.Now;
                    moneyDonation.StripePaymentId = response.PaymentIntentId;
                    moneyDonation.IndividualId = individualID;
                    DonationRepository.CreateMoneyDonation(moneyDonation);
                    Console.WriteLine("tu sam");
                    DonationRepository.SaveChanges();
                    Console.WriteLine("eto me opet tu");
                    return Ok();
                }
                return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }
        [Authorize]
        [HttpPost("individual/solidarity/confirmation")]
        public IActionResult ConfirmSolidarityDonation(SolidarityDonationConfirmationDto solidarityDonationDto)
        {
            try
            {
                var service = new Stripe.Checkout.SessionService();
                var response = service.Get(
                  solidarityDonationDto.StripeSessionID
                );
                if (solidarityDonationDto.IndividualID.ToString() == response.ClientReferenceId && response.PaymentStatus == "paid")
                {
                    Console.WriteLine(response.PaymentStatus);
                    var solidarityDonation = Mapper.Map<SolidarityDinnerDonation>(solidarityDonationDto);
                    solidarityDonation.Id = Guid.NewGuid();
                    solidarityDonation.DonationDateTime = DateTime.Now;
                    solidarityDonation.StripePaymentId = response.PaymentIntentId;
                    DonationRepository.CreateSolidarityDonation(solidarityDonation);
                    DonationRepository.SaveChanges();
                    return Ok();
                }
                return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }
        [Authorize]
        [HttpGet("individual/money")]
        public ActionResult<MoneyDonationDto> GetYourMoneyDonations(int? page)
        {
            if (page == null || page < 0)
                page = 1;
            var individualID = Guid.Parse(new JwtSecurityToken(Request.Cookies["token"]).Claims.First(c => c.Type == "aud").Value);
            if (IndividualRepository.GetIndividualByID(individualID).HasNoValue)
                return BadRequest();
            var donations = IndividualRepository.GetAllMoneyDonationsOfIndividual(individualID, (int)page);
            if (donations.Count > 0)
                return Ok(new { numOfPages = IndividualRepository.GetNumberOfMoneyDonationsPages(individualID), moneyDonations = Mapper.Map<List<MoneyDonationDto>>(donations) });
            return NoContent();
        }
        [Authorize]
        [HttpGet("individual/solidarity")]
        public ActionResult<SolidarityDinnerDonationDto> GetSolidarityDonations(int? page)
        {
            if (page == null || page < 1)
                page = 1;
            var individualID = Guid.Parse(new JwtSecurityToken(Request.Cookies["token"]).Claims.First(c => c.Type == "aud").Value);
            if (IndividualRepository.GetIndividualByID(individualID).HasNoValue)
                return BadRequest();

            var donations = IndividualRepository.GetSolidarityDinners(individualID, (int)page);
            if (donations.Count > 0)
                return Ok(new { numOfPages = IndividualRepository.GetNumberOfSolidarityDonationsPages(individualID), solidarityDonations = Mapper.Map<List<SolidarityDinnerDonationDto>>(donations) });
            return NoContent();
        }
        [Authorize]
        [HttpPost("business/food")]
        public IActionResult DonateMeals(FoodDonationCreationDto donation)
        {
            var businessID = Guid.Parse(new JwtSecurityToken(Request.Cookies["token"]).Claims.First(c => c.Type == "aud").Value);
            FoodDonation newDonation = DonationRepository.CreateFoodDonation(new FoodDonation() { BusinessId = businessID, DonationDateTime = DateTime.Now, Id = Guid.NewGuid(), Quantity = donation.Quantity, StationId = donation.StationId });
            BusinessRepository.SaveChanges();
            return Created("", Mapper.Map<FoodDonationDto>(newDonation));

        }
        [Authorize]
        [HttpPost("business/solidarity")]
        public IActionResult BusinessSolidarityDonation(SolidarityDonationCreationDto donation)
        {
            var businessID = Guid.Parse(new JwtSecurityToken(Request.Cookies["token"]).Claims.First(c => c.Type == "aud").Value);
            SolidarityDinnerDonation newDonation = DonationRepository.CreateSolidarityDonation(new SolidarityDinnerDonation() { BusinessId = businessID, DonationDateTime = DateTime.Now, Id = Guid.NewGuid(), IndividualId = null, Quantity = donation.Quantity, StripePaymentId = null });
            BusinessRepository.SaveChanges();
            return Created("", Mapper.Map<SolidarityDinnerDonationDto>(newDonation));
        }
    }
}
