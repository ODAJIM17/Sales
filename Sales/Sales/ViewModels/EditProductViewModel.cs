

namespace Sales.ViewModels
{
    using Plugin.Media.Abstractions;
    using Sales.Common.Models;
    using Sales.Services;
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

       
    }
}
