using System;
using System.Collections.Generic;

namespace ClothingStore.DataAccess
{
    public partial class OrderDetail
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int DeliveryType { get; set; }
        public int? PaymentType { get; set; }

        public virtual UserDetail User { get; set; } = null!;
    }
}
