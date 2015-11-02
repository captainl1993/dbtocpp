using PacketGenerator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dbtocpp.JsonTools
{
    public static class GenJsonMG
    {
        public static void Gen()
        {
            var outputDirPath = @"..\Json";//System.IO.Path.Combine( Application.StartupPath, "Output" );
            if (!Directory.Exists(outputDirPath))
            {
                try
                {
                    Directory.CreateDirectory(outputDirPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadKey();
                    return;
                }
            }
            foreach (var fn in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "T_*.dll"))
            {
                var asm = Assembly.LoadFile(fn);
                var t = Helpers.GetTemplate(asm);
                var shortfn = new FileInfo(fn).Name;
                shortfn = shortfn.Substring(0, shortfn.LastIndexOf('.'));
                var path = outputDirPath;
                if (!Directory.Exists(path))
                {
                    try
                    {
                        Directory.CreateDirectory(path);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.ReadKey();
                        return;
                    }
                }

                var rtv = JsonGen.Gen(t, path, shortfn.Substring("T_".Length));
                if (rtv)
                {
                    Console.WriteLine(rtv.ToString());
                    Console.ReadKey();
                    return;
                }
            }
        }
        
    }
}
