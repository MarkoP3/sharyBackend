using CSharpFunctionalExtensions;
using SharyApi.Entities;
using SharyApi.Models.Individual;
using System;
using System.Collections.Generic;

namespace SharyApi.Data
{
    public interface IIndividualRepository
    {
        public ICollection<Individual> GetAllIndividuals();
        public Maybe<Individual> GetIndividualByID(Guid ID);
        public Individual GetIndividualCredentialsByUsername(string username);
        public void DeleteIndividual(Guid ID);
        public IndividualConfirmationDto CreateIndividual(Individual individual);
        public ICollection<MoneyDonation> GetAllMoneyDonationsOfIndividual(Guid ID, int page);
        public int GetNumberOfMoneyDonationsPages(Guid id);
        public int GetNumberOfSolidarityDonationsPages(Guid id);

        public ICollection<SolidarityDinnerDonation> GetSolidarityDinners(Guid ID, int page);
        public bool SaveChanges();
    }
}
