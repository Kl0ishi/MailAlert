using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.ServiceProcess;
using System;

namespace WindowsServiceTest
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        //public void OnDebug()
        //{
        //    this.OnStart(null);
        //}

        // นับ Debug ทุกวินาที
        //protected override void OnStart(string[] args)
        //{
        //    timer = new System.Timers.Timer();
        //    timer.Interval = 1000; // 1 second interval
        //    timer.Elapsed += TimerElapsed;
        //    timer.Start();

        //    string strPath = AppDomain.CurrentDomain.BaseDirectory + "Log.txt";
        //    System.IO.File.AppendAllLines(strPath, new[] { "Starting time : " + DateTime.Now.ToString() });
        //}

        //private void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    string strPath = AppDomain.CurrentDomain.BaseDirectory + "Log.txt";
        //    System.IO.File.AppendAllLines(strPath, new[] { "Current time : " + DateTime.Now.ToString() });
        //}

        //protected override void OnStop()
        //{
        //    timer.Stop();
        //    timer.Dispose();
        //    string strPath = AppDomain.CurrentDomain.BaseDirectory + "Log.txt";
        //    System.IO.File.AppendAllLines(strPath, new[] { "Stop time : " + DateTime.Now.ToString() });
        //}

        //private System.Timers.Timer timer1;

        //protected override void OnStart(string[] args)
        //{
        //    timer1 = new System.Timers.Timer();
        //    timer1.Interval = 60000; // 1 minute interval
        //    timer1.Elapsed += EmailTimerElapsed;
        //    timer1.Start();

        //    // Rest of your code...
        //}

        private void EmailTimerElapsed()
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    // Execute a SQL command to retrieve email addresses from your database
                    string sqlQuery = "ps"; // Adjust the query to your database schema
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string emailAddress = reader["email_send"].ToString();
                                // Send email to emailAddress using SmtpClient
                                SendEmail(emailAddress);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    connection.Close();
                    // Handle exceptions and log errors here
                }
            }
        }

        private bool emailSent = false; // Track if the email has been sent today

        protected override void OnStart(string[] args)
        {
            WriteToFile("Service is started at " + DateTime.Now);
            EmailTimerElapsed();
        }

        private void SendEmail(string emailAddress)
        {
            try
            {
                // Create an instance of SmtpClient
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(userName: "Tester27531@gmail.com", password: "");

                // Create an instance of MailMessage
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("Tester27531@gmail.com");
                mail.To.Add(emailAddress); // Recipient's email address
                mail.Subject = "ทดสอบการแจ้งเตือน";
                mail.Body = "This is the body of your email.";

                // Send the email
                smtpClient.Send(mail);

                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        protected override void OnStop()
        {
            WriteToFile("Service is stopped at " + DateTime.Now);
        }
        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
    }
}