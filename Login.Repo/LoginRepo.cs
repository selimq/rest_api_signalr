using Login.Data;
using Login.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Repo
{
    public class LoginRepo : ILogin,INobet
    {
        private readonly LoginDbContext _db;
        public LoginRepo(LoginDbContext db)
        {
            _db = db;
        }

        IQueryable<Nobet> INobet.GetNobets => _db.Nobetler;
        IQueryable<Giris> ILogin.GetLogins => _db.Girisler;
        
        //Login
        public async Task<POJO> Delete(int? id)
        {
            POJO model = new POJO();
            Giris login = await GetLogin(id);
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
                model.Message = "Geçerli personel bulunamadı.";
            }
            return model;
        }

        public async Task<Giris> GetLogin(int? id)
        {
            Giris login = new Giris();
            if(id != null)
            {
                login = await _db.Girisler.FindAsync(id);
            }
            return login;
        }

        public async Task<Giris> GetLoginWithMail(String mail)
        {
            Giris login = new Giris();
            if (mail != null)
            {
                login = await _db.Girisler.Where(x => x.Mail ==mail).FirstOrDefaultAsync();
         
            }
            return login;
        }

        public async Task<List<Giris>> GetLoginsWithMail(String mail)
        {
            List<Giris> logins = new List<Giris>();
            if (mail != null)
            {
                logins = await _db.Girisler.Where(i => i.Mail == mail).ToListAsync();
            }
            return logins;
        }

        public async Task<POJO> Save(Giris login)
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
                Giris _Entity = await GetLogin(login.Id);
                _Entity.Id = login.Id;
                _Entity.Ad = login.Ad;
                _Entity.Soyad = login.Soyad;
                _Entity.Mail = login.Mail;
                _Entity.Sifre = login.Sifre;
                _Entity.Unvan = login.Unvan;
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


        //Nobet
        public async Task<Nobet> GetNobet(int? id)
        {
            Nobet nobet = new Nobet();
            if (id != null)
            {
                nobet = await _db.Nobetler.FindAsync(id);
            }
            return nobet;
        }

        public async Task<POJO> Save(Nobet nobet)
        {
            POJO model = new POJO();
            if (nobet.Id == 0)
            {
                try
                {
                    await _db.AddAsync(nobet);
                    await _db.SaveChangesAsync();

                    model.Id = nobet.Id;
                    model.Flag = true;
                    model.Message = "Ekleme tamamlandı.";
                }
                catch (Exception ex)
                {
                    model.Flag = false;
                    model.Message = ex.ToString();
                }
            }
            else if (nobet.Id != 0)
            {
                Nobet _Entity = await GetNobet(nobet.Id);
                _Entity.Id = nobet.Id;
                _Entity.Nobet_Yer = nobet.Nobet_Yer;
                _Entity.Ogretmen_Id = nobet.Ogretmen_Id;
                _Entity.Nobet_Zaman = nobet.Nobet_Zaman;
              
                try
                {
                    await _db.SaveChangesAsync();
                    model.Id = nobet.Id;
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

        public async Task<List<Nobet>> GetNobetsWithId(int? id)
        {
            List<Nobet> nobetler = new List<Nobet>();
            if(id != null)
            {
                nobetler = await _db.Nobetler.Where(i => i.Ogretmen_Id == id).ToListAsync();
            }
            return nobetler;
        }
        
        public async Task<POJO> Nobet_Delete (int? id)
        {

            POJO model = new POJO();
            Nobet nobet = await GetNobet(id);
            if (nobet != null)
            {
                try
                {
                    _db.Nobetler.Remove(nobet);
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
                model.Message = "Geçerli personel bulunamadı.";
            }
            return model;

        }
    }
}
