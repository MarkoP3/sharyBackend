using CSharpFunctionalExtensions;
using SharyApi.Entities;
using System;
using System.Collections.Generic;

namespace SharyApi.Data
{
    public interface IStationRepository
    {
        ICollection<Station> GetStations();
        Maybe<Station> GetStationByID(Guid ID);
        SharedMeal ShareMeal(Guid ID);
        ReceivedMeal ReceiveMeal(ReceivedMeal receivedMeals);
        MealPrice GetActiveMealPrice();
        Maybe<Station> GetStationByUsername(string username);
        bool SaveChanges();
    }
}
