using Auth.Data;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Services
{
    public interface ILogin
    {
        Task<Person> GetLogin(int? id);
        Task<Person> Authenticate(string ad, String soyad);
        IQueryable <Person> GetLogins { get; }
        Task<POJO> Save(Person giris);
        Task<POJO> Delete(int? Id);
       // Task<Person> GetLoginWithMail(String mail);
      //  Task<List<Person>> GetLoginsWithMail(String mail);

    }
    
}
