using DryIoc;
using MaterialDesignThemes.Wpf;
using Prism.Dialogs;
using System;
using System.Windows;

namespace Osu.Music.Services.Dialogs
{
	public class PopupDialogService(IContainer container) : IPopupDialogService
	{
		public void ShowPopupDialog<TView, TViewModel>() where TView : FrameworkElement where TViewModel : IPopupDialogAware
		{
			var dialogView = container.Resolve<TView>();
			dialogView.DataContext = container.Resolve<TViewModel>();
			var viewModel = dialogView.DataContext as IPopupDialogAware;

			var closeHandler = new Action<IDialogResult>((e) =>
			{
				if (viewModel.CanCloseDialog())
				{
					DialogHost.CloseDialogCommand.Execute(e, null);
					viewModel.OnDialogClosed();
				}
			});

			viewModel.RequestClose += closeHandler;
			DialogHost.Show(dialogView);
		}

		public void ShowPopupDialog<TView, TViewModel>(string dialogHost) where TView : FrameworkElement where TViewModel : IPopupDialogAware
		{
			var dialogView = container.Resolve<TView>();
			dialogView.DataContext = container.Resolve<TViewModel>();
			var viewModel = dialogView.DataContext as IPopupDialogAware;

			var closeHandler = new Action<IDialogResult>((e) =>
			{
				if (viewModel.CanCloseDialog())
				{
					DialogHost.CloseDialogCommand.Execute(e, null);
					viewModel.OnDialogClosed();
				}
			});

			viewModel.RequestClose += closeHandler;
			DialogHost.Show(dialogView, dialogHost);
		}

		public void ShowPopupDialog<TView, TViewModel>(Action<IDialogResult> callBack) where TView : FrameworkElement where TViewModel : IPopupDialogAware
		{
			var dialogView = container.Resolve<TView>();
			dialogView.DataContext = container.Resolve<TViewModel>();
			var viewModel = dialogView.DataContext as IPopupDialogAware;

			var closeHandler = new Action<IDialogResult>((e) =>
			{
				if (viewModel.CanCloseDialog())
				{
					DialogHost.CloseDialogCommand.Execute(e, null);
					viewModel.OnDialogClosed();
					callBack?.Invoke(e);
				}
			});

			viewModel.RequestClose += closeHandler;
			DialogHost.Show(dialogView);
		}

		public void ShowPopupDialog<TView, TViewModel>(string dialogHost, Action<IDialogResult> callBack) where TView : FrameworkElement where TViewModel : IPopupDialogAware
		{
			var dialogView = container.Resolve<TView>();
			dialogView.DataContext = container.Resolve<TViewModel>();
			var viewModel = dialogView.DataContext as IPopupDialogAware;

			var closeHandler = new Action<IDialogResult>((e) =>
			{
				if (viewModel.CanCloseDialog())
				{
					DialogHost.CloseDialogCommand.Execute(e, null);
					viewModel.OnDialogClosed();
					callBack?.Invoke(e);
				}
			});

			viewModel.RequestClose += closeHandler;
			DialogHost.Show(dialogView, dialogHost);
		}

		public void ShowPopupDialog<TView, TViewModel>(IDialogParameters parameters, Action<IDialogResult> callBack) where TView : FrameworkElement where TViewModel : IPopupDialogAware
		{
			var dialogView = container.Resolve<TView>();
			dialogView.DataContext = container.Resolve<TViewModel>();
			var viewModel = dialogView.DataContext as IPopupDialogAware;

			var closeHandler = new Action<IDialogResult>((e) =>
			{
				if (viewModel.CanCloseDialog())
				{
					DialogHost.CloseDialogCommand.Execute(e, null);
					viewModel.OnDialogClosed();
					callBack?.Invoke(e);
				}
			});

			viewModel.RequestClose += closeHandler;
			DialogHost.Show(dialogView);
			viewModel.OnDialogOpened(parameters);
		}

		public void ShowPopupDialog<TView, TViewModel>(string dialogHost, IDialogParameters parameters, Action<IDialogResult> callBack) where TView : FrameworkElement where TViewModel : IPopupDialogAware
		{
			var dialogView = container.Resolve<TView>();
			dialogView.DataContext = container.Resolve<TViewModel>();
			var viewModel = dialogView.DataContext as IPopupDialogAware;

			var closeHandler = new Action<IDialogResult>((e) =>
			{
				if (viewModel.CanCloseDialog())
				{
					DialogHost.CloseDialogCommand.Execute(e, null);
					viewModel.OnDialogClosed();
					callBack?.Invoke(e);
				}
			});

			viewModel.RequestClose += closeHandler;
			DialogHost.Show(dialogView, dialogHost);
			viewModel.OnDialogOpened(parameters);
		}
	}
}
