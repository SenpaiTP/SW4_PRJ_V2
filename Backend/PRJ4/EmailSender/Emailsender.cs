// using System.Net.Mail;
// using System.Net;
// using System;

// namespace SendingEmails;

// public class EmailSender : IEmailSender
// {
//     public Task SendEmail(string email, string subject, string content)
//     {
//        var mail="KontoKaren@outlook.com";
//        var pw = "irpaaablzswgfsij";

//        var client =new SmtpClient("smtp-mail.outlook.com",587)
//        {
//         EnableSsl = true,
//         Credentials = new NetworkCredential(mail,pw)
//        };   

//        return client.SendMailAsync(
//         new MailMessage(from:mail,
//                         to:email,
//                         subject,
//                         content));
//     }
            
// }
