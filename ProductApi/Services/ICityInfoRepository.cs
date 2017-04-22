﻿using System.Collections.Generic;
using ProductApi.Entities;

namespace ProductApi.Services
{
    public interface IProductApiRepository
    {
        //Get all the cities from the DB
        IEnumerable<City> GetCities();

        //Get a single city
        City GetCity(int cityId, bool includePointsOfInterest);

        //Get a city by it's name
        City GetCityByName(string name, bool includePointsOfInterest);

        //Check that a City exists
        bool CityExists(int cityId);

        //Get all POIs
        IEnumerable<PointOfInterest> GetPointsOfInterestForCity(int cityId);

        //Get a single POI for a specified CityId
        PointOfInterest GetPointOfInterestForCity(int cityId, int poiId);
        
        //Add a city
        void AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest);

        //Required to save new entities to the database context when they are created.
        bool Save();

        void DeletePointOfInterest(PointOfInterest pointOfInterest);
    }
}
