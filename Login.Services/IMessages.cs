using System.Collections.Generic;
using System.Threading.Tasks;

namespace Login.Services{
    public interface IMessages{
        Task Save (ChatMessage message);
        Task<ChatMessage> GetMessage (int id);

    }
}