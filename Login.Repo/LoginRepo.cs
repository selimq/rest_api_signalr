using Login.Data;
using Login.Services;
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

namespace Login.Repo
{
    public class LoginRepo : ILogin, IConnection, ICache, IPhoto//,IMessages
    {
        private readonly LoginDbContext _db;
        public LoginRepo(LoginDbContext db)
        {
            _db = db;
        }


        IQueryable<Person> ILogin.GetLogins => _db.Girisler;

        IQueryable<Connection> IConnection.GetConnections => _db.Connection;

        //Login
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
                model.Message = "Geçerli personel bulunamadı.";
            }
            return model;
        }

        public async Task<Person> GetLogin(int? id)
        {
            Person login = new Person();
            if (id != null)
            {
                login = await _db.Girisler.FindAsync(id);
            }
            return login;
        }

        public async Task<POJO> Save(Person login)
        {
            POJO model = new POJO();

            //Add
            if (login.Id == 0)
            {
                try
                {

                    await _db.Girisler.AddAsync(login);
                    await _db.SaveChangesAsync();

                    //eklemeden sonra bir de connection tablosuna ekleme yapıyoruz
                    Connection con = new Connection();
                    con.UserName = Convert.ToString(login.Id);
                    con.Connected = '0';
                    con.LastSeen = DateTime.Now;
                    _db.Connection.Add(con);
                    //connection tablosu gibi bir de photo için ekleme yapılacak
                    Photo pp = new Photo();
                    pp.Id = login.Id;
                    _db.Images.Add(pp);

                    //token üretimi
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes("12345678909876543211234567890");
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                    new Claim(ClaimTypes.Name, login.Id.ToString())
                        }),
                        Expires = DateTime.UtcNow.AddYears(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    login.Token = tokenHandler.WriteToken(token);


                    model.Token =login.Token;
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
                _Entity.Token = login.Token;
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


        public String Authenticate(String userID)
        {/*
            Person user = await _db.Girisler.SingleOrDefaultAsync(x => x.Ad == ad && x.Sifre == sifre);
            if (user == null)
                return null;
            */
            // Authentication(Yetkilendirme) başarılı ise JWT token üretilir.
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("12345678909876543211234567890");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userID)
                }),
                Expires = DateTime.UtcNow.AddYears(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }



        /// Conn state

        public async Task<POJO> _Save(Connection connection)
        {

            POJO model = new POJO();

            if (connection.ConnectionID == 0)
            {
                try
                {
                    await _db.Connection.AddAsync(connection);
                    await _db.SaveChangesAsync();
                    model.Id = connection.ConnectionID;
                    model.Flag = true;
                    model.Message = "Comp";
                }
                catch (Exception e)
                {
                    model.Flag = false;
                    model.Message = "hata" + e.ToString();

                }
            }
            else if (connection.ConnectionID != 0)
            {
                Connection conn = new Connection();
                conn = await _GetConnection(connection.UserName);
                conn.ConnectionID = connection.ConnectionID;
                conn.Connected = connection.Connected;
                conn.UserName = connection.UserName;
                conn.LastSeen = connection.LastSeen;
                try
                {
                    await _db.SaveChangesAsync();
                    model.Id = int.Parse(connection.UserName);
                    model.Flag = true;
                    model.Message = "Comp";
                }
                catch (Exception e)
                {
                    model.Flag = false;
                    model.Message = "hata" + e.ToString();

                }

            }
            return model;
        }
        public async Task<Connection> _GetConnection(String name)
        {
            Connection connection = new Connection();
            if (name != null)
            {
                connection = await _db.Connection.Where(x => x.UserName == name).FirstOrDefaultAsync();
            }
            return connection;

        }




        //Messages
        /*
        public async Task Save(ChatMessage message)
        {

            if (message.id == 0)
            {

                await _db.Messages.AddAsync(message);
                await _db.SaveChangesAsync();

            }
            else if (message.id != 0)
            {
                ChatMessage msg = new ChatMessage();
                msg = await GetMessage(message.id);
                msg.id = message.id;
                msg.Message = message.Message;
                msg.Sender = message.Sender;
                msg.ToUser = message.ToUser;
                msg.Time = message.Time;
                msg.IsRead = message.IsRead;
                msg.IsSend = msg.IsSend;


                await _db.SaveChangesAsync();
            }

        }


        public async Task<ChatMessage> GetMessage(int id)
        {
            ChatMessage msg = new ChatMessage();
            if (id != 0)
            {
                msg = await _db.Messages.FindAsync(id);
            }
            return msg;
        }*/


        //CACHE


        public async Task Save(CacheMessage message)
        {

            if (message.id == 0)
            {

                await _db.CacheMessages.AddAsync(message);
                await _db.SaveChangesAsync();

            }
            else if (message.id != 0)
            {
                CacheMessage msg = new CacheMessage();
                msg = await GetMessage(message.id);
                msg.id = message.id;
                msg.Message = message.Message;
                msg.Sender = message.Sender;
                msg.ToUser = message.ToUser;
                msg.Time = message.Time;
                msg.TypeId = message.TypeId;
                await _db.SaveChangesAsync();
            }

        }


        public async Task<CacheMessage> GetMessage(int id)
        {
            CacheMessage msg = new CacheMessage();
            if (id != 0)
            {
                msg = await _db.CacheMessages.FindAsync(id);
            }
            return msg;
        }

        public async Task<List<CacheMessage>> GetMessages(string user)
        {
            List<CacheMessage> messages;
            messages = await _db.CacheMessages.Where(x => x.ToUser == user).ToListAsync();
            return messages;
        }
        public void Delete(CacheMessage message)
        {
            _db.CacheMessages.Remove(message);
            _db.SaveChanges();
        }

        //PHOTO

        public async Task<POJO> SavePhoto(Photo photo)
        {

            POJO pojo = new POJO();
            try
            {

                Photo pht = new Photo();
                pht = await GetPhoto(photo.Id);
                pht.Photo64 = photo.Photo64;
                await _db.SaveChangesAsync();

                pojo.Flag = true;
                pojo.Message = "Başarıyla eklendi.";
                pojo.Id = photo.Id;
                return pojo;

            }
            catch (Exception e)
            {
                pojo.Message = e.Data + "Hatası";
                pojo.Flag = false;
                return pojo;
            }

        }
        public async Task<Photo> GetPhoto(int? id)
        {
            Photo photo = new Photo();
            if (id != 0)
            {

                photo = await _db.Images.FindAsync(id);
                Console.WriteLine(photo.Photo64);

            }
            return photo;

        }
        public async Task<POJO> DeletePhoto(int? id)
        {
            POJO pojo = new POJO();
            try
            {
                Photo photo = await GetPhoto(id);
                _db.Images.Remove(photo);
                _db.SaveChanges();
                pojo.Flag = true;
                pojo.Message = "Başarıyla silindi.";
                return pojo;
            }
            catch (Exception e)
            {
                pojo.Flag = false;
                pojo.Message = "Hata" + e.Data;
                return pojo;
            }
        }



    }

}
