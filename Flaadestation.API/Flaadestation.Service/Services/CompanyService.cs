using Flaadestation.Repository.Database.Entities;
using Flaadestation.Repository.Repositories.Interfaces;
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
        public async Task<Company?> GetCompanyByIdAsync(Guid id)
        {
            return await _companyRepository.GetByIdAsync(id);
        }

        // Metode til at oprette virksommhed
        public async Task<Company> CreateCompanyAsync(Company company)
        {
            var nameExists = await _companyRepository.CompanyExistsByNameAsync(company.Name);
            if (nameExists)
            {
                throw new InvalidOperationException($"Virksomhed med navnet '{company.Name}' eksisterer allerede.");
            }

            var createdCompany = await _companyRepository.AddAsync(company);
            await _companyRepository.SaveChangesAsync();
            return createdCompany;
        }

        // Metode til at opdatere virksomhedsinformationer
        public async Task<Company?> UpdateCompanyAsync(Guid id, Company company)
        {
            var existingCompany = await _companyRepository.GetByIdAsync(id);
            if (existingCompany == null)
                return null;

            if (existingCompany.Name != company.Name)
            {
                var nameExists = await _companyRepository.CompanyExistsByNameAsync(company.Name);
                if (nameExists)
                {
                    throw new InvalidOperationException($"Virksomhed med navnet '{company.Name}' eksisterer allerede.");
                }
            }

            existingCompany.Name = company.Name;
            existingCompany.AddressId = company.AddressId;

            _companyRepository.Update(existingCompany);
            await _companyRepository.SaveChangesAsync();
            return existingCompany;
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
    }
}
