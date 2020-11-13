using System.Collections.Generic;
using System.Threading.Tasks;
using Login.Services;

namespace Login.Repo{
    public class MessageRepo:IMessages{
        private readonly MessageDbContext db;
        public MessageRepo(MessageDbContext _db){
            db = _db;
        }
        public async Task<ChatMessage> Save(ChatMessage message){

                await db.AddAsync(message);
                await db.SaveChangesAsync();
                return message;

        }
        public async Task<List<ChatMessage>> GetHistory(){
                return null;
        }
    }
}