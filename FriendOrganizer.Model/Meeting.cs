using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace FriendOrganizer.Model
{
   public class Meeting
   {
	  public Meeting()
	  {
		 Friends = new Collection<Friend>();
	  }

	  public int Id { get; set; }
	
	  [StringLength(50)]
	  [Display(Name = "عنوان")]
	  [Required(ErrorMessage = "مقدار {0} نمی تواند خالی باشد")]
	  public string Title { get; set; }
	  public DateTime DateFrom { get; set; }
	  public DateTime DateTo { get; set; }

	  public ICollection<Friend> Friends { get;  set; }
   }
}
