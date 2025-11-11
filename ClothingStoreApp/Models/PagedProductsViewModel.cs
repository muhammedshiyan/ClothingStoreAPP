using ClothingStore.Core.Entities;

namespace ClothingStoreApp.Models
{
    public class PagedProductsViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
