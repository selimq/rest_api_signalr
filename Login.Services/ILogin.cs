using Login.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Login.Services
{
    public interface ILogin
    {
        Task<Giris> GetLogin(int? id);
        IQueryable <Giris> GetLogins { get; }
        Task<POJO> Save(Giris giris);
        Task<POJO> Delete(int? Id);
        Task<Giris> GetLoginWithMail(String mail);
        Task<List<Giris>> GetLoginsWithMail(String mail);

    }
}
