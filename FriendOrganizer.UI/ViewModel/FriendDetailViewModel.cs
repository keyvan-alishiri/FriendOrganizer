﻿using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Data.Lookups;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using FriendOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
   public class FriendDetailViewModel : DetailViewModelBase, IFriendDetailViewModel
   {
	  private IFriendRepository _friendRepository;
	
	 
	  private IProgrammingLanguageLookupDataService _programmingLanguageLookupDataService;
	  private FriendWrapper _friend;
	  private FriendPhoneNumberWrapper _selectedPhoneNumber;

	  public FriendDetailViewModel(IFriendRepository friendRepository,
		  IEventAggregator eventAggregator,
		  IMessageDialogService messageDialogService,
		  IProgrammingLanguageLookupDataService programmingLanguageLookupDataService)
		 :base(eventAggregator,messageDialogService)
	  {
		 _friendRepository = friendRepository;
		
	
		 _programmingLanguageLookupDataService = programmingLanguageLookupDataService; 
		
		 AddPhoneNumberCommand = new DelegateCommand(OnAddPhoneNumberExecute);
		 RemovePhoneNumberCommand = new DelegateCommand(OnRemovePhoneNumberExecute, OnRemovePhoneNumberCanExecute);

         ProgrammingLanguages = new ObservableCollection<LookupItem>();
		 PhoneNumbers = new ObservableCollection<FriendPhoneNumberWrapper>();
	  }

	  private bool OnRemovePhoneNumberCanExecute()
	  {
		 return SelectedPhoneNumber != null;
	  }

	  private void OnRemovePhoneNumberExecute()
	  {
		 SelectedPhoneNumber.PropertyChanged -= FriendPhoneNumberWrapper_PropertyChanged;
		 _friendRepository.RemovePhoneNumber(SelectedPhoneNumber.Model);
		 PhoneNumbers.Remove(SelectedPhoneNumber);
		 SelectedPhoneNumber = null;
		 HasChanges = _friendRepository.HasChanges();
		 ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
	  }

	  private void OnAddPhoneNumberExecute()
	  {
		 var newNumber = new FriendPhoneNumberWrapper(new FriendPhoneNumber());
		 newNumber.PropertyChanged += FriendPhoneNumberWrapper_PropertyChanged;
		 PhoneNumbers.Add(newNumber);
		 Friend.Model.PhoneNumbers.Add(newNumber.Model);
		 newNumber.Number = ""; //Triger validation 
	  }

	  public override async Task LoadAsync(int? friendId)
	  {
		 var friend = friendId.HasValue
			? await _friendRepository.GetByIdAsync(friendId.Value)
			: CreateNewFriend();
		 Id = friend.Id;

		 InitializeFriend(friend);
		 InitializeFriendPhoneNumbers(friend.PhoneNumbers);
		 await LoadProgrammingLanguagesLookupAsync();
	  }

	  private void InitializeFriendPhoneNumbers(ICollection<FriendPhoneNumber> phoneNumbers)
	  {
		 foreach (var wrapper in PhoneNumbers)
		 {
			wrapper.PropertyChanged -= FriendPhoneNumberWrapper_PropertyChanged;
		 }
		 PhoneNumbers.Clear();
		 foreach (var friendPhoneNumber in phoneNumbers )
		 {
			var wrapper = new FriendPhoneNumberWrapper(friendPhoneNumber);
			PhoneNumbers.Add(wrapper);
			wrapper.PropertyChanged += FriendPhoneNumberWrapper_PropertyChanged;
		 }
	  }

	  private void FriendPhoneNumberWrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
	  {
		if(!HasChanges)
		 {
			HasChanges = _friendRepository.HasChanges();
		 }
		if(e.PropertyName == nameof(FriendPhoneNumberWrapper.HasErrors))
		 {
			((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
		 }
	  }

	  private void InitializeFriend(Friend friend)
	  {
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
			if (e.PropertyName == nameof(Friend.FirstName) || e.PropertyName == nameof(Friend.LastName))
			{
			   SetTitle();
			}
			
		 };

		 ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

		 if (Friend.Id == 0)
		 {
			//Little trick to trigger the validation
			Friend.FirstName = "";
		 }
		 SetTitle();
	  }

	  private void SetTitle()
	  {
		 Title = $"{Friend.FirstName} {Friend.LastName}";
	  }

	  private async Task LoadProgrammingLanguagesLookupAsync()
	  {
		 
		 ProgrammingLanguages.Clear();
		 ProgrammingLanguages.Add(new NullLookupItem { DisplayMember= " - "});
		 var lookup = await _programmingLanguageLookupDataService.GetProgrammingLanguageAsync();
		 foreach (var lookupItem in lookup)
		 {
			ProgrammingLanguages.Add(lookupItem);
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

	 

	  public FriendPhoneNumberWrapper SelectedPhoneNumber
	  {
		 get { return _selectedPhoneNumber; }
		 set {
			
			_selectedPhoneNumber =value;
			OnPropertyChanged();
			((DelegateCommand)RemovePhoneNumberCommand).RaiseCanExecuteChanged();
		 }
	  }




	

	  public ICommand AddPhoneNumberCommand { get;  }
	  public ICommand RemovePhoneNumberCommand { get;  }


	  public ObservableCollection<LookupItem> ProgrammingLanguages { get; }
	  public ObservableCollection<FriendPhoneNumberWrapper> PhoneNumbers { get; }

	  protected override async void OnSaveExecute()
	  {
		 await _friendRepository.SaveAsync();
		 HasChanges = _friendRepository.HasChanges();
		 Id = Friend.Id;

		 RaiseDetailSavedEvent(Friend.Id, $"{Friend.FirstName} {Friend.LastName}");
		 
	  }
	  protected override bool OnSaveCanExecute()
	  {
		 //Todo: Check in additation if friend has changes
		 return Friend != null
			&& !Friend.HasErrors 
			&& PhoneNumbers.All(pn => !pn.HasErrors)
			&& HasChanges;


	  }

	  protected override async void OnDeleteExecute()
	  {
		 if(await _friendRepository.HasMeetingsAsync(Friend.Id))
		 {
			MessageDialogService.ShowOkCancelDialog($" نمی توانید این ملاقات را حذف کنید چرا که {Friend.FirstName} {Friend.LastName}در لیست ملاقات ها قرار دارد", "اخطار حذف");
			return;
		 }
		 var result = MessageDialogService.ShowOkCancelDialog($"آیا می خواهید  اطلاعات {Friend.FirstName} {Friend.LastName} را حذف کنید؟","اخطار حذف");
		 if(result == MessageDialogResult.Ok)
		 {
			_friendRepository.Remove(Friend.Model);
			await _friendRepository.SaveAsync();
			RaiseDetailDeletedEvent(Friend.Id);
			
		 }
		
	  }




   }
}
