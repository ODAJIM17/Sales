

namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using Sales.Common.Models;
    using Sales.Helpers;
    using Sales.Services;
    using Sales.Views;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class EditProductViewModel : BaseViewModel
    {
        #region Attributes

        private Product product;

        private MediaFile file;

        private ImageSource imageSource;

        private ApiService apiService;

        private bool isRunning;

        private bool isEnabled;

        #endregion

        #region Constructors
        public EditProductViewModel(Product product)
        {
            this.product = product;
            this.apiService = new ApiService();
            this.IsEnabled = true;
            this.ImageSource = product.ImageFullPath;
        }
        #endregion

        #region Properties
        public Product Product
        {
            get { return this.product; }
            set { this.SetValue(ref this.product, value); }
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }

        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { this.SetValue(ref this.imageSource, value); }
        }
        #endregion


        #region Commands

        //public ICommand EditProductCommand
        //{
        //    get
        //    {
        //        return new RelayCommand(EditProduct);
        //    }
        //}

        //private async void EditProduct()
        //{
        //    MainViewModel.GetInstance().EditProduct = new EditProductViewModel(this);
        //    await Application.Current.MainPage.Navigation.PushAsync(new EditProductPage());
           
        //}


        public ICommand DeleteCommand
        {
            get
            {
                return new RelayCommand(Delete);
            }
        }

        private async void Delete()
        {
            var answer = await Application.Current.MainPage.DisplayAlert(
                Languages.Confirm,
                Languages.DeleteConfirmation,
                Languages.Yes,
                Languages.No);
            if (!answer)
            {
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();
            var response = await this.apiService.Delete(url, prefix, controller, this.Product.ProductId,Settings.TokenType, Settings.AccessToken);
            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            var productsViewModel = ProductsViewModel.GetInstance();
            var deletedProduct = productsViewModel.MyProducts.Where(p => p.ProductId == this.Product.ProductId).FirstOrDefault();
            if (deletedProduct != null)
            {
                productsViewModel.MyProducts.Remove(deletedProduct);
            }

            productsViewModel.RefreshList();

            this.IsRunning = false;
            this.IsEnabled = true;
            await Application.Current.MainPage.Navigation.PopAsync();
            //await App.Navigator.PopAsync();
        }


        public ICommand ChangeImageCommand
        {
            get
            {
                return new RelayCommand(ChangeImage);
            }
        }

        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();

            var source = await Application.Current.MainPage.DisplayActionSheet(
                Languages.ImageSource,
                Languages.Cancel,
                null,
                Languages.FromGallery,
                Languages.NewPicture);

            if (source == Languages.Cancel)
            {
                this.file = null;
                return;
            }

            if (source == Languages.NewPicture)
            {
                this.file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.file != null)
            {
                this.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = this.file.GetStream();
                    return stream;
                });
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(this.Product.Description))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.DescriptionError,
                    Languages.Accept);
                return;
            }

           

          
            if (this.Product.Price < 0)
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

            byte[] imageArray = null;
            if (this.file != null)
            {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
                this.product.ImageArray = imageArray;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();
            var response = await this.apiService.Put(url, prefix, controller, this.product, this.product.ProductId,Settings.TokenType,Settings.AccessToken);

            // the code below also works when adding a new product. just uncomment the url  and the response vars.
            // var url = Application.Current.Resources["UrlAPI"].ToString();
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

            
            var oldProduct = productsViewModel.MyProducts.Where(p => p.ProductId == this.Product.ProductId).FirstOrDefault();
            if (oldProduct != null)
            {
                productsViewModel.MyProducts.Remove(oldProduct);
            }

            productsViewModel.MyProducts.Add(newProduct);
            productsViewModel.RefreshList();

            this.IsEnabled = true;
            this.IsRunning = false;
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }

    #endregion

}

