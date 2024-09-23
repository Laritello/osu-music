using Prism.Dialogs;
using System;

namespace Osu.Music.Services.Dialogs
{
	/// <summary>
	/// Provides a way for objects involved in Popup Dialogs to be notified of Dialog activities.
	/// </summary>
	public interface IPopupDialogAware
	{
		/// <summary>
		/// Evaluates whether the Dialog is in a state that would allow the Dialog to Close
		/// </summary>
		/// <returns><c>true</c> if the Dialog can close</returns>
		bool CanCloseDialog();

		/// <summary>
		/// Provides a callback to clean up resources or finalize tasks when the Dialog has been closed
		/// </summary>
		void OnDialogClosed();

		/// <summary>
		/// Initializes the state of the Dialog with provided DialogParameters
		/// </summary>
		/// <param name="parameters"></param>
		void OnDialogOpened(IDialogParameters parameters);

		event Action<IDialogResult> RequestClose;
	}
}
