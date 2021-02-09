using System;
using System.Collections.Generic;

namespace KurbSide.Models
{
    public partial class Business
    {
        public string AspNetId { get; set; }
        public Guid BusinessId { get; set; }

        public virtual AspNetUsers AspNet { get; set; }
    }
}
