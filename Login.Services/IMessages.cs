using System.Collections.Generic;
using System.Threading.Tasks;

namespace Login.Services{
    public interface IMessages{
        Task<ChatMessage> Save (ChatMessage message);
        Task<List<ChatMessage>> GetHistory();

    }
}