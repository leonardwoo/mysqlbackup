﻿using System;
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
            string strCmd = string.Format("mysqldump -u{0} -p{1} {2}>{3}", 
                setting.mysql_username, setting.mysql_password, setting.mysql_database,(setting.mysql_database + "-" + datatime) +".sql.bak");
            string echo = new Program().StartCmd(".\\", strCmd);
            if ("".Equals(echo))
            {
                echo = "Database " + setting.mysql_database + " has been successfully exported";
            }
            Console.Out.WriteLine(echo);
        }

        /// <summary>
        /// 执行cmd命令
        /// </summary>
        /// <param name="workingDirectory">要启动的进程目录</param>
        /// <param name="command">要执行的命令</param>
        public string StartCmd(string workingDirectory, string command)
        {
            string strOutput = "";

            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/C" + command;// "/c"标示执行完命令后退出
            p.StartInfo.WorkingDirectory = workingDirectory;
            p.StartInfo.UseShellExecute = false;//不启用shell 启动进程
            p.StartInfo.RedirectStandardInput = true;//重定向输入
            p.StartInfo.RedirectStandardOutput = true;//重定向标准输出
            p.StartInfo.RedirectStandardError = true;//重定向错误输出
            p.StartInfo.CreateNoWindow = true;//不创建新窗口

            try
            {
                if (p.Start())
                {
                    StreamReader reader = p.StandardOutput;//截取输出流
                    StreamReader error = p.StandardError;//截取错误信息
                    strOutput = reader.ReadToEnd();
                    if (error != null)
                    {
                        Console.Error.WriteLine(error.ReadToEnd());
                    }
                    p.WaitForExit();
                }
            }
            catch (Exception exp)
            {
               // MessageBox.Show(exp.Message, "软件提示");
            }
            finally
            {
                if (p != null) p.Close();
            }
            return strOutput;
        }
    }
}
