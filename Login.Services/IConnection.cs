using System.Linq;
using System.Threading.Tasks;
using Login.Data;

namespace Login.Services{
    public interface IConnection{
        Task<Connection> _GetConnection(string name);
        Task<POJO> _Save(Connection connection);

        IQueryable <Connection> GetConnections { get; }
        
    }
}