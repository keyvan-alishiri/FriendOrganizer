using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
   public class NavigationItemViewModel :ViewModelBase
    {
        private string _DisplayMember;
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

        public NavigationItemViewModel(int id,string displayMember)
        {
            Id = id;
            DisplayMember = displayMember;
        }    
    }
}
