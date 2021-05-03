using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace FriendOrganizer.Model
{

   
    public class Friend
    {
	  public Friend()
	  {
        PhoneNumbers = new Collection<FriendPhoneNumber>();
         Meetings = new Collection<Meeting>();
	  } 
        public int Id { get; set; }

        [Display(Name ="نام")]
        [Required(ErrorMessage ="مقدار {0} نمی تواند خالی باشد")]
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }
	  public int? FavoriteLanguageId { get; set; }
	  public ProgrammingLanguage FavoriteLanguage { get; set; }

	  public ICollection<FriendPhoneNumber> PhoneNumbers { get; set; }
      public ICollection<Meeting> Meetings { get; set; }

   }
}
