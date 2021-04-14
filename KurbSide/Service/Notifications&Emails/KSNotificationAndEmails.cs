namespace KurbSide.Service
{
    public static class KSNotificationAndEmails
    {
        /// <summary>
        /// Creates a message depending on the orderStatus for later use.
        /// <br/>
        /// <code>Example: CreateMessage(orderStatus, businessName)</code>
        /// </summary>
        /// <remarks>
        /// Liam De Rivers
        /// </remarks>
        /// <param name="orderStatus"></param>
        /// <param name="businessName"></param>
        /// <returns>A message curated for the entered orderStatus</returns>
        public static string CreateMessage(OrderStatus orderStatus, string businessName)
        {
            return orderStatus switch
            {
                OrderStatus.PENDING        => "You have received a new order!",
                OrderStatus.ACCEPTED       => $"Your order with {businessName} has been accepted!",
                OrderStatus.PREPARING      => $"Your order with {businessName} is now being prepared!",
                OrderStatus.READYFORPICKUP => $"Your order with {businessName} is now ready to be picked up!",
                OrderStatus.PICKEDUP       => $"Your order with {businessName} has been picked up (we hope by you!)",
                OrderStatus.CANCELED       => $"Oh no! one of your orders has been cancelled by a customer!",
                OrderStatus.DENIED         => $"Oh no! one of your orders with {businessName} has been cancelled by the merchant!",
                _ => "Error: "
            };
        }
    }
}