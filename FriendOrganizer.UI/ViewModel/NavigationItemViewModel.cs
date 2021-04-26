using FriendOrganizer.UI.Event;
using Prism.Commands;
using Prism.Events;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
   public class NavigationItemViewModel :ViewModelBase
    {
	  private IEventAggregator _eventAggregator;

	  public int Id { get; }
        private string _displayMember;

        public string DisplayMember
        {
            get { return _displayMember; }
            set { 
                _displayMember = value;
                OnPropertyChanged();
            }
        }

        public NavigationItemViewModel(int id,string displayMember , IEventAggregator eventAggregator)
        {

         _eventAggregator = eventAggregator;
            Id = id;
            DisplayMember = displayMember; 
         OpenFriendDetailViewCommand = new DelegateCommand(OnOpenFriendDetailView);
        }

	  private void OnOpenFriendDetailView()
	  {
         _eventAggregator.GetEvent<OpenFriendDetailViewEvent>()
                     .Publish(Id);
      }

      public ICommand OpenFriendDetailViewCommand { get; }
    }
}
