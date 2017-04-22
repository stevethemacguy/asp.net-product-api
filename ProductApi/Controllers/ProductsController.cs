using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ProductApi.Models;
using ProductApi.Services;

namespace ProductApi.Controllers
{
    [Route("api/products")]
    public class ProductsController : Controller
    {
        private IProductApiRepository _productApiRepo;
        public ProductsController(IProductApiRepository productApiRepo)
        {
            _productApiRepo = productApiRepo;
        }

        [HttpGet()]
        public IActionResult GetCities()
        {
            //Get the cities from the DB. NOTE: You don't return the entities directly, but use the DTO classes instead
            var cityEntities = _productApiRepo.GetCities();

            //Map the city entities to the DTO classes using automapper
            var allCities = AutoMapper.Mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities);

            return Ok(allCities);
        }

        [HttpGet("{id}")]
        //id is automatically set with the id parameters coming from the request URL
        public IActionResult GetCity(int id, bool includePois = false)
        {
            var cityToReturn = _productApiRepo.GetCity(id, includePois);

            if (cityToReturn == null)
            {
                return NotFound();
            }
            else
            {
                if (includePois)
                {
                    var cityResult = AutoMapper.Mapper.Map<CityDto>(cityToReturn);
                    return Ok(cityResult);
                }
                else
                {
                    var cityResult = AutoMapper.Mapper.Map<CityWithoutPointsOfInterestDto>(cityToReturn);
                    return Ok(cityResult);
                }
            }
        }

        [HttpGet("getcitybyname")]
        //id is automatically set with the id parameters coming from the request URL
        public IActionResult GetCityByName(string name, bool includePois = false)
        {
            var cityToReturn = _productApiRepo.GetCityByName(name, includePois);

            if (cityToReturn == null)
            {
                return NotFound();
            }
            else
            {
                if (includePois)
                {
                    var cityResult = AutoMapper.Mapper.Map<CityDto>(cityToReturn);
                    return Ok(cityResult);
                }
                else
                {
                    var cityResult = AutoMapper.Mapper.Map<CityWithoutPointsOfInterestDto>(cityToReturn);
                    return Ok(cityResult);
                }
            }
        }
    }
}
