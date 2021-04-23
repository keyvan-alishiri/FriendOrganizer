﻿using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Event
{
    public class AfterFriendSaveEvent : PubSubEvent<AfterFriendSaveEventArgs>
    {
       
    }

    public class AfterFriendSaveEventArgs
    {
        public int Id { get; set; }
        public string DisplayMember { get; set; }
    }
}
