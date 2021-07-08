using System;
using System.IO;
using System.Threading;

internal class Logger 
{
    internal enum Severity
    {
        race, debug,
        information,
        warning, error, critical
    }

    internal struct systemlogin
    {
        public string data { get; set; }
        public Logger.Severity severity { get; set; }
    }

    internal Logger(string filepath)
    {
        fs = new StreamWriter(filepath, true);
    }

    public static void Log(string data, Severity severity)
    {
        fs.Write(Environment.NewLine + " [ " + DateTime.Now.ToString() + "] [ " + severity.ToString() + " ] :" + data);

    }

    public void Break()
    {
        fs.Flush();
        fs.Close();
        fs.Dispose();
    }

    private static StreamWriter fs;
}

class Pragma
{
    Logger Logger { get; set; }


    static Logger.systemlogin _syslog;

    static Random random = new Random();

    static Array values = Enum.GetValues(typeof(Logger.Severity));

    public void Launch(string path)
    {
        Logger = new Logger(path);
    }

    private void fillbeforetwelve()
    {
        _syslog.data = "First Result";
        _syslog.severity = (Logger.Severity)values.GetValue(random.Next(values.Length));
    }
    private static void fillaftertwelve()
    {
        _syslog.data = "Second Result";
        _syslog.severity = (Logger.Severity)values.GetValue(random.Next(values.Length));
    }

    public int Do()
    {
        try
        {
            if (Logger == null) throw new DirectoryNotFoundException("Logger or filepath can be equal null,relaunch Pragma");
            if (DateTime.Now.Hour < 12)
                fillbeforetwelve();

            else
                fillaftertwelve();

            Logger.Log(_syslog.data, _syslog.severity);
            return 0;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
            return -1;
        }
    }

    public void Break()
    {
        Logger.Break();
        Logger = null;
    }
}

namespace practica
{
    class Program
    {
        static void Main(string[] args)
        {
            Pragma pragma = new Pragma();

            pragma.Launch("tests.txt");

            pragma.Do();
            pragma.Do();
            pragma.Do();
            pragma.Do();
            pragma.Break();
            FileStream flow = new FileStream("tests.txt", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            StreamReader str = new StreamReader(flow);
            string data = str.ReadToEnd();
            string[] array_data = data.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);



            for (int i = 0; i < array_data.Length; i++) Console.WriteLine(array_data[i]);



            str.Close();
        }
    }
}
