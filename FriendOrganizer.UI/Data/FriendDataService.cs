using FriendOrganizer.Model;
using System.Collections.Generic;

namespace FriendOrganizer.UI.Data
{
    public class FriendDataService : IFriendDataService
    {
        public IEnumerable<Friend> GetAll()
        {
            yield return new Friend { FirstName = "keyvan", LastName = "Alishir" };
            yield return new Friend { FirstName = "peyman", LastName = "Alishir" };
            yield return new Friend { FirstName = "hamed", LastName = "Alishir" };
        }
    }


}
