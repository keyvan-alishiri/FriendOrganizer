using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
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
	  private IMessageDialogService _messageDialogService;
	  private FriendWrapper _friend;

	  public FriendDetailViewModel(IFriendRepository friendRepository,
		  IEventAggregator eventAggregator,
		  IMessageDialogService messageDialogService)
	  {
		 _friendRepository = friendRepository;
		 _eventAggregator = eventAggregator;
		 _messageDialogService = messageDialogService;
		 SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
		 DeleteCommand = new DelegateCommand(OnDeleteExecute);
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

		 if (Friend.Id == 0)
		 {
			//Little trick to trigger the validation
			Friend.FirstName = "";
		 }


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
	  public ICommand DeleteCommand { get; }

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

	  private async void OnDeleteExecute()
	  {
		 var result = _messageDialogService.ShowOkCancelDialog($"آیا می خواهید  اطلاعات {Friend.FirstName} {Friend.LastName} را حذف کنید؟","اخطار حذف");
		 if(result == MessageDialogResult.Ok)
		 {
			_friendRepository.Remove(Friend.Model);
			await _friendRepository.SaveAsync();
			_eventAggregator.GetEvent<AfterFriendDeleteEvent>().Publish(Friend.Id);
		 }
		
	  }




   }
}
