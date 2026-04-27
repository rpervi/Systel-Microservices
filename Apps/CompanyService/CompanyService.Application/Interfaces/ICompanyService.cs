using CompanyService.Domain.Entities;
using Systel.Shared.ResponseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyService.Application.Interfaces
{
    public interface ICompanyService
    {
        Task<ResponseEntity<IEnumerable<companymaster>>> GetAllCompaniesAsync();
        Task<ResponseEntity<companymaster?>> GetCompanyByIdAsync(int id);
        Task<ResponseEntity<bool>> CreateCompanyAsync(companymaster company);
        Task<ResponseEntity<bool>> UpdateCompanyAsync(companymaster company);
        Task<ResponseEntity<bool>> DeleteCompanyAsync(int id);
    }
}
