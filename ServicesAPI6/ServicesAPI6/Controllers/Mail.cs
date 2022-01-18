using System.Net;
using System.Net.Mail;

namespace ServicesAPI6.Controllers
{
    public class Mail
    {
        public void sendMail()
        {
            try
            {
                SmtpClient sc = new SmtpClient();
                sc.Port = 587;
                sc.Host = "smtp.gmail.com";
                sc.EnableSsl = true;

                string kime = "aysenurkocakx@gmail.com";
                string konu = "cc";
                string icerik = "vvvvvv";

                sc.Credentials = new NetworkCredential("aysenurkocakx@gmail.com", "Ak53*c3a");
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("posta@gmail.com", "Ahmet Cansever");
                mail.To.Add(kime);
                //mail.To.Add("alici2@mail.com");
                //mail.CC.Add("alici3@mail.com");
                //mail.CC.Add("alici4@mail.com");
                mail.Subject = konu;
                mail.IsBodyHtml = true;
                mail.Body = icerik;
                sc.UseDefaultCredentials = true;
                //mail.Attachments.Add(new Attachment(DosyaYolu));
                sc.Send(mail);
            }
            catch(Exception exc)
            {

            }
        }
    }
}
