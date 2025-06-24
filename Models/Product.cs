using System;
using System.Collections.Generic;

namespace task1.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public int CategoryId { get; set; }

    public decimal? Price { get; set; }

    public int? StockQuantity { get; set; }

    public string Unit { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }
    public virtual Category? Category { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
