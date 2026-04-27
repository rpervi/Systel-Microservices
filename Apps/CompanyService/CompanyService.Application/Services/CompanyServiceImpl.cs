using CompanyService.Application.Interfaces;
using CompanyService.Domain.Entities;
using CompanyService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyService.Application.Services
{
    public class CompanyServiceImpl : ICompanyService
    {
        private readonly ICompanyRepository _repository;

        public CompanyServiceImpl(ICompanyRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<companymaster>> GetAllCompaniesAsync()
        {
            // Logic: You could filter out deleted items here if not handled by Repo
            return await _repository.GetAllAsync();
        }

        public async Task<companymaster?> GetCompanyByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> CreateCompanyAsync(companymaster company)
        {
            // Business Logic Example: Ensure code is uppercase
            if (!string.IsNullOrEmpty(company.ccode))
            {
                company.ccode = company.ccode.ToUpper();
            }
            
            if (company.modifiedon.HasValue)
            {
                company.modifiedon = DateTime.SpecifyKind(company.modifiedon.Value, DateTimeKind.Utc);
            }

            // Set audit fields
            company.createdon = DateTime.SpecifyKind(company.createdon.Value, DateTimeKind.Utc);
            company.isactive = true;

            await _repository.AddAsync(company);
            return await _repository.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateCompanyAsync(companymaster company)
        {
            var existing = await _repository.GetByIdAsync(company.companyid);
            if (existing == null) return false;

            // Manual mapping or logic updates
            company.modifiedon = DateTime.UtcNow;

            _repository.Update(company);
            return await _repository.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCompanyAsync(int id)
        {
            var company = await _repository.GetByIdAsync(id);
            if (company == null) return false;

            // Logic: Use Soft Delete instead of hard removal
            company.isdeleted = true;
            _repository.Update(company);

            return await _repository.SaveChangesAsync() > 0;
        }
    }
}
