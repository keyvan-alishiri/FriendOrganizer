﻿using Autofac.Features.Indexed;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
   public class MainViewModel : ViewModelBase
   {


	  public INavigationViewModel NavigationViewModel { get; }
	  public ObservableCollection<IDetailViewModel> DetailViewModels { get;}

	  private IIndex<string, IDetailViewModel> _detailViewModelCreator;
      private IMessageDialogService _messageDialogService;
	  private IEventAggregator _eventAggregator;
	  public ICommand CreateNewDetailCommand { get; }
	  private IDetailViewModel _selectedDetailViewModel;

	  public IDetailViewModel SelectedDetailViewModel
	  {
		 get { return _selectedDetailViewModel; }
		 set
		 {
			_selectedDetailViewModel = value;
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
		 DetailViewModels = new ObservableCollection<IDetailViewModel>();


		 _eventAggregator.GetEvent<OpenDetailViewEvent>()
			   .Subscribe(OnOpenDetailView);
		 _eventAggregator.GetEvent<AfterDetailDeletedEvent>()
			.Subscribe(AfterDetailDeleted);
		 CreateNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetailExecute);


	  }

	  private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
	  {
		 var detailViewModel = DetailViewModels.SingleOrDefault(vm => vm.Id == args.Id && vm.GetType().Name == args.ViewModelName);
		 if (detailViewModel != null)
		 {
			DetailViewModels.Remove(detailViewModel);
		 }


	  }

	  private async void OnOpenDetailView(OpenDetailViewEventArgs args)
	  {
		 var detailViewModel = DetailViewModels
		.SingleOrDefault(vm => vm.Id == args.Id
		&& vm.GetType().Name == args.ViewModelName);

		 if (detailViewModel == null)
		 {

			detailViewModel = _detailViewModelCreator[args.ViewModelName];


			await detailViewModel.LoadAsync(args.Id);

			DetailViewModels.Add(detailViewModel);
		 }

		 SelectedDetailViewModel = detailViewModel;

	  }

	  private void OnCreateNewDetailExecute(Type viewModelType)
	  {
		 OnOpenDetailView(new OpenDetailViewEventArgs { ViewModelName = viewModelType.Name });
	  }

	  public async Task LoadAsync()
	  {
		 await NavigationViewModel.LoadAsync();

	  }


	  //private async void onOpenDetailView(OpenDetailViewEventArgs args)
	  //{
		 //if (SelectedDetailViewModel != null && SelectedDetailViewModel.HasChanges)
		 //{
			//var result = _messageDialogService.ShowOkCancelDialog("شما تغییرات را ذخیره نکرده اید ", "Question");
			//if (result == MessageDialogResult.Cancel)
			//{
			//   return;
			//}
		 //}
		 //SelectedDetailViewModel = _detailViewModelCreator[args.ViewModelName];

		 //await SelectedDetailViewModel.LoadAsync(args.Id);




		 ////switch (args.ViewModelName)
		 ////{
		 ////case nameof(FriendDetailViewModel):
		 ////   DetailViewModel = _friendDetailViewModelCreator();
		 ////   break;
		 ////case nameof(MeetingDetailViewModel):
		 ////   DetailViewModel = _meetingDetailViewModelCreator(); 
		 ////   break;
		 ////default:
		 ////   throw new Exception($"ViewModel { args.ViewModelName} not mapped");

		 ////}


	  //}




   }
}
