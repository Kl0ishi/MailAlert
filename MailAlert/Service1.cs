using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MailAlert
{
    [RunInstaller(true)]
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        public void OnDebug()
        {
            this.OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            DataSet nDs = new DataSet();
            nDs = GetDataMail();
            SendLoobMail(nDs);
            timer1.Start();

            //string strPath = AppDomain.CurrentDomain.BaseDirectory + "Log.txt";
            //System.IO.File.AppendAllLines(strPath, new[] { "Starting time : " + DateTime.Now.ToString() });

            //DataSet nDs = new DataSet();
            //nDs = GetDataMail();
            //SendLoobMail(nDs);

        }

        private void SendLoobMail(DataSet nDs)
        {
            if (nDs.Tables.Count != 0)
            {

                DataTable nDtThirty = new DataTable();
                if (nDs.Tables[0].Rows.Count > 0)
                {
                    nDtThirty = nDs.Tables[0];
                    if (nDtThirty.Rows.Count > 0)
                    {
                        if (nDtThirty.Rows[0][0].ToString() != "0")
                        {
                            //ส่ง Email 30 วัน
                            for (int i = 0; i < nDtThirty.Rows.Count; i++)
                            {
                                if (nDtThirty.Rows[i]["email"].ToString() != "")
                                {
                                    //ส่งเมล์
                                    SendMail(int.Parse(nDtThirty.Rows[i]["research_id"].ToString()),
                                        nDtThirty.Rows[i]["research_name_thai"].ToString(),
                                        nDtThirty.Rows[i]["end_date_lab"].ToString(),
                                        nDtThirty.Rows[i]["fullname"].ToString(),
                                        nDtThirty.Rows[i]["email"].ToString(),
                                       int.Parse(nDtThirty.Rows[i]["therty"].ToString()));
                                }
                                else
                                {

                                }
                            }
                        }
                        else
                        {

                        }
                    }
                    else
                    {

                    }
                }


                DataTable nDtNinety = new DataTable();
                nDtNinety = nDs.Tables[1];
                if (nDs.Tables[1].Rows.Count > 0)
                {
                    if (nDtNinety.Rows.Count > 0)
                    {
                        if (nDtNinety.Rows[0][0].ToString() != "1")
                        {
                            //ส่ง Email 90 วัน
                            for (int i = 0; i < nDtNinety.Rows.Count; i++)
                            {
                                if (nDtNinety.Rows[i]["email"].ToString() != "")
                                {
                                    //ส่งเมล์
                                    SendMail(int.Parse(nDtNinety.Rows[i]["research_id"].ToString()),
                                        nDtNinety.Rows[i]["research_name_thai"].ToString(),
                                        nDtNinety.Rows[i]["end_date_lab"].ToString(),
                                        nDtNinety.Rows[i]["fullname"].ToString(),
                                        nDtNinety.Rows[i]["email"].ToString(),
                                       int.Parse(nDtNinety.Rows[i]["nine"].ToString()));
                                }
                                else
                                {

                                }
                            }
                        }
                        else
                        {

                        }
                    }
                    else
                    {

                    }
                }

            }
            else
            {

            }
        }

        private void SendMail(int ResearchId, string ResearchName, string EndDate, string FullName, string Email2, int Numday)
        {
            string strURL = "https://www3.ra.mahidol.ac.th/ERequest/index.aspx";
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.EnableSsl = true;
            smtpClient.Host = "mumail.mahidol.ac.th";
            smtpClient.Port = 587;
            smtpClient.Timeout = 20000;
            smtpClient.Credentials = new NetworkCredential(userName: "ra_erequest@mahidol.ac.th", password: "");


            StringBuilder sb = new StringBuilder();
            sb.Append("เรียน คุณ" + FullName);
            sb.Append("<br>");
            sb.Append("<br>");
            sb.Append("<br>");
            sb.Append("     ตามที่ท่านได้กรอกข้อมูลระบบขอใช้ห้องปฏิบัติการวิจัย (LAB) งานห้องปฏิบัติการวิจัย สำนักงานวิจัยวิชาการและนวัฒกรรม คณะแพทยศาสตร์โรงพยาบาลรามาธิบดี มาแล้วนั้น ");
            sb.Append("<br>");

            sb.Append("     บัดนี้ข้อมูลโครงการ   " + ResearchName + "    ของท่านเหลืออายุโครงการ " + Numday + " วัน ขอให้ท่านเข้าระบบเพื่อต่ออายุโครงการ ด้วยครับ ");
            sb.Append("<br>");
            sb.Append(" <br> <a style='text-decoration: none; font-weight: bold;' href='" + strURL + "'>โดยเข้าสู่ระบบ คลิกที่นี้ !</a> ");
            sb.Append("<br>");

            sb.Append("จึงเรียนมาเพื่อทราบ");
            sb.Append("<br>");
            sb.Append("ขอบคุณครับ");
            sb.Append("<br>");
            sb.Append("<br>");
            sb.Append("<br>");
            sb.Append("งานห้องปฏิบัติการวิจัย สำนักงานวิจัย วิชาการ และนวัฒกรรม คณะแพทยศาสตร์โรงพยาบาลรามาธิบดี");

            sb.Append("<br>");
            sb.Append("<br>");
            sb.Append("<br>");
            sb.Append("********** หมายเหตุ : ข้อความและ e-mail นี้เป็นการสร้างอัตโนมัติจากระบบฯ ไม่ต้องตอบกลับ **********");

            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(Email2);
                mail.From = new MailAddress("ra_erequest@mahidol.ac.th", "ERequest");
                mail.Subject = "แจ้งเตือนวันหมดอายุของโครงการ";
                mail.Body = sb.ToString();
                mail.IsBodyHtml = true;
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                string strPath = AppDomain.CurrentDomain.BaseDirectory + "Log.txt";
                System.IO.File.AppendAllLines(strPath, new[] { "Stop time : " + DateTime.Now.ToString() });
            }
        }

        protected override void OnStop()
        {
            //string strPath = AppDomain.CurrentDomain.BaseDirectory + "Log.txt";
            //System.IO.File.AppendAllLines(strPath, new[] { "Stop time : " + DateTime.Now.ToString() });

            timer1.Stop();
        }


        private DataSet GetDataMail()
        {

            DataSet nDs = new DataSet();

            string scheduledTime = (ConfigurationManager.AppSettings["scheduledTime"]);
            string Thirty = (ConfigurationManager.AppSettings["Thirty"]);
            string Ninety = (ConfigurationManager.AppSettings["Ninety"]);


            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            using (SqlConnection Con = new SqlConnection(constr))
            {
                try
                {
                    Con.Open();
                    using (SqlCommand Cmd = new SqlCommand("Spd_get_EmailNoti", Con))
                    {
                        Cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter Adp = new SqlDataAdapter(Cmd);
                        Cmd.Parameters.AddWithValue("@Time", scheduledTime);
                        Cmd.Parameters.AddWithValue("@NumDay30", int.Parse(Thirty));
                        Cmd.Parameters.AddWithValue("@NumDa90", int.Parse(Ninety));
                        Adp.Fill(nDs, "0");
                        Con.Close();
                    }
                }
                catch (Exception ex)
                {
                    Con.Close();
                    nDs = null;
                }

            }

            return nDs;
        }
      

        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            DataSet nDs = new DataSet();
            nDs = GetDataMail();
            SendLoobMail(nDs);
        }
    }
}
