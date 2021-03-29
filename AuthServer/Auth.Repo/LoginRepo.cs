using Auth.Data;
using Auth.Helpers;
using Auth.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Repo
{
    public class LoginRepo : ILogin
    {
        private readonly LoginDbContext _db;

        private readonly AppSettings appSettings;
 
 
        public LoginRepo(LoginDbContext db , IOptions<AppSettings> _appSettings)
        {
            _db = db;
            appSettings = _appSettings.Value;
        }

        IQueryable<Person> ILogin.GetLogins => _db.Girisler;



        //delete
        public async Task<POJO> Delete(int? id)
        {
            POJO model = new POJO();
            Person login = await GetLogin(id);
            if (login != null)
            {
                try
                {
                    _db.Girisler.Remove(login);
                    await _db.SaveChangesAsync();
                    //mesaj
                    model.Flag = true;
                    model.Message = "Silme işlemi tamamlandı";
                }
                catch (Exception ex)
                {
                    model.Flag = false;
                    model.Message = ex.ToString();
                }
            }
            else
            {
                model.Flag = false;
                model.Message = "Geçerli kullanıcı bulunamadı.";
            }
            return model;
        }
        
        //get login
        public async Task<Person> GetLogin(int? id)
        {
            Person login = new Person();
            if(id != null)
            {
                login = await _db.Girisler.FindAsync(id);
            }
            login.Sifre = null;
            return login;
        }

        //save
        public async Task<POJO> Save(Person login)
        {
            POJO model = new POJO();
            //Add
            if (login.Id == 0)
            {
                try
                {
                    await _db.AddAsync(login);
                    await _db.SaveChangesAsync();

                    model.Id = login.Id;
                    model.Flag = true;
                    model.Message = "Ekleme tamamlandı.";
                }
                catch (Exception ex)
                {
                    model.Flag = false;
                    model.Message = ex.ToString();
                }
            }
            //update
            else if (login.Id != 0)
            {
                Person _Entity = await GetLogin(login.Id);
                _Entity.Id = login.Id;
                _Entity.Ad = login.Ad;
                _Entity.Soyad = login.Soyad;
                _Entity.Mail = login.Mail;
                _Entity.Sifre = login.Sifre;
                _Entity.Role = login.Role;
                _Entity.PublicKey = login.PublicKey;
              
                try
                {
                    await _db.SaveChangesAsync();
                    model.Id = login.Id;
                    model.Flag = true;
                    model.Message = "Güncelleme işlemi tamamlandı";
                }
                catch (Exception ex)
                {
                    model.Flag = false;
                    model.Message = ex.ToString();
                }
            }
            return model;
        }

        //auth
        public async Task<Person> Authenticate(String ad,String sifre)
        {
            Person user = await _db.Girisler.SingleOrDefaultAsync(x => x.Ad == ad && x.Sifre == sifre);
            if (user == null)
                return null;

            // Authentication(Yetkilendirme) başarılı ise JWT token üretilir.
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role , user.Role),
                    new Claim(ClaimTypes.GivenName ,user.Ad)
                }),
                Expires = DateTime.UtcNow.AddDays(360),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // Sifre null olarak gonderilir.
            user.Sifre = null;
            return user;
        }


        /*   public async Task<Person> GetLoginWithMail(String mail)
           {
               Person login = new Person();
               if (mail != null)
               {
                   login = await _db.Girisler.Where(x => x.Mail ==mail).FirstOrDefaultAsync();

               }
               return login;
           }

           public async Task<List<Person>> GetLoginsWithMail(String mail)
           {
               List<Person> logins = new List<Person>();
               if (mail != null)
               {
                   logins = await _db.Girisler.Where(i => i.Mail == mail).ToListAsync();
               }
               return logins;
           }*/
    }
}
