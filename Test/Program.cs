using System.IO;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            ApkParser.ApkParser.Parse(Directory.GetCurrentDirectory() + @"\sample.apk");
        }
    }
}
