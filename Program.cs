using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace mysqlbackup
{
    class Program
    {
        static void Main(string[] args)
        {
            mysql setting = mysql.Default;
            DateTime now = DateTime.Now;
            string datatime = now.ToString("yyyyMMddhhmmss");
            string strCmd = string.Format("mysqldump -u{0} -p{1} {2}>{3}", 
                setting.mysql_username, setting.mysql_password, setting.mysql_database,(setting.mysql_database + "-" + datatime) +".sql.bak");
            new Program().StartCmd(".\\", strCmd);
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
                if (p.Start())//执行
                {
                    p.WaitForExit();//等待程序执行完，退出进程
                    //strOutput = p.StandardOutput.ReadToEnd();//获取返回值
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
