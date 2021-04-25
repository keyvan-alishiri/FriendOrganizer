using FriendOrganizer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Repositories

{
    public interface IFriendRepository
    {
        Task<Friend> GetByIdAsync(int id);
        Task SaveAsync();
        bool HasChanges();
    }
}