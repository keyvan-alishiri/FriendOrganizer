using FriendOrganizer.Model;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.View.Services;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
  public class ProgrammingLanguageDetailViewModel: DetailViewModelBase

   {

     

      public ProgrammingLanguageDetailViewModel(IEventAggregator eventAggregator,
      IMessageDialogService messageDialogService,
      IProgrammingLanguageRepository programmingLanguageRepository)
      : base(eventAggregator, messageDialogService)
      {
         
         Title = "Programming Languages";
        
       
      }

	  public override Task LoadAsync(int id)
	  {
		 throw new NotImplementedException();
	  }

	  protected override void OnDeleteExecute()
	  {
		 throw new NotImplementedException();
	  }

	  protected override bool OnSaveCanExecute()
	  {
		 throw new NotImplementedException();
	  }

	  protected override void OnSaveExecute()
	  {
		 throw new NotImplementedException();
	  }
   }
}
