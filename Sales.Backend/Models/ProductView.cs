

namespace Sales.Backend.Models
{
    using Sales.Common.Models;
    using System.Web;

    public class ProductView : Product
    {
        public HttpPostedFileBase ImageFile { get; set; }
    }
}