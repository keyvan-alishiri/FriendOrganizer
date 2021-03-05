using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data
{
    public class FriendDataService : IFriendDataService
    {
        private Func<FriendOrganizerDbContext> _contextCreator;

        public FriendDataService(Func<FriendOrganizerDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }
        public async  Task<List<Friend>> GetAllAsync()
        {
            using (var ctx = _contextCreator())
            {
                //var friends = await ctx.Friends.AsNoTracking().ToListAsync();
                //await Task.Delay(5000);

                //return friends;
                 return await ctx.Friends.AsNoTracking().ToListAsync();
            }
        }
    }


}
