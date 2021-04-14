using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace KurbSide.Models
{
    public partial class Notification
    {
        public Guid NotificationId { get; set; }
        public string SenderId { get; set; }
        public string RecipientId { get; set; }
        public DateTime NotificationDateTime { get; set; }
        public string NotificationDetails { get; set; }
        public bool Read { get; set; }
        public Guid? SaleId { get; set; }
        public Guid? OrderId { get; set; }

        public virtual Order Order { get; set; }
        public virtual AspNetUsers Recipient { get; set; }
        public virtual Sale Sale { get; set; }
        public virtual AspNetUsers Sender { get; set; }
    }
}
