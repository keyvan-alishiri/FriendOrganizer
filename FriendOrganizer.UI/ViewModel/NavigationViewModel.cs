using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Data.Lookups;
using FriendOrganizer.UI.Event;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private IFriendLookupDataService _friendLookupDataService;
        private readonly IEventAggregator _eventAggregator;

        public NavigationViewModel(IFriendLookupDataService friendLookupDataService , IEventAggregator eventAggregator)
        {
            _friendLookupDataService = friendLookupDataService;
            _eventAggregator = eventAggregator;
            Friends = new ObservableCollection<NavigationItemViewModel>();
            _eventAggregator.GetEvent<AfterFriendSaveEvent>().Subscribe(AfterFriendSave);
            _eventAggregator.GetEvent<AfterFriendDeleteEvent>().Subscribe(AfterFriendDeleted);
          
      }

      private void AfterFriendDeleted(int friendId)
      {
         var friend = Friends.SingleOrDefault(f => f.Id == friendId);
         if(friend!=null)
		 {
            Friends.Remove(friend);
		 }
      }

	  private void AfterFriendSave(AfterFriendSaveEventArgs obj)
        {
            var lookupItem = Friends.SingleOrDefault(l => l.Id == obj.Id);
         if(lookupItem == null)
		 {
            Friends.Add(new NavigationItemViewModel(obj.Id,obj.DisplayMember,_eventAggregator));
		 }
         else
		 {
            lookupItem.DisplayMember = obj.DisplayMember;
         }
            
        }

        public async Task LoadAsync()
        {
            var lookup = await _friendLookupDataService.GetFriendLookupAsync();
            Friends.Clear();
            foreach (var item in lookup)
            {
                Friends.Add(new NavigationItemViewModel(item.Id, item.DisplayMember,_eventAggregator));
            }
        }

        public ObservableCollection<NavigationItemViewModel> Friends { get; }

        //private NavigationItemViewModel _selectedFriend;

        //public NavigationItemViewModel SelectedFriend
        //{
        //    get { return _selectedFriend; }
        //    set {
        //        _selectedFriend = value;
        //        OnPropertyChanged();
        //        if(_selectedFriend!=null)
        //        {
        //            _eventAggregator.GetEvent<OpenFriendDetailViewEvent>()
        //                .Publish(_selectedFriend.Id);
        //        }
        //    }
        //}

    }
}
