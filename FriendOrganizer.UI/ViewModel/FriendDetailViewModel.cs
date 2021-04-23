using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        private IFriendDataService _dataService;
        private  IEventAggregator _eventAggregator;
        private FriendWrapper _friend;

        public FriendDetailViewModel(IFriendDataService dataService,
            IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<OpenFriendDetailViewEvent>()
                .Subscribe(onOpenFriendDetailView);
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
        }

        public async Task LoadAsync(int friendId)
        {
            var friend = await _dataService.GetByIdAsync(friendId);
            Friend = new FriendWrapper(friend);
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

        public ICommand SaveCommand { get; }
       
        private async void OnSaveExecute()
        {
            await _dataService.SaveAsync(Friend.Model);
            _eventAggregator.GetEvent<AfterFriendSaveEvent>().Publish(
                new AfterFriendSaveEventArgs
            {
                Id = Friend.Id,
                DisplayMember = $"{Friend.FirstName} {Friend.LastName}"
            });
        }
        private bool OnSaveCanExecute()
        {
            //Todo: Check if friend is valid
            return true;
        }

        private async  void onOpenFriendDetailView(int friendId)
        {
            await LoadAsync(friendId);
        }

       
    }
}
