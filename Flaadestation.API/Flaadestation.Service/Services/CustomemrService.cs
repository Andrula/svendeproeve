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
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<Customer>> GetCustomersByCompanyAsync(Guid companyId)
        {
            return await _customerRepository.GetCustomersByCompanyAsync(companyId);
        }

        public async Task<Customer?> GetCustomerByIdAsync(Guid id)
        {
            return await _customerRepository.GetByIdAsync(id);
        }

        public async Task<Customer?> GetCustomerWithJobsAsync(Guid customerId)
        {
            return await _customerRepository.GetCustomerWithJobsAsync(customerId);
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            var emailExists = await _customerRepository.CustomerExistsByEmailAsync(customer.Email, customer.CompanyId);
            if (emailExists)
            {
                throw new InvalidOperationException($"Kunde med email '{customer.Email}' eksisterer allerede for denne virksomhed.");
            }

            var created = await _customerRepository.AddAsync(customer);
            await _customerRepository.SaveChangesAsync();
            return created;
        }

        public async Task<Customer?> UpdateCustomerAsync(Guid id, Customer customer)
        {
            var existing = await _customerRepository.GetByIdAsync(id);
            if (existing == null)
                return null;

            if (existing.Email != customer.Email)
            {
                var emailExists = await _customerRepository.CustomerExistsByEmailAsync(customer.Email, customer.CompanyId);
                if (emailExists)
                {
                    throw new InvalidOperationException($"Kunde med email '{customer.Email}' eksisterer allerede for denne virksomhed.");
                }
            }

            existing.Name = customer.Name;
            existing.Email = customer.Email;
            existing.Phone = customer.Phone;
            existing.AddressId = customer.AddressId;

             _customerRepository.Update(existing);
            await _customerRepository.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteCustomerAsync(Guid id)
        {
            var customer = await _customerRepository.GetCustomerWithJobsAsync(id);
            if (customer == null)
                return false;

            if (customer.Jobs.Any())
            {
                throw new InvalidOperationException("Kan ikke slette kunde der har aktive jobs. Slet jobs først.");
            }

             _customerRepository.Delete(id);
            await _customerRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsCustomerEmailAvailableAsync(string email, Guid companyId)
        {
            return !await _customerRepository.CustomerExistsByEmailAsync(email, companyId);
        }
    }
}
