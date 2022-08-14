using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClothingStore.DataAccess
{
    public partial class UserDetail
    {
        public UserDetail()
        {
            OrderDetails = new HashSet<OrderDetail>();
            UserLogins = new HashSet<UserLogin>();
        }


        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int Type { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailId { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string PinCode { get; set; } = null!;

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<UserLogin> UserLogins { get; set; }
    }
}
