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
        public async  Task<Friend> GetByIdAsync(int id)
        {
            using (var ctx = _contextCreator())
            {
                //var friends = await ctx.Friends.AsNoTracking().ToListAsync();
                //await Task.Delay(5000);

                //return friends;
                return await ctx.Friends.AsNoTracking().SingleOrDefaultAsync(f => f.Id == id);
            }
        }

        public async Task SaveAsync(Friend friend)
        {
            using (var ctx = _contextCreator())
            {
                ctx.Friends.Attach(friend);
                ctx.Entry(friend).State = EntityState.Modified;
                await ctx.SaveChangesAsync();
            }
        }
    }


}
