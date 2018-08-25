

namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Sales.Views;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class MainViewModel
    {

        public ProductsViewModel Products { get; set; }
        public AddProductViewModel AddProduct { get; set; }

        public MainViewModel()
        {
            this.Products = new ProductsViewModel();
        }

        public ICommand AddProductCommand
        {
            get
            {
                return new RelayCommand(GoToAddProduct);
            }
        }

        private async void GoToAddProduct()
        {
            this.AddProduct = new AddProductViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new AddProductPage());
        }
    }
}
