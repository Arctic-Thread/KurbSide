namespace KurbSide.Service
{
    public enum OrderStatus
    {
        /// <summary>
        /// Order creation.
        /// </summary>
        PENDING = 0,

        /// <summary>
        /// The business has accepted the order.
        /// </summary>
        ACCEPTED = 1,

        /// <summary>
        /// The business is preparing the order.
        /// </summary>
        PREPARING = 2,

        /// <summary>
        /// The business has indicated the order is ready to be picked up.
        /// </summary>
        READYFORPICKUP = 3,

        /// <summary>
        /// The member has picked up the order. The order is now closed.
        /// </summary>
        PICKEDUP = 4,

        /// <summary>
        /// The member has cancelled the order.
        /// </summary>
        CANCELED = 5,

        /// <summary>
        /// The business has denied the order.
        /// </summary>
        DENIED = 6
    }
}