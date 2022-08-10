using System;
using System.Collections.Generic;

namespace ClothingStore.DataAccess
{
    public partial class CartItem
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int ProductCount { get; set; }

        public virtual OrderDetail Order { get; set; } = null!;
        public virtual ProductDetail Product { get; set; } = null!;
    }
}
