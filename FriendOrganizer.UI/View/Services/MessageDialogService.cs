﻿
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FriendOrganizer.UI.View.Services
{
   public class MessageDialogService : IMessageDialogService
   {
	  private MetroWindow MetroWindow => (MetroWindow)App.Current.MainWindow;

	  public MessageDialogResult ShowOkCancelDialog(string text, string title)
	  {
		 var result = MessageBox.Show(text, title, MessageBoxButton.OKCancel);
		 return result == MessageBoxResult.OK ? MessageDialogResult.Ok : MessageDialogResult.Cancel;
	  }

      public async Task<MessageDialogResult> ShowOkCancelDialogAsync(string text, string title)
      {
         var result =
           await MetroWindow.ShowMessageAsync(title, text, MessageDialogStyle.AffirmativeAndNegative);

         return result == MahApps.Metro.Controls.Dialogs.MessageDialogResult.Affirmative
           ? MessageDialogResult.Ok
           : MessageDialogResult.Cancel;
      }
      public async Task ShowInfoDialogAsync(string text)
      {
         await MetroWindow.ShowMessageAsync("Info", text);
      }

   }

   public enum MessageDialogResult
   {
	  Ok,
	  Cancel,
   }
}
