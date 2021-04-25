using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace FriendOrganizer.Model
{
    public class Friend
    {
        
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

    }
}
