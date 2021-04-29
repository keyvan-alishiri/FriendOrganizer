using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Event
{
    public class AfterDetailSaveEvent : PubSubEvent<AfterDetailSaveEventArgs>
    {
       
    }

    public class AfterDetailSaveEventArgs
    {
        public int Id { get; set; }
        public string DisplayMember { get; set; }
	    public string ViewModelName { get; set; }
   }
}
