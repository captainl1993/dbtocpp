using dbtocpp.JsonTools;
using dbtocpp.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbtocpp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 1)
                Gencpp.folder = args[1];
            //初始化所有的数据库结构
            DownDatas.download();
            //生成数据库数据结构
            Gencpp.GenStructs();
            Gencpp.GenIds();
            Gencpp.GenDBHandler();
            Gencpp.GenDBReader();
            Gencpp.GenRedisHandler();
            Gencpp.GenRedisReader();

            //生成协议相关
            Gencpp.GenDBGameProto();
            Gencpp.GenDBStructProto();

            //生成特定的消息处理类
           // Gencpp.GenDBGameTask();

            //生成CS对应得结构
            //GenCS.GenStructs();
            //生成json相关的C++序列化反序列化文件
            //GenJsonMG.Gen();
        }
    }
}
