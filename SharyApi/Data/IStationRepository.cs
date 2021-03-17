using SharyApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharyApi.Data
{
    public interface IStationRepository
    {
        ICollection<Station> GetStations();
        Station GetStationByID(Guid ID);
        SharedMeal ShareMeal(Guid ID);
        ReceivedMeal ReceiveMeal(ReceivedMeal receivedMeals);
        MealPrice GetActiveMealPrice();
        bool SaveChanges();
    }
}
