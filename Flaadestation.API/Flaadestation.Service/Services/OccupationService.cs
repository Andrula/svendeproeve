using Flaadestation.Repository.Database.Entities;
using Flaadestation.Repository.Repositories;
using Flaadestation.Repository.Repositories.Interfaces;
using Flaadestation.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Flaadestation.Service.Services
{
    public class OccupationService : IOccupationService
    {
        private readonly IOccupationRepository _occupationRepository;
        public OccupationService(IOccupationRepository occupationRepository)
        {
            _occupationRepository = occupationRepository;   
        }

        // Metode til at oprette beskæftigelse
        public async Task<Occupation> CreateOccupationAsync(Occupation occupation)
        {
            var occupationAlreadyExists = await _occupationRepository.OccupationExistsByNameAsync(occupation.Name);
            if (occupationAlreadyExists)
            {
                throw new InvalidOperationException($"Beskæftigelse '{occupation.Name}' eksisterer allerede.");
            }

            var createdCompany = await _occupationRepository.AddAsync(occupation);
            await _occupationRepository.SaveChangesAsync();
            return createdCompany;
        }

        // Metode til at slette beskæftigelse
        public async Task<bool> DeleteOccupationAsync(Guid id)
        {
            var occupation = await _occupationRepository.GetByIdAsync(id);
            if (occupation == null)
            {
                return false;
            }
            else
            {
                _occupationRepository.Delete(id);
                await _occupationRepository.SaveChangesAsync();
                return true;
            }
        }

        // Metode til at hente alle beskæftigelser
        public Task<IEnumerable<Occupation>> GetAllOccupationsAsync()
        {
            return _occupationRepository.GetAllAsync();
        }

        // Metode til at hente beskæftigelse via. ID.
        public async Task<Occupation> GetOccupationByIdAsync(Guid id)
        {
            return await _occupationRepository.GetByIdAsync(id);
        }

        // Metode til og tjekke om en beskæftning allerede eksisterer.
        public async Task<bool> IsOccupationNameAvailableAsync(string name)
        {
            return !await _occupationRepository.OccupationExistsByNameAsync(name);
        }

        // Metode til at opdatere beskæftigelse
        public async Task<Occupation?> UpdateOccupationAsync(Guid id, Occupation occupation)
        {
            var existingOccupation = await _occupationRepository.GetByIdAsync(id);
            if (existingOccupation == null)
                return null;

            if (existingOccupation.Name != occupation.Name)
            {
                var nameExists = await _occupationRepository.OccupationExistsByNameAsync(occupation.Name);
                if (nameExists)
                {
                    throw new InvalidOperationException($"Beskæftigelse med navnet '{occupation.Name}' eksisterer allerede.");
                }
            }

            existingOccupation.Name = occupation.Name;

            _occupationRepository.Update(existingOccupation);
            await _occupationRepository.SaveChangesAsync();
            return existingOccupation;
        }
    }
}
