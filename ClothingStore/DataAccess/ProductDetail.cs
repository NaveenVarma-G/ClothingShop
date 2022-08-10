using System;
using System.Collections.Generic;

namespace ClothingStore.DataAccess
{
    public partial class ProductDetail
    {
        public ProductDetail()
        {
            ProductInventories = new HashSet<ProductInventory>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string ProductType { get; set; } = null!;
        public string ProductDescription { get; set; } = null!;
        public double Price { get; set; }

        public virtual ICollection<ProductInventory> ProductInventories { get; set; }
    }
}
