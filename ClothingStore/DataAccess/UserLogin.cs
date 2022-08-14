using System;
using System.Collections.Generic;

namespace ClothingStore.DataAccess
{
    public partial class UserLogin
    {
        public int CredentialId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; } = null!;

        public virtual UserDetail User { get; set; } = null!;
    }
}
