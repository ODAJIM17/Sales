

namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Sales.Common.Models;
    using Sales.Helpers;
    using Sales.Services;
    using System.Windows.Input;
    using Xamarin.Forms;
    public class AddProductViewModel : BaseViewModel
    {

        #region Attributes
        private ApiService apiService;
        public bool isRunning;
        public bool isEnabled;
        #endregion

        #region Properties
        public string Description { get; set; }
        public string Price { get; set; }
        public string Remarks { get; set; }
        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        #endregion

        #region Construtors

        public AddProductViewModel()
        {
            this.apiService = new ApiService();
            this.IsEnabled = true;

        }

        #endregion



        #region Commands
        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }
        }

        private async void Save()
        {
            if(string.IsNullOrEmpty(this.Description))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.DescriptionError, 
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.Price))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PriceError,
                    Languages.Accept);
                return;
            }

            var price = decimal.Parse(this.Price);
            if(price < 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                   Languages.Error,
                   Languages.PriceError,
                   Languages.Accept);
                return;
            }

            this.IsEnabled = false;
            this.IsRunning = true;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                return;
            }

            var product = new Product
            {
                Description = this.Description,
                Price = price,
                Remarks = this.Remarks,
            };

            //var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            //var response = await this.apiService.Post(
            //    apiSecurity,
            //    "/api",
            //    "/Users",
            //    user);


            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();
            var response = await this.apiService.Post(url, prefix, controller, product);

            // var response = await this.apiService.Post(url, "/api", "/Products", product);

            if (!response.IsSuccess)
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            var newProduct = (Product)response.Result;
            var productsViewModel = ProductsViewModel.GetInstance();
            productsViewModel.Products.Add(newProduct);

            this.IsEnabled = true;
            this.IsRunning = false;
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }

        #endregion

    }

