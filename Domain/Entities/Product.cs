namespace Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double PriceUnit { get; set; }
        public int StockUnit { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
