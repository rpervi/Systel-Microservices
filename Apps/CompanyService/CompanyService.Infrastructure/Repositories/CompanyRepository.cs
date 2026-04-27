using CompanyService.Domain.Entities;
using CompanyService.Domain.Interfaces;
using CompanyService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyService.Infrastructure.Repositories
{
    public class CompanyRepository : Repository<companymaster>, ICompanyRepository
    {
        public CompanyRepository(CompanyDbContext context) : base(context) { }

        public async Task<companymaster?> GetByCodeAsync(string ccode)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.ccode == ccode);
        }

        public async Task<IEnumerable<companymaster>> GetActiveCompaniesAsync()
        {
            return await _dbSet.Where(x => x.isactive && !x.isdeleted).ToListAsync();
        }

        public async Task<IEnumerable<companymaster>> SearchByNameAsync(string name)
        {
            // Using EF.Functions.Like for PostgreSQL case-insensitive search if needed
            return await _dbSet
                .Where(x => x.cname != null && x.cname.Contains(name))
                .ToListAsync();
        }
    }
}
