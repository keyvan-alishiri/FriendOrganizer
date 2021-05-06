using System.Threading.Tasks;

namespace FriendOrganizer.UI.View.Services
{
   public interface IMessageDialogService
   {
	  MessageDialogResult ShowOkCancelDialog(string text, string title);
	  Task<MessageDialogResult> ShowOkCancelDialogAsync(string text, string title);
	  Task ShowInfoDialogAsync(string text);
	  
   }
}