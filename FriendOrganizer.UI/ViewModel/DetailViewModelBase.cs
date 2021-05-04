
using Prism.Commands;
using Prism.Events;
using System.Threading.Tasks;
using System.Windows.Input;
using FriendOrganizer.UI.Event;

namespace FriendOrganizer.UI.ViewModel
{
   public abstract class DetailViewModelBase : ViewModelBase, IDetailViewModel
   {

	  private bool _hasChanges;
	  protected readonly IEventAggregator EventAggregator;
	  private int _id;

	  public ICommand SaveCommand { get; private set; }
	  public ICommand DeleteCommand { get; private set; }

	 

	  public int Id
	  {
		 get { return _id; }
		protected set { _id = value; }
	  }

	  public DetailViewModelBase(IEventAggregator eventAggregator)
	  {
		 EventAggregator = eventAggregator;
		 SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
		 DeleteCommand = new DelegateCommand(OnDeleteExecute);
	  }

	  protected abstract void OnDeleteExecute();


	  protected abstract bool OnSaveCanExecute();


	  protected abstract void OnSaveExecute();

	  protected virtual void RaiseDetailDeletedEvent(int modelId)
	  {
		 EventAggregator.GetEvent<AfterDetailDeletedEvent>().Publish(new
			AfterDetailDeletedEventArgs
		 {
			Id = modelId,
			ViewModelName = this.GetType().Name
		 });
	  }

	
	  protected virtual void RaiseDetailSavedEvent(int modelId, string displayMember)
	  {
		 EventAggregator.GetEvent<AfterDetailSaveEvent>().Publish(new AfterDetailSaveEventArgs
		 {
			Id = modelId,
			DisplayMember = displayMember,
			ViewModelName = this.GetType().Name
		 });
	  }


	  public bool HasChanges
	  {
		 get { return _hasChanges; }
		 set
		 {
			if(_hasChanges != value)
			{
			   _hasChanges = value;
			   OnPropertyChanged();
			   ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
			}
		 }
	  }

	  public abstract Task LoadAsync(int? id);
	 
   }
}
