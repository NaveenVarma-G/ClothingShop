using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClothingStore.DataAccess
{
    public partial class ProductDetail
    {
        public ProductDetail()
        {
            ProductInventories = new HashSet<ProductInventory>();
        }

        public ProductDetail(int productId, string productName, string productType, string productDescription, double price, int count)
        {
            ProductId = productId;
            ProductName = productName;
            ProductType = productType;
            ProductDescription = productDescription;
            Price = price;
            Count = count;
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string ProductType { get; set; } = null!;
        public string ProductDescription { get; set; } = null!;
        public double Price { get; set; }

        [NotMapped]
        public int Count { get; set; }

        public virtual ICollection<ProductInventory> ProductInventories { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ProductDetail detail &&
                   ProductId == detail.ProductId &&
                   ProductName == detail.ProductName &&
                   ProductType == detail.ProductType &&
                   ProductDescription == detail.ProductDescription &&
                   Price == detail.Price &&
                   Count == detail.Count;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ProductId, ProductName, ProductType, ProductDescription, Price, Count);
        }
    }
}
