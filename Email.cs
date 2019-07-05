using System;
using System.Net.Mail;

namespace ConsoleApp3
{
    static public  class Email
    {

        public static void Send()
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("from");
                mail.To.Add("to");
              

                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(@"C:\Users\user\xz.txt");
                mail.Attachments.Add(attachment);

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("from", "password");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

            }
            catch (Exception ex)
            {
              
            }
        }
    }
}
