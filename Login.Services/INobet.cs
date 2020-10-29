using Login.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Services
{
    public interface INobet
    {
        Task<Nobet> GetNobet(int? id);
        IQueryable<Nobet> GetNobets { get; }
        Task<POJO> Save(Nobet nobet);
        Task<POJO> Nobet_Delete(int? Id);
        Task<List<Nobet>> GetNobetsWithId(int? id);
    }
}
