using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using Prism.Events;
using System;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
   public class MainViewModel : ViewModelBase
   {


	  public INavigationViewModel NavigationViewModel { get; }

	  private Func<IFriendDetailViewModel> _friendDetailViewModelCreator;
	  private IMessageDialogService _messageDialogService;
	  private IEventAggregator _eventAggregator;

	  private IFriendDetailViewModel _friendDetailViewModel;

	  public IFriendDetailViewModel FriendDetailViewModel
	  {
		 get { return _friendDetailViewModel; }
		 private  set { _friendDetailViewModel = value;
			OnPropertyChanged();
		 }
	  }

	  public MainViewModel(INavigationViewModel navigationViewModel,
		Func<IFriendDetailViewModel> friendDetailViewModelCreator,
		 IEventAggregator eventAggregator,
		 IMessageDialogService messageDialogService )
	  {
		 _messageDialogService = messageDialogService;
		 _eventAggregator = eventAggregator;
		 NavigationViewModel = navigationViewModel;
		 _friendDetailViewModelCreator = friendDetailViewModelCreator;

		 _eventAggregator.GetEvent<OpenFriendDetailViewEvent>()
			   .Subscribe(onOpenFriendDetailView);
		
	  }


	  public async Task LoadAsync()
	  {
		 await NavigationViewModel.LoadAsync();

	  }


	  private async void onOpenFriendDetailView(int friendId)
	  {
		 if(FriendDetailViewModel !=null && FriendDetailViewModel.HasChanges)
		 {
			var result = _messageDialogService.ShowOkCancelDialg("شما تغییرات را ذخیره نکرده اید ", "Question");
			if(result == MessageDialogResult.Cancel)
			{
			   return;
			}
		 }
		 FriendDetailViewModel = _friendDetailViewModelCreator();
		 await FriendDetailViewModel.LoadAsync(friendId);
	  }




   }
}
