using Flaadestation.Repository.Database;
using Flaadestation.Repository.Database.Entities;
using Flaadestation.Repository.Repositories;
using Flaadestation.Repository.Repositories.Interfaces;
using Flaadestation.Service.DTO.LicenseDTO;
using Flaadestation.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.Services
{
    public class LicenseService : ILicenseService
    {
        private readonly ILicenseRepository _licenseRepository;
        private readonly ApplicationDBContext _context;

        public LicenseService(ILicenseRepository licenseRepository, ApplicationDBContext context)
        {
            _licenseRepository = licenseRepository;
            _context = context;
        }

        public async Task<IEnumerable<LicenseResponseDTO>> AddRangeByCompanyIdAsync(Guid companyId, int amount)
        {
            var licenses = new List<License>();
            for (int i = 0; i < amount; i++)
            {
                licenses.Add(new License
                {
                    CompanyId = companyId,
                    LicenseKey = Guid.NewGuid(),
                    ValidFrom = DateTime.Today.AddDays(-1),
                    ValidTo = DateTime.Today.AddYears(1)
                });
            }

            var createdLicenses = await _licenseRepository.AddRangeAsync(licenses.ToArray());

            return createdLicenses.Select(MapLicenseToLicenseResponse);
        }

        public async Task<LicenseResponseDTO> CreateLicenseAsync(LicenseRequestDTO licenseRequest)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var license = MapLicenseRequestToLicense(licenseRequest);

                license.LicenseKey = Guid.NewGuid();
                license.ValidFrom = DateTime.Today.AddDays(-1);
                license.ValidTo = DateTime.Today.AddYears(1);

                var createdLicense = await _licenseRepository.AddAsync(license);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return MapLicenseToLicenseResponse(createdLicense);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteLicenseAsync(Guid licenseId)
        {
            var licenseEntity = await _licenseRepository.GetByIdAsync(licenseId);
            if (licenseEntity == null)
                return false;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _licenseRepository.Delete(licenseId);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<LicenseResponseDTO?> GetLicenseByLicenseKeyAsync(Guid licenseKey)
        {
            var license = await _licenseRepository.GetLicenseByLicenseKeyAsync(licenseKey);
            return license == null ? null : MapLicenseToLicenseResponse(license);
        }

        public async Task<IEnumerable<LicenseResponseDTO>> GetLicensesByCompanyIdAsync(Guid companyId)
        {
            var licenses = await  _licenseRepository.GetLicensesByCompanyIdAsync(companyId);
            return licenses.Select(MapLicenseToLicenseResponse);
        }

        public async Task<LicenseResponseDTO?> GetLicensesByUserIdAsync(string userId)
        {
            var license = await _licenseRepository.GetLicensesByUserIdAsync(userId);
            return license == null ? null : MapLicenseToLicenseResponse(license);
        }

        public async Task<LicenseResponseDTO?> UpdateLicenseByIdAsync(Guid licenseId, LicenseRequestDTO licenseRequest)
        {
            var existingLicense = await _licenseRepository.GetByIdAsync(licenseId);
            if (existingLicense == null)
                return null;

            existingLicense.ValidFrom = licenseRequest.ValidFrom;
            existingLicense.ValidTo = licenseRequest.ValidTo;
            existingLicense.UserId = licenseRequest.UserId;

            _licenseRepository.Update(existingLicense);
            await _licenseRepository.SaveChangesAsync();
            return MapLicenseToLicenseResponse(existingLicense);
        }

        private LicenseResponseDTO MapLicenseToLicenseResponse(License license)
        {
            return new LicenseResponseDTO
            {
                LicenseId = license.LicenseId,
                LicenseKey = license.LicenseKey,
                ValidFrom = license.ValidFrom,
                ValidTo = license.ValidTo,
                Company = license.Company is null ? null : new LicenseCompanyResponseDTO
                {
                    CompanyId = license.Company.CompanyId,
                    Name = license.Company.Name,
                },
                User = license.User is null ? null : new LicenseUserResponseDTO
                {
                    UserId = license.User.Id,
                    Email = license.User.Email ?? string.Empty,
                    UserName = license.User.UserName ?? string.Empty,
                    IsCompanyOwner = license.User.IsCompanyOwner,
                }
            };
        }

        private License MapLicenseRequestToLicense(LicenseRequestDTO licenseRequest)
        {
            return new License
            {
                ValidFrom = licenseRequest.ValidFrom,
                ValidTo = licenseRequest.ValidTo,
                CompanyId = licenseRequest.CompanyId,
                UserId = licenseRequest.UserId,
            };
        }
    }
}
