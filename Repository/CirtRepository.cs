using Microsoft.EntityFrameworkCore;
using StudentForm.Data;
using StudentForm.Models;
using StudentForm.Services;

namespace StudentForm.Repository
{
    public class CirtRepository : ICity
    {
        private readonly MyDBContext _context;

        public CirtRepository(MyDBContext context)
        {
            _context = context;
        }

        public async Task<List<City>> GetAllCities()
        {
            return await _context.Cities.ToListAsync();
        }

        public async Task<List<Country>> GetAllCountries()
        {
            return await _context.Countries.ToListAsync();
        }

        public async Task<List<State>> GetStatesByCountryId(int countryId)
        {
            return await _context.States
                .Where(s => s.CountryId == countryId)
                .ToListAsync();
        }

        public async Task<List<City>> GetCitiesByStateId(int stateId)
        {
            return await _context.Cities
                .Where(c => c.StateId == stateId)
                .ToListAsync();
        }
    }
}
