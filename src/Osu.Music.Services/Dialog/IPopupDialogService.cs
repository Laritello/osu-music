using Prism.Services.Dialogs;
using System;
using System.Windows;

namespace Osu.Music.Services.Dialog
{
    public interface IPopupDialogService
    {
        /// <summary>
        /// Shows the "materialDesign Popup Dialog" using the IDialogAware interface from Prism.
        /// Without ViewModelLocator, Without registering 
        /// 
        ///  => Uses the RootDialogHost
        /// </summary>
        /// <typeparam name="TView">View (normaly a UserControl)</typeparam>
        /// <typeparam name="TViewModel">ViewModel (DataContext)</typeparam>
        void ShowPopupDialog<TView, TViewModel>() where TView : FrameworkElement where TViewModel : IDialogAware;

        /// <summary>
        /// Shows the "materialDesign Popup Dialog" using the IDialogAware interface from Prism.
        /// Without ViewModelLocator, Without registering 
        /// </summary>
        /// <typeparam name="TView">View (normaly a UserControl)</typeparam>
        /// <typeparam name="TViewModel">ViewModel (DataContext)</typeparam>
        /// <param name="dialogHost">The dialogidentifier from materialDesign</param>
        void ShowPopupDialog<TView, TViewModel>(string dialogHost) where TView : FrameworkElement where TViewModel : IDialogAware;

        /// <summary>
        /// Shows the "materialDesign Popup Dialog" using the IDialogAware interface from Prism.
        /// Without ViewModelLocator, Without registering 
        /// 
        ///  => Uses the RootDialogHost
        /// </summary>
        /// <typeparam name="TView">View (normaly a UserControl)</typeparam>
        /// <typeparam name="TViewModel">ViewModel (DataContext)</typeparam>
        /// <param name="callBack">callBackAction like normal dialog in Prism</param>
        void ShowPopupDialog<TView, TViewModel>(Action<IDialogResult> callBack) where TView : FrameworkElement where TViewModel : IDialogAware;

        /// <summary>
        /// Shows the "materialDesign Popup Dialog" using the IDialogAware interface from Prism.
        /// Without ViewModelLocator, Without registering 
        /// </summary>
        /// <typeparam name="TView">View (normaly a UserControl)</typeparam>
        /// <typeparam name="TViewModel">ViewModel (DataContext)</typeparam>
        /// <param name="dialogHost">The dialogidentifier from materialDesign</param>
        /// <param name="callBack">callBackAction like normal dialog in Prism</param>
        void ShowPopupDialog<TView, TViewModel>(string dialogHost, Action<IDialogResult> callBack) where TView : FrameworkElement where TViewModel : IDialogAware;

        /// <summary>
        /// Shows the "materialDesign Popup Dialog" using the IDialogAware interface from Prism.
        /// Without ViewModelLocator, Without registering 
        /// 
        ///  => Uses the RootDialogHost
        /// </summary>
        /// <typeparam name="TView">View (normaly a UserControl)</typeparam>
        /// <typeparam name="TViewModel">ViewModel (DataContext)</typeparam>
        /// <param name="parameters">dialogParameters like normal dialog in Prism</param>
        /// <param name="callBack">callBackAction like normal dialog in Prism</param>
        void ShowPopupDialog<TView, TViewModel>(IDialogParameters parameters, Action<IDialogResult> callBack) where TView : FrameworkElement where TViewModel : IDialogAware;

        /// <summary>
        /// Shows the "materialDesign Popup Dialog" using the IDialogAware interface from Prism.
        /// Without ViewModelLocator, Without registering 
        /// </summary>
        /// <typeparam name="TView">View (normaly a UserControl)</typeparam>
        /// <typeparam name="TViewModel">ViewModel (DataContext)</typeparam>
        /// <param name="dialogHost">The dialogidentifier from materialDesign</param>
        /// <param name="parameters">dialogParameters like normal dialog in Prism</param>
        /// <param name="callBack">callBackAction like normal dialog in Prism</param>
        void ShowPopupDialog<TView, TViewModel>(string dialogHost, IDialogParameters parameters, Action<IDialogResult> callBack) where TView : FrameworkElement where TViewModel : IDialogAware;
    }
}
