using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Osu.Music.UI.Models
{
    public class MainModel : BindableBase
    {
        private string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public MainModel()
        {
            Message = "Model is set!";
        }
    }
}
