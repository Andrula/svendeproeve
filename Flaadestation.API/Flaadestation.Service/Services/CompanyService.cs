using Flaadestation.Repository.Database.Entities;
using Flaadestation.Repository.Repositories.Interfaces;
using Flaadestation.Service.DTO.CompanyDTO;
using Flaadestation.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        // Metode til og hente virksomhedsinformationer
        public async Task<CompanyResponseDTO?> GetCompanyByIdAsync(Guid id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            return company == null ? null : MapCompanyToCompanyResponse(company);
        }

        // Metode til at oprette virksommhed
        public async Task<CompanyResponseDTO> CreateCompanyAsync(CompanyRequestDTO companyRequest)
        {
            var nameExists = await _companyRepository.CompanyExistsByNameAsync(companyRequest.Name);
            if (nameExists)
            {
                throw new InvalidOperationException($"Virksomhed med navnet '{companyRequest.Name}' eksisterer allerede.");
            }

            var company = MapCompanyRequestToCompany(companyRequest);

            var createdCompany = await _companyRepository.AddAsync(company);
            await _companyRepository.SaveChangesAsync();
            return MapCompanyToCompanyResponse(createdCompany);
        }

        // Metode til at opdatere virksomhedsinformationer
        public async Task<CompanyResponseDTO?> UpdateCompanyAsync(Guid id, CompanyRequestDTO companyRequest)
        {
            var existingCompany = await _companyRepository.GetByIdAsync(id);
            if (existingCompany == null)
                return null;

            if (existingCompany.Name != companyRequest.Name)
            {
                var nameExists = await _companyRepository.CompanyExistsByNameAsync(companyRequest.Name);
                if (nameExists)
                {
                    throw new InvalidOperationException($"Virksomhed med navnet '{companyRequest.Name}' eksisterer allerede.");
                }
            }

            existingCompany.Name = companyRequest.Name;
            existingCompany.AddressId = companyRequest.AddressId;

            _companyRepository.Update(existingCompany);
            await _companyRepository.SaveChangesAsync();
            return MapCompanyToCompanyResponse(existingCompany);
        }

        // Metode til at slette virksomhed
        public async Task<bool> DeleteCompanyAsync(Guid id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null)
            {
                return false;
            }
            else
            {
                _companyRepository.Delete(id);
                await _companyRepository.SaveChangesAsync();
                return true;
            }
        }

        // Metode til at tjekke om virksomhedsnavn er optaget
        public async Task<bool> IsCompanyNameAvailableAsync(string name)
        {
            return !await _companyRepository.CompanyExistsByNameAsync(name);
        }

        private CompanyResponseDTO MapCompanyToCompanyResponse(Company company)
        {
            return new CompanyResponseDTO
            {
                CompanyId = company.CompanyId,
                Name = company.Name,
            };
        }

        private Company MapCompanyRequestToCompany(CompanyRequestDTO companyRequest)
        {
            return new Company
            {
                Name = companyRequest.Name,
                AddressId = companyRequest.AddressId,
            };
        }
    }
}
