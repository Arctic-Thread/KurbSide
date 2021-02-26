using System;
using System.Collections.Generic;

namespace KurbSide.Models
{
    public partial class Member
    {
        public string AspNetId { get; set; }
        public Guid MemberId { get; set; }

        public virtual AspNetUsers AspNet { get; set; }
    }
}
