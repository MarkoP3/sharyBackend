using AutoMapper;
using SharyApi.Entities;
using SharyApi.Models.Individual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharyApi.Data
{
    public class IndividualRepository : IIndividualRepository
    {
        public IndividualRepository(IMapper mapper, Shary2Context context)
        {
            Mapper = mapper;
            Context = context;
        }

        public IMapper Mapper { get; }
        public Shary2Context Context { get; }

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

        public ICollection<MoneyDonation> GetAllMoneyDonationsOfIndividual(Guid ID)
        {
            return Context.Individuals.Find(ID).MoneyDonations;
        }

        public Individual GetIndividualByID(Guid ID)
        {
            return Context.Individuals.Find(ID);
        }

        public Individual GetIndividualCredentialsByUsername(string username)
        {
            return Context.Individuals.Where(i => i.Username == username).FirstOrDefault();
        }

        public ICollection<SolidarityDinnerDonation> GetSolidarityDinners(Guid ID)
        {
            return Context.Individuals.Find(ID).SolidarityDinnerDonations;
        }

        public bool SaveChanges()
        {
            return Context.SaveChanges() > 0;
        }
    }
}
