using CompanyService.Application.Interfaces;
using CompanyService.Domain.Entities;
using CompanyService.Domain.Interfaces;
using Systel.Shared.ResponseEntity;
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

        public async Task<ResponseEntity<IEnumerable<companymaster>>> GetAllCompaniesAsync()
        {
            // Logic: You could filter out deleted items here if not handled by Repo
            var list = await _repository.GetAllAsync();

            if (list == null || !list.Any())
                return ResponseEntity<IEnumerable<companymaster>>.Failure("No companies found", 404);

            return ResponseEntity<IEnumerable<companymaster>>.Success(list);

        }

        public async Task<ResponseEntity<companymaster?>> GetCompanyByIdAsync(int id)
        {
            // 1. Fetch the entity from the repository
            var company = await _repository.GetByIdAsync(id);

            // 2. Check if the entity exists
            if (company == null)
            {
                // Return a 404 Failure if not found
                return ResponseEntity<companymaster?>.Failure(
                    message: $"Company with ID {id} was not found.",
                    code: 404
                );
            }

            // 3. Return a 200 Success with the data
            return ResponseEntity<companymaster?>.Success(
                data: company,
                message: "Company details retrieved successfully."
            );
        }

        public async Task<ResponseEntity<bool>> CreateCompanyAsync(companymaster company)
        {
            // Business Logic: Check if CCode already exists
            var existing = await _repository.FindAsync(x => x.ccode == company.ccode);
            if (existing.Any())
                return ResponseEntity<bool>.Failure("Company code already exists", 409);

            await _repository.AddAsync(company);
            var result = await _repository.SaveChangesAsync() > 0;

            return result
                ? ResponseEntity<bool>.Success(true, "Created successfully")
                : ResponseEntity<bool>.Failure("Database save failed", 500);

        }

        public async Task<ResponseEntity<bool>> UpdateCompanyAsync(companymaster company)
        {
            // 1. Check if the record exists in the database
            var existing = await _repository.GetByIdAsync(company.companyid);
            if (existing == null)
            {
                return ResponseEntity<bool>.Failure($"Cannot update. Company with ID {company.companyid} not found.", 404);
            }

            // 2. Apply logic updates
            // Ensure the date is UTC for PostgreSQL compatibility
            company.modifiedon = DateTime.UtcNow;

            // 3. Perform update and save
            _repository.Update(company);
            var result = await _repository.SaveChangesAsync() > 0;

            if (result)
            {
                return ResponseEntity<bool>.Success(true, "Company updated successfully.");
            }

            return ResponseEntity<bool>.Failure("Failed to update company in the database.", 500);
        }

        public async Task<ResponseEntity<bool>> DeleteCompanyAsync(int id)
        {
            // 1. Check if the record exists
            var company = await _repository.GetByIdAsync(id);
            if (company == null)
            {
                return ResponseEntity<bool>.Failure($"Cannot delete. Company with ID {id} not found.", 404);
            }

            // 2. Logic: Soft Delete (Set isdeleted flag instead of removing from DB)
            company.isdeleted = true;
            company.modifiedon = DateTime.UtcNow;

            // 3. Save changes
            _repository.Update(company);
            var result = await _repository.SaveChangesAsync() > 0;

            if (result)
            {
                return ResponseEntity<bool>.Success(true, "Company deleted (deactivated) successfully.");
            }

            return ResponseEntity<bool>.Failure("An error occurred while deleting the company.", 500);
        }
    }
}
