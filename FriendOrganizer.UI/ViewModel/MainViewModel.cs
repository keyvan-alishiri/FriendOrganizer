using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
   public class MainViewModel : ViewModelBase
   {


	  public INavigationViewModel NavigationViewModel { get; }

	  private Func<IFriendDetailViewModel> _friendDetailViewModelCreator;
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
		 IEventAggregator eventAggregator)
	  {
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
		 FriendDetailViewModel = _friendDetailViewModelCreator();
		 await FriendDetailViewModel.LoadAsync(friendId);
	  }




   }
}
