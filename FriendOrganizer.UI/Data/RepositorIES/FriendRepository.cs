using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Repositories
{
   public class FriendRepository : IFriendRepository
   {

	  private FriendOrganizerDbContext _context;

	  public FriendRepository(FriendOrganizerDbContext context)
	  {
		 _context = context;
	  }

	  public void Add(Friend friend)
	  {
		 _context.Friends.Add(friend);
	  }

	  public async Task<Friend> GetByIdAsync(int id)
	  {

		 return await _context.Friends.SingleOrDefaultAsync(f => f.Id == id);

	  }

	  public bool HasChanges()
	  {
		 return _context.ChangeTracker.HasChanges();
	  }

	  public async Task SaveAsync()
	  {


		 await _context.SaveChangesAsync();

	  }
   }


}
