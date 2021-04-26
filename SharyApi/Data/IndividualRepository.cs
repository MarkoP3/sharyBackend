using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Configuration;
using SharyApi.Entities;
using SharyApi.Models.Individual;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharyApi.Data
{
    public class IndividualRepository : IIndividualRepository
    {
        public IndividualRepository(IMapper mapper, Shary2Context context, IConfiguration configuration)
        {
            Mapper = mapper;
            Context = context;
            Configuration = configuration;
        }

        public IMapper Mapper { get; }
        public Shary2Context Context { get; }
        public IConfiguration Configuration { get; }

        public IndividualConfirmationDto CreateIndividual(Individual individual)
        {
            individual.Id = Guid.NewGuid();
            var createindividual = Context.Add(individual);
            return Mapper.Map<IndividualConfirmationDto>(createindividual.Entity);
        }

        public void DeleteIndividual(Guid ID)
        {
            var individual = GetIndividualByID(ID);
            Context.Remove(individual);
        }

        public ICollection<Individual> GetAllIndividuals()
        {
            return Context.Individuals.ToList();
        }

        public ICollection<MoneyDonation> GetAllMoneyDonationsOfIndividual(Guid ID, int page)
        {
            return Context.Individuals.Find(ID).MoneyDonations.Skip((page - 1) * Convert.ToInt32(Configuration["recordsPerPage"])).Take(Convert.ToInt32(Configuration["recordsPerPage"])).ToList();
        }

        public Maybe<Individual> GetIndividualByID(Guid ID)
        {
            return Context.Individuals.Find(ID);
        }

        public Individual GetIndividualCredentialsByUsername(string username)
        {
            return Context.Individuals.Where(i => i.Username == username).FirstOrDefault();
        }

        public int GetNumberOfMoneyDonationsPages(Guid id)
        {
            return (int)Math.Ceiling((decimal)Context.Individuals.Find(id).MoneyDonations.Count / Convert.ToDecimal(Configuration["recordsPerPage"]));
        }

        public int GetNumberOfSolidarityDonationsPages(Guid id)
        {
            return (int)Math.Ceiling((decimal)Context.Individuals.Find(id).SolidarityDinnerDonations.Count / Convert.ToDecimal(Configuration["recordsPerPage"]));
        }

        public ICollection<SolidarityDinnerDonation> GetSolidarityDinners(Guid ID, int page)
        {
            return Context.Individuals.Find(ID).SolidarityDinnerDonations.Skip((page - 1) * Convert.ToInt32(Configuration["recordsPerPage"])).Take(Convert.ToInt32(Configuration["recordsPerPage"])).ToList(); ;
        }

        public bool SaveChanges()
        {
            return Context.SaveChanges() > 0;
        }
    }
}
