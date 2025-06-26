using StudentForm.Models;

namespace StudentForm.Services
{
    public interface ICity
    {
        Task<List<City>> GetAllCities();

        Task<List<Country>> GetAllCountries();
        Task<List<State>> GetStatesByCountryId(int countryId);
        Task<List<City>> GetCitiesByStateId(int stateId);

    }
}
