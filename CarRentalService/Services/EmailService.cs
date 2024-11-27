using System.Net.Mail;
using System.Net;
using CarRentalService.Models;
public interface IEmailService
{
    Task SendRentalConfirmationEmailAsync(Car c, User u, int days);
}

namespace CarRentalService.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendRentalConfirmationEmailAsync(Car c, User u, int days)
        {
            var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("5b************", "*********5117f"),
                EnableSsl = true
            };
            var subject = "Car Rental Confirmation";
            decimal rentalCost = c.PricePerDay * days;

            // Calculate rental start and end date (assume the rental starts from tomorrow)
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(days);

            // Create the plain text version of the email
            var plainText = $@"
            Dear {u.Name},  

            Your car rental has been confirmed!

            Details:
            Car: {c.Make} {c.Model} ({c.Year})
            Duration: {days} Days
            Total Price: ₹{rentalCost}

            Thank you for choosing our service!

            Best regards,
            Car Rental System Team
            ";

            // Create the HTML version of the email
            var htmlContent = $@"
            <html>
            <body style='font-family: Arial, sans-serif; color: #333;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px; background-color: #f9f9f9;'>
                    <p style='font-size: 18px; color: #444;'>Dear <strong style='color: #2c3e50;'>{u.Name}</strong>,</p>
                    <p style='font-size: 16px; color: #444;'>Your car rental has been confirmed!</p>
                    <p style='font-size: 16px; color: #444;'><strong style='color: #2980b9;'>Details:</strong></p>
                    <p style='font-size: 14px; color: #444; margin-bottom: 10px;'>
                        <strong style='color: #2980b9;'>Car:</strong> {c.Make} {c.Model} ({c.Year})<br/>
                        <strong style='color: #2980b9;'>Duration:</strong> {days} <span style='color: #2980b9;'>Days</span><br/>
                        <strong style='color: #2980b9;'>Total Price:</strong> <span style='font-weight: bold;'>₹{rentalCost}</span>
                    </p>
                    <p style='font-size: 16px; color: #444;'>Thank you for choosing our service!</p>
                    <p style='font-size: 14px; color: #444;'>Best regards,<br/>Car Rental System Team</p>
                </div>
            </body>
            </html>
            ";



            var mailMessage = new MailMessage
            {
                From = new MailAddress("CarRentalService@guvi.com"),
                Subject = subject,
                IsBodyHtml = true 
            };

            mailMessage.To.Add(u.Email);  


            var plainTextView = AlternateView.CreateAlternateViewFromString(plainText, null, "text/plain");
            var htmlView = AlternateView.CreateAlternateViewFromString(htmlContent, null, "text/html");

            mailMessage.AlternateViews.Add(plainTextView);
            mailMessage.AlternateViews.Add(htmlView);

            try
            {
                client.Send(mailMessage);
                System.Console.WriteLine("Email sent successfully.");
            }
            catch (SmtpException ex)
            {
                System.Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
}
