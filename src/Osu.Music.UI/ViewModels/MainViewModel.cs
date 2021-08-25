using Osu.Music.UI.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osu.Music.UI.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private MainModel _model;
        public MainModel Model
        {
            get => _model;
            set => SetProperty(ref _model, value);
        }

        public MainViewModel()
        {
            Model = new MainModel();
        }
    }
}
