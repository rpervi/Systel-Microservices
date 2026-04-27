using CompanyService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyService.Domain.Interfaces
{
    public interface ICompanyRepository : IRepository<companymaster>
    {
        // Custom method to find a company by its unique code
        Task<companymaster?> GetByCodeAsync(string ccode);

        // Custom method to check if a company is active
        Task<IEnumerable<companymaster>> GetActiveCompaniesAsync();

        // Custom method to search by name
        Task<IEnumerable<companymaster>> SearchByNameAsync(string name);
    }
}
