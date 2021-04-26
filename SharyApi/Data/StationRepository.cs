using CSharpFunctionalExtensions;
using SharyApi.Entities;
using SharyApi.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharyApi.Data
{
    public class StationRepository : IStationRepository
    {
        public StationRepository(Shary2Context context)
        {
            Context = context;
        }

        public Shary2Context Context { get; }
        public IAuthenticationHelper AuthenticationHelper { get; }

        public MealPrice GetActiveMealPrice()
        {
            return Context.MealPrices.Where(x => x.ValidTo == null).FirstOrDefault();
        }

        public Maybe<Station> GetStationByID(Guid ID)
        {
            return Context.Stations.Find(ID);
        }

        public Maybe<Station> GetStationByUsername(string username)
        {
            return Context.Stations.TryFirst(station => station.Username == username);
        }

        public ICollection<Station> GetStations()
        {
            return Context.Stations.ToList();
        }

        public ReceivedMeal ReceiveMeal(ReceivedMeal receivedMeals)
        {
            var receivedMeal = Context.Add(receivedMeals);
            return receivedMeal.Entity;
        }

        public bool SaveChanges()
        {
            return Context.SaveChanges() > 0;
        }

        public SharedMeal ShareMeal(Guid ID)
        {
            var sharedMeal = Context.Add(new SharedMeal() { Id = Guid.NewGuid(), ShareDateTime = DateTime.Now, StationId = ID });
            return sharedMeal.Entity;
        }
    }
}
