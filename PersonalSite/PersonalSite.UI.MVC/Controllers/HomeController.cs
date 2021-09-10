using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using PersonSiteMVC.UI.MVC.Models;

namespace PersonSiteMVC.UI.MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Portfolio()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactViewModel cvm)
        {
            if (ModelState.IsValid)
            {
                string content = $"You have a new email from {cvm.Email}.\n The subject line is: {cvm.Subject}\n" +
                    $"Message: \n" +
                    $"{cvm.Message}";

                MailMessage mailMessage = new MailMessage(
                    //FROM
                    ConfigurationManager.AppSettings["EmailUser"].ToString(),
                    //TO
                    ConfigurationManager.AppSettings["EmailTo"].ToString(),
                    //Subject
                    cvm.Subject,
                    //Message
                    content);

                mailMessage.IsBodyHtml = true;
                mailMessage.Priority = MailPriority.High;
                mailMessage.ReplyToList.Add(cvm.Email);

                //Config for SMTP mail client
                SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["EmailClient"].ToString());
                client.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailUser"].ToString(), ConfigurationManager.AppSettings["EmailPass"].ToString());

                //client.Port = 25;
                client.Port = 8889;

                //Checks if the mail servers are online and the message can be send, if not it throws and exception
                try
                {
                    //Send email
                    client.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = $"I'm sorry, your email could not be sent at this time. Please try again later. Error Message: <br /> {ex.StackTrace}";
                    return View(cvm);
                }
                return View("EmailConfirmation", cvm);
            }//End IF

            return View(cvm);
        }

        public ActionResult Resume()
        {
            return View();
        }
    }
}