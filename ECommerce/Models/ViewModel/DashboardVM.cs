namespace ECommerce.Models.ViewModel
{
    public class DashboardVM
    {
        public List<Category> CategoryInfo { get; set; }
        public List<ProductItem> ProductItem { get; set; }

        public int Count { get; set; }
        public string CountryName { get; set; }
    }
}
