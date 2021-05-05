﻿using FriendOrganizer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Repositories
{
   public interface IMeetingRepository : IGenericRepository<Meeting>
   {
	  Task<List<Friend>> GetAllFriendsAnc();
	  Task ReloadFriendAsync(int friendId);
   }
}