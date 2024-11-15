using Gateway.Models;
using System.Threading.Tasks;

namespace Gateway.ServiceInterfaces
{
    public interface ILoyaltyService
    {

        public Task<Loyalty?> GetLoyaltyByUsernameAsync(string username);

        public Task<Loyalty?> PutLoyaltyByUsernameAsync(string username);

        public Task<Loyalty?> DeleteLoyaltyByUsernameAsync(string username);
    }
}
