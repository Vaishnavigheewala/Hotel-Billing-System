namespace HotelBillingMVC.Models
{
    public class OrderViewModel
    {
        public int UserId { get; set; }
        public int TableId { get; set; }
        public string CustName { get; set; }
        public string PaymentType { get; set; }
        public List<OrderDetailViewModel> OrderDetails { get; set; }
    }
    public class OrderDetailViewModel
    {
        public int MenuId { get; set; }
        public int Quantity { get; set; }
        public MenuViewModel Menu { get; set; } // Ensure this property exists

    }

    public class MenuViewModel
    {
        public string ItemName { get; set; }
    }
}
