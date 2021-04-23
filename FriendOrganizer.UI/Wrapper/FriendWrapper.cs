using FriendOrganizer.Model;
using System;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Wrapper
{
   public class FriendWrapper : NotifyDataErrorInfoBase
   {
	  public Friend Model { get; }
	  public FriendWrapper(Friend model)
	  {
		 Model = model;
	  }


	  public int Id { get { return Model.Id; } }
	  public string FirstName
	  {
		 get { return Model.FirstName; }
		 set
		 {
			Model.FirstName = value;
			OnPropertyChanged();
			Validateproperty(nameof(FirstName));

		 }
	  }

	  private void Validateproperty(string propertyName)
	  {
		 ClearErrors(propertyName);
		 switch (propertyName)
		 {
			case nameof(FirstName):
			   if (string.Equals(FirstName, "Robot", StringComparison.OrdinalIgnoreCase))
			   {
				  AddError(propertyName, "Robots are not valid friends");
			   }
			   break;
		 }
	  }

	  public string LastName
	  {
		 get { return Model.LastName; }
		 set
		 {
			Model.LastName = value;
			OnPropertyChanged();

		 }
	  }

	  public string Email
	  {
		 get { return Model.Email; }
		 set
		 {
			Model.Email = value;
			OnPropertyChanged();

		 }
	  }

	 

   }
}
