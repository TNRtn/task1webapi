using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace task1.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

}
