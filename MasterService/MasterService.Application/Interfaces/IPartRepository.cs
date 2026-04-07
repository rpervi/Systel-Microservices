using MasterService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterService.Application.Interfaces
{
    public interface IPartRepository
    {
        Task<PartMaster> GetByIdAsync(int id);
        Task<IEnumerable<PartMaster>> GetAllAsync();
        Task<int> AddAsync(PartMaster part);
    }
}
