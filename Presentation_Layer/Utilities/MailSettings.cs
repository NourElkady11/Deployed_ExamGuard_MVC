using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Net;
using System.Net.Mail;

namespace Presentation_Layer.Utilities
{

	public class Mail
	{
        public string? Subject { get; set; }

        public string? body { get; set; }

        public string? Recipent { get; set; }

    }
	public class MailSettings
	{
		public static void SendEmail(Mail email)
		{
			//Create SMTP Client
			var client = new SmtpClient("smtp.gmail.com", 587);
			client.EnableSsl = true;
			client.UseDefaultCredentials = false;
			//Create Network credintials
			client.Credentials= new NetworkCredential("nourel2ady11@gmail.com", "dsayrujvcwdvxmhn");
			client.Send("nourel2ady11@gmail.com", email.Recipent, email.Subject, email.body);
		}
	}
}
