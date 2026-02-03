namespace FactoryApp.Models // Убедитесь, что namespace совпадает с названием вашего проекта
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public decimal OrderDate { get; set; }
        public int TotalAmount { get; set; }
        
    }
}