using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using static KurbSide.Service.KSNotificationAndEmails;

namespace KurbSide.Service
{
    public class KSEmail
    {
        /// <summary>
        /// Generates an email message for order status updates.
        /// <br/>
        /// <code>Example: CreateEmailMessage(orderStatus, orderId, businessName)</code>
        /// </summary>
        /// <remarks>
        /// Liam De Rivers
        /// </remarks>
        /// <param name="orderStatus">The new order status.</param>
        /// <param name="orderId">The Id of the order being updated.</param>
        /// <param name="businessName">The business associated with the order.</param>
        /// <returns>A message with the relating the new order status and a link to the order.</returns>
        private static string CreateEmailMessage(OrderStatus orderStatus, Guid orderId, string businessName)
        {
            string message = CreateMessage(orderStatus, businessName);
            var linkToOrder = $"https://localhost:5001/Order/{orderId}"; //TODO

            message += $" to view it's details <a href='{HtmlEncoder.Default.Encode(linkToOrder)}'>Click here</a>";
            return message;
        }

        /// <summary>
        /// Generates an email message for order status updates.
        /// <br/>
        /// <code>Example: SendEmail(recipientEmail, orderStatus, orderId, BusinessName)</code>
        /// </summary>
        /// <remarks>
        /// TODO Still needs to be implemented
        /// TODO orderId should be optional; e.g. if the email is not associated with an order, and with a sale instead.
        /// Liam De Rivers
        /// </remarks>
        /// <param name="recipientEmail">The email address for the user who will be receiving the email.</param>
        /// <param name="orderStatus">The new order status.</param>
        /// <param name="orderId">The Id of the order associated with the notification.</param>
        /// <param name="businessName">The business associated with the order.</param>
        public static async Task SendEmail(string recipientEmail, OrderStatus orderStatus, Guid orderId, string businessName)
        {
            SendGridMailer mailer = new SendGridMailer();

            string subject = orderStatus switch
            {
                OrderStatus.PENDING        => "New KurbSide Order Received",
                OrderStatus.ACCEPTED       => "KurbSide Status Update",
                OrderStatus.PREPARING      => "KurbSide Status Update",
                OrderStatus.READYFORPICKUP => "KurbSide Status Update",
                OrderStatus.PICKEDUP       => "KurbSide Status Update",
                OrderStatus.CANCELED       => "KurbSide Order Cancellation",
                OrderStatus.DENIED         => "KurbSide Order Denied",
                _ => ""
            };

            string htmlMessage = CreateEmailMessage(orderStatus, orderId, businessName);

            await mailer.SendEmailAsync(recipientEmail, subject, htmlMessage);
        }
    }
}