using SharyApi.Entities;
using SharyApi.Models.Individual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharyApi.Data
{
    public interface IIndividualRepository
    {
        public ICollection<Individual> GetAllIndividuals();
        public Individual GetIndividualByID(Guid ID);
        public Individual GetIndividualCredentialsByUsername(string username);
        public void DeleteIndividual(Guid ID);
        public IndividualConfirmationDto CreateIndividual(Individual individual);
        public ICollection<MoneyDonation> GetAllMoneyDonationsOfIndividual(Guid ID);
        public ICollection<SolidarityDinnerDonation> GetSolidarityDinners(Guid ID);
        public bool SaveChanges();
    }
}
