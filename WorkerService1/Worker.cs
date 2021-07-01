using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerService1
{
    public class WorkerOptions
    {

        public string Email { get; set; }
        public string Epass { get; set; }

        //... other properties
    }

    public class TimedHostedService : IHostedService, IDisposable
    {
        private readonly WorkerOptions _options;
        private int executionCount = 0;
        private readonly ILogger<TimedHostedService> _logger;
        private System.Threading.Timer _timer;

        public TimedHostedService(ILogger<TimedHostedService> logger, WorkerOptions options)
        {

            _logger = logger;
            this._options = options;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {

            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new System.Threading.Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(1));

            return Task.CompletedTask;

        }

        private void DoWork(object state)
        {
            try
            {
                string dateInString = "05.10.2021";
                
                DateTime startDate = DateTime.Parse(dateInString);

                DateTime expiryDate = startDate.AddDays(-30);

                DateTime dateStringSeven = startDate.AddDays(-7);

                if (DateTime.Now >= expiryDate && DateTime.Now >= dateStringSeven)
                {

                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress("email");
                        mail.To.Add("remetente");
                        mail.Subject = "-7 dias be like";
                        mail.Body = "<h1>teste</h1>";
                        mail.IsBodyHtml = true;
                        //mail.Attachments.Add(new Attachment("C:\\file.zip"));


                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                        {
                            //smtp.Credentials = new NetworkCredential("email", "password");
                            string strEmail = this._options.Email;
                            string strPassword = this._options.Epass;
                            smtp.Credentials = new System‎‎.Net‎‎.NetworkCredential(strEmail, strPassword);
                            smtp.EnableSsl = true;
                            smtp.Send(mail);
                            Console.WriteLine(dateStringSeven);
                            Console.WriteLine("Mail Enviado 7 dias pos 1 email");
                            dateStringSeven = dateStringSeven.AddDays(7);
                            Console.WriteLine(dateStringSeven);
                        }
                    }
                }
                else if(DateTime.Now >= expiryDate)
                {
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress("email");
                        mail.To.Add("remetente");
                        mail.Subject = "-30 dias be like";
                        mail.Body = "<h1>teste</h1>";
                        mail.IsBodyHtml = true;
                        //mail.Attachments.Add(new Attachment("C:\\file.zip"));


                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                        {
                            //smtp.Credentials = new NetworkCredential("email", "password");
                            string strEmail = this._options.Email;
                            string strPassword = this._options.Epass;
                            smtp.Credentials = new System‎‎.Net‎‎.NetworkCredential(strEmail, strPassword);
                            smtp.EnableSsl = true;
                            smtp.Send(mail);
                            Console.WriteLine("Mail Enviado 30 dias antes do prazo final ");
                        }
                    }
                    
                }
                else
                {
                    Console.WriteLine("Mail nao Enviado");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
