namespace FactoryApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string OrderDate { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
    }
}