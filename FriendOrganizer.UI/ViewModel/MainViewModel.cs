using Autofac.Features.Indexed;
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

	  private IIndex<string, IDetailViewModel> _detailViewModelCreator;
	  
	 
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
		IIndex<string,IDetailViewModel> detailViewModelCreator,
		 IEventAggregator eventAggregator,
		 IMessageDialogService messageDialogService )
	  {
		 _messageDialogService = messageDialogService;
		 _eventAggregator = eventAggregator;
		 NavigationViewModel = navigationViewModel;
		 _detailViewModelCreator = detailViewModelCreator;
		

		 _eventAggregator.GetEvent<OpenDetailViewEvent>()
			   .Subscribe(onOpenDetailView);
		 _eventAggregator.GetEvent<AfterDetailDeletedEvent>()
			.Subscribe(AfterDetailDeleted);
		 CreateNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetailExecute);


	  }

	  private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
	  {
		 DetailViewModel = null;
	  }

	  private void OnCreateNewDetailExecute(Type viewModelType)
	  {
		 onOpenDetailView(new OpenDetailViewEventArgs { ViewModelName = viewModelType.Name });
	  }

	  public async Task LoadAsync()
	  {
		 await NavigationViewModel.LoadAsync();

	  }

	   public ICommand CreateNewDetailCommand { get; }
	  private async void onOpenDetailView(OpenDetailViewEventArgs args)
	  {
		 if(DetailViewModel !=null && DetailViewModel.HasChanges)
		 {
			var result = _messageDialogService.ShowOkCancelDialog("شما تغییرات را ذخیره نکرده اید ", "Question");
			if(result == MessageDialogResult.Cancel)
			{
			   return;
			}
		 }
		 DetailViewModel = _detailViewModelCreator[args.ViewModelName];

		 await DetailViewModel.LoadAsync(args.Id);
		
		 
		 
		 
		 //switch (args.ViewModelName)
		 //{
		 //case nameof(FriendDetailViewModel):
		 //   DetailViewModel = _friendDetailViewModelCreator();
		 //   break;
		 //case nameof(MeetingDetailViewModel):
		 //   DetailViewModel = _meetingDetailViewModelCreator(); 
		 //   break;
		 //default:
		 //   throw new Exception($"ViewModel { args.ViewModelName} not mapped");

		 //}


	  }




   }
}
