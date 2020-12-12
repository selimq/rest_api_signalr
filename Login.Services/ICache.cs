using System.Collections.Generic;
using System.Threading.Tasks;

namespace Login.Services{
    public interface ICache{
        Task Save (CacheMessage message);
        Task<CacheMessage> GetMessage (int id);
        Task<List<CacheMessage>> GetMessages(string username);
        
        void Delete (CacheMessage message);
    }
}