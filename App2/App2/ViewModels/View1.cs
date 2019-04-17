using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace App2.ViewModels
{
    public class View1 : BaseViewModel
    {
        public View1()
        {
            Title = "SnakesAndLadders";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
        }

        public ICommand OpenWebCommand { get; }
    }
}