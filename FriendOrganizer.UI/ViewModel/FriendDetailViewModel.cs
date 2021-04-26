using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
   public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
   {
	  private IFriendRepository _friendRepository;
	  private IEventAggregator _eventAggregator;
	  private FriendWrapper _friend;

	  public FriendDetailViewModel(IFriendRepository friendRepository,
		  IEventAggregator eventAggregator)
	  {
		 _friendRepository = friendRepository;
		 _eventAggregator = eventAggregator;
		 SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
	  }

	  public async Task LoadAsync(int? friendId)
	  {
		 var friend = friendId.HasValue
			? await _friendRepository.GetByIdAsync(friendId.Value)
			: CreateNewFriend();
		 Friend = new FriendWrapper(friend);
		 Friend.PropertyChanged += (s, e) =>
		 {
			if (!HasChanges)
			{
			   HasChanges = _friendRepository.HasChanges();
			}
			if (e.PropertyName == nameof(Friend.HasErrors))
			{

			   ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

			}
		 };




	  }

	  private Friend CreateNewFriend()
	  {
		 var friend = new Friend();
		 _friendRepository.Add(friend);
		 return friend;
	  }

	  public FriendWrapper Friend
	  {
		 get { return _friend; }
		 private set
		 {
			_friend = value;
			OnPropertyChanged();
		 }
	  }



	  private bool _hasChanges;

	  public bool HasChanges
	  {
		 get { return _hasChanges; }
		 set
		 {
			_hasChanges = value;
			OnPropertyChanged();
			((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
		 }
	  }


	  public ICommand SaveCommand { get; }

	  private async void OnSaveExecute()
	  {
		  await _friendRepository.SaveAsync();
		  HasChanges = _friendRepository.HasChanges();
		 _eventAggregator.GetEvent<AfterFriendSaveEvent>().Publish(
			 new AfterFriendSaveEventArgs
			 {
				Id = Friend.Id,
				DisplayMember = $"{Friend.FirstName} {Friend.LastName}"
			 });
	  }
	  private bool OnSaveCanExecute()
	  {
		 //Todo: Check in additation if friend has changes
		 return Friend != null && !Friend.HasErrors && HasChanges;



	  }




   }
}
