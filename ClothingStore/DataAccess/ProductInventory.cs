using System;
using System.Collections.Generic;

namespace ClothingStore.DataAccess
{
    public partial class ProductInventory
    {
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public int InventoryCount { get; set; }
        public double? Discount { get; set; }

        public virtual ProductDetail Product { get; set; } = null!;
    }
}
