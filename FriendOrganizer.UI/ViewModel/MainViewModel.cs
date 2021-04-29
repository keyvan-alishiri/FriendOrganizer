using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
   public class MainViewModel : ViewModelBase
   {


	  public INavigationViewModel NavigationViewModel { get; }

	  private Func<IFriendDetailViewModel> _friendDetailViewModelCreator;
	  private IMessageDialogService _messageDialogService;
	  private IEventAggregator _eventAggregator;

	  private IDetailViewModel _detailViewModel;

	  public IDetailViewModel DetailViewModel
	  {
		 get { return _detailViewModel; }
		 private  set { _detailViewModel = value;
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
		 _eventAggregator.GetEvent<AfterFriendDeleteEvent>()
			.Subscribe(AfterFriendDeleted);
		 CreateNewFriendCommand = new DelegateCommand(OnCreateNewFriendExecute);


	  }

	  private void AfterFriendDeleted(int friendId)
	  {
		 DetailViewModel = null;
	  }

	  private void OnCreateNewFriendExecute()
	  {
		 onOpenFriendDetailView(null);
	  }

	  public async Task LoadAsync()
	  {
		 await NavigationViewModel.LoadAsync();

	  }

	   public ICommand CreateNewFriendCommand { get; }
	  private async void onOpenFriendDetailView(int? friendId)
	  {
		 if(DetailViewModel !=null && DetailViewModel.HasChanges)
		 {
			var result = _messageDialogService.ShowOkCancelDialog("شما تغییرات را ذخیره نکرده اید ", "Question");
			if(result == MessageDialogResult.Cancel)
			{
			   return;
			}
		 }
		 DetailViewModel = _friendDetailViewModelCreator();
		 await DetailViewModel.LoadAsync(friendId);
	  }




   }
}
