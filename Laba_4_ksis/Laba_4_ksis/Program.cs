using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace Laba_4_ksis
{
    class Program
    {
        FtpClient client = new FtpClient();
        static FtpWebRequest request;

        public string Filename()
        {
            Console.WriteLine("Choose file (write name of file with extension)");
            string filename = Console.ReadLine();
            return filename;
        }
        public void startMenu()
        {
            Console.WriteLine("Write your UserName");
            client.UserName = Console.ReadLine();
            Console.WriteLine("Write your Password");
            client.Password = Console.ReadLine();
            Console.WriteLine("Write your FTP-Adress (in format ftp://....:PORT/.../");
            client.Host = Console.ReadLine();
        }
        public void Uploadfile()
        {
            try
            {
                Console.WriteLine("Write full path for file");
                string path = Console.ReadLine();
                if (path[0] == path[path.Length - 1])
                    path = path.Substring(1, path.Length - 2);
                string filename = Path.GetFileName(path);
                request = (FtpWebRequest)WebRequest.Create(client.Host + filename);
                request.Credentials = new NetworkCredential(client.UserName, client.Password);

                request.UseBinary = true;
                request.Method = WebRequestMethods.Ftp.UploadFile;

                byte[] buffer = File.ReadAllBytes(path);

                request.ContentLength = buffer.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(buffer, 0, buffer.Length);
                }

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public void Deletefile()
        {
            try
            {
                string filename = Filename();
                request = (FtpWebRequest)WebRequest.Create(client.Host + filename);
                request.Credentials = new NetworkCredential(client.UserName, client.Password);

                //request.EnableSsl = _UseSSL;
                request.Method = WebRequestMethods.Ftp.DeleteFile;

                FtpWebResponse ftpResponse = (FtpWebResponse)request.GetResponse();
                ftpResponse.Close();
                Console.WriteLine("Remove of file are completed");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void DownloadFile()
        {
            try
            {
                string filename = Filename();
                request = (FtpWebRequest)WebRequest.Create(client.Host + filename);
                request.Credentials = new NetworkCredential(client.UserName, client.Password);

                request.Method = WebRequestMethods.Ftp.DownloadFile;

                //request.EnableSsl = true; // если используется ssl

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                Stream responseStream = response.GetResponseStream();

                FileStream fs = new FileStream(filename, FileMode.Create);

                byte[] buffer = new byte[64];
                int size = 0;

                while ((size = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fs.Write(buffer, 0, size);

                }
                fs.Close();
                response.Close();
                Console.WriteLine("Download and save of file are completed");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void ViewContent()
        {
            try
            {
                request = (FtpWebRequest)WebRequest.Create(client.Host);
                request.Credentials = new NetworkCredential(client.UserName, client.Password);

                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Console.WriteLine("Content of server:");
                Console.WriteLine();

                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                Console.WriteLine(reader.ReadToEnd());

                reader.Close();
                responseStream.Close();
                response.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                startMenu();
            }
        }
        static void Main(string[] args)
        {
            bool flag = true;
            string act = "0";
            Program thiscls = new Program();
            thiscls.startMenu();
            while (flag)
            { 
                Console.WriteLine("Choose act : \n 0 - exit\n 1 - UpLoadFile\n 2 - DownloadFile" +
                    "\n 3 - DeleteFile\n 4 - ViewContent\n 5 - Switch address");
                act = Console.ReadLine();
                switch (act)
                {
                    case "0" : flag = false; break;
                    case "1" : thiscls.Uploadfile();break;
                    case "2" : thiscls.DownloadFile(); break;
                    case "3" : thiscls.Deletefile(); break;
                    case "4" : thiscls.ViewContent(); break;
                    case "5" : thiscls.startMenu(); break;
                }
            }
            
        }
    }
}
