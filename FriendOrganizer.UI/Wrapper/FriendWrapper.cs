using FriendOrganizer.Model;
using FriendOrganizer.UI.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Wrapper
{
   public class FriendWrapper : ViewModelBase, INotifyDataErrorInfo
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

	  private Dictionary<string, List<string>> _errorsByPropertyName
		  = new Dictionary<string, List<string>>();
	  public bool HasErrors => _errorsByPropertyName.Any();

	  public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

	  public IEnumerable GetErrors(string propertyName)
	  {
		 return _errorsByPropertyName.ContainsKey(propertyName)
			 ? _errorsByPropertyName[propertyName]
			 : null;
	  }

	  private void OnErrorChanged(string propertyName)
	  {
		 ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
	  }

	  private void AddError(string propertyName, string error)
	  {
		 if (!_errorsByPropertyName.ContainsKey(propertyName))
		 {
			_errorsByPropertyName[propertyName] = new List<string>();
		 }
		 if (!_errorsByPropertyName[propertyName].Contains(error))
		 {
			_errorsByPropertyName[propertyName].Add(error);
			OnErrorChanged(propertyName);
		 }
	  }
	  private void ClearErrors(string propertyName)
	  {
		 if (!_errorsByPropertyName.ContainsKey(propertyName))
		 {
			_errorsByPropertyName.Remove(propertyName);
			OnErrorChanged(propertyName);
		 }
	  }

   }
}
