using System.Threading.Tasks;
using Login.Data;

namespace Login.Services{
    public interface IPhoto{
        Task<POJO> SavePhoto(Photo image);
        Task<Photo> GetPhoto(int? id);
        Task<POJO> DeletePhoto(int? id);
    }
    
}