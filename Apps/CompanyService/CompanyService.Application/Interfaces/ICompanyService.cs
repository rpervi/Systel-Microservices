using CompanyService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyService.Application.Interfaces
{
    public interface ICompanyService
    {
        Task<IEnumerable<companymaster>> GetAllCompaniesAsync();
        Task<companymaster?> GetCompanyByIdAsync(int id);
        Task<bool> CreateCompanyAsync(companymaster company);
        Task<bool> UpdateCompanyAsync(companymaster company);
        Task<bool> DeleteCompanyAsync(int id);
    }
}
