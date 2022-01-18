using Microsoft.AspNetCore.Mvc;

namespace ServicesAPI6.Controllers
{
    public class LoggerCls : ControllerBase
    {
        string _Path = @"C:\Users\Ayse\source\repos\ServicesAPI6\ServicesAPI6\Log\";
        string _FileName = DateTime.Now.ToString("yyyyMMdd") + ".txt";
        public void createLog(string message)
        {
            FileStream fs = new FileStream(_Path + _FileName, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(DateTime.Now.ToString() + ":" + message);
            sw.Flush();
            sw.Close();
            fs.Close();
        }
    }
}
