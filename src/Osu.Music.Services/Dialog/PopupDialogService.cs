using MaterialDesignThemes.Wpf;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System;
using System.Windows;

namespace Osu.Music.Services.Dialog
{
    public class PopupDialogService : IPopupDialogService
    {
        private readonly IContainerExtension _container;

        public PopupDialogService(IContainerExtension container)
        {
            _container = container;
        }

        public void ShowPopupDialog<TView, TViewModel>() where TView : FrameworkElement where TViewModel : IDialogAware
        {
            var dialogView = _container.Resolve<TView>();
            dialogView.DataContext = _container.Resolve<TViewModel>();
            var viewModel = dialogView.DataContext as IDialogAware;

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

        public void ShowPopupDialog<TView, TViewModel>(string dialogHost) where TView : FrameworkElement where TViewModel : IDialogAware
        {
            var dialogView = _container.Resolve<TView>();
            dialogView.DataContext = _container.Resolve<TViewModel>();
            var viewModel = dialogView.DataContext as IDialogAware;

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

        public void ShowPopupDialog<TView, TViewModel>(Action<IDialogResult> callBack) where TView : FrameworkElement where TViewModel : IDialogAware
        {
            var dialogView = _container.Resolve<TView>();
            dialogView.DataContext = _container.Resolve<TViewModel>();
            var viewModel = dialogView.DataContext as IDialogAware;

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

        public void ShowPopupDialog<TView, TViewModel>(string dialogHost, Action<IDialogResult> callBack) where TView : FrameworkElement where TViewModel : IDialogAware
        {
            var dialogView = _container.Resolve<TView>();
            dialogView.DataContext = _container.Resolve<TViewModel>();
            var viewModel = dialogView.DataContext as IDialogAware;

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

        public void ShowPopupDialog<TView, TViewModel>(IDialogParameters parameters, Action<IDialogResult> callBack) where TView : FrameworkElement where TViewModel : IDialogAware
        {
            var dialogView = _container.Resolve<TView>();
            dialogView.DataContext = _container.Resolve<TViewModel>();
            var viewModel = dialogView.DataContext as IDialogAware;

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

        public void ShowPopupDialog<TView, TViewModel>(string dialogHost, IDialogParameters parameters, Action<IDialogResult> callBack) where TView : FrameworkElement where TViewModel : IDialogAware
        {
            var dialogView = _container.Resolve<TView>();
            dialogView.DataContext = _container.Resolve<TViewModel>();
            var viewModel = dialogView.DataContext as IDialogAware;

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
