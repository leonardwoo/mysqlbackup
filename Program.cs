using System;
using System.Diagnostics;
using System.IO;

namespace mysqlbackup
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Out.WriteLine("MySQL/MariaDB Backup");
            mysql setting = mysql.Default;
            DateTime now = DateTime.Now;
            string datatime = now.ToString("yyyyMMddhhmmss");
            string strCmd = string.Format("{0} -u{1} -p{2} {3}>{4}", setting.mysql_pathname, 
                setting.mysql_username, setting.mysql_password, setting.mysql_database,(setting.mysql_database + "-" + datatime) +".sql.bak");
            string echo = new Program().StartCmd(".\\", strCmd);
            if ("".Equals(echo))
            {
                echo = "Database " + setting.mysql_database + " has been successfully exported";
            }
            Console.Out.WriteLine(echo);
        }

        /// <summary>
        /// Run command line
        /// </summary>
        /// <param name="workingDirectory">working directory</param>
        /// <param name="command">commang</param>
        public string StartCmd(string workingDirectory, string command)
        {
            string strOutput = "";

            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/C" + command;
            p.StartInfo.WorkingDirectory = workingDirectory;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;

            try
            {
                if (p.Start())
                {
                    StreamReader reader = p.StandardOutput;
                    StreamReader error = p.StandardError;
                    strOutput = reader.ReadToEnd();
                    if (error != null)
                    {
                        Console.Error.WriteLine(error.ReadToEnd());
                    } else
                    {
                        Console.WriteLine(reader.ReadToEnd());
                    }
                    p.WaitForExit();
                }
            }
            catch (Exception exp)
            {
                Console.Error.WriteLine(exp.Message);
            }
            finally
            {
                if (p != null) p.Close();
            }
            return strOutput;
        }
    }
}
