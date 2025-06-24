namespace task1.DTO
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Unit { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public int CategoryId { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public int? StockQuantity { get; set; }

    }
}
