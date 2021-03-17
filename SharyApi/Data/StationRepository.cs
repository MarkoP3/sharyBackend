using SharyApi.Entities;
using SharyApi.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharyApi.Data
{
    public class StationRepository : IStationRepository
    {
        public StationRepository(Shary2Context context, IAuthenticationHelper authenticationHelper)
        {
            Context = context;
            AuthenticationHelper = authenticationHelper;
        }

        public Shary2Context Context { get; }
        public IAuthenticationHelper AuthenticationHelper { get; }

        public MealPrice GetActiveMealPrice()
        {
            return Context.MealPrices.Where(x => x.ValidTo == null).FirstOrDefault();
        }

        public Station GetStationByID(Guid ID)
        {
            return Context.Stations.Find(ID);
        }

        public ICollection<Station> GetStations()
        {
            return Context.Stations.ToList();
        }

        public ReceivedMeal ReceiveMeal(ReceivedMeal receivedMeals)
        {
            throw new NotImplementedException();
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
