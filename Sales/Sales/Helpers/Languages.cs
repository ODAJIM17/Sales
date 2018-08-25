

namespace Sales.Helpers
{
    using Sales.Interfaces;
    using Sales.Resources;
    using Xamarin.Forms;
    public static class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Accept
        {
            get { return Resource.Accept; }
        }

        public static string Error
        {
            get { return Resource.Error; }
        }

        public static string InternetSettings
        {
            get { return Resource.InternetSettings; }
        }

        public static string NoInternet
        {
            get { return Resource.NoInternet; }
        }

        public static string Products
        {
            get { return Resource.Products; }
        }

        public static string AddProduct
        {
            get { return Resource.AddProduct; }
        }

        public static string Description
        {
            get { return Resource.Description; }
        }

        public static string DescriptionPlaceholder
        {
            get { return Resource.DescriptionPlaceholder; }
        }

        public static string Price
        {
            get { return Resource.Price; }
        }

        public static string PricePlaceholder
        {
            get { return Resource.PricePlaceholder; }
        }

        public static string Remarks
        {
            get { return Resource.Remarks; }
        }

        public static string DescriptionError
        {
            get { return Resource.DescriptionErrror; }
        }

        public static string PriceError
        {
            get { return Resource.PriceError; }
        }

        public static string ChangeImage
        {
            get { return Resource.ChangeImage; }
        }
    }
}