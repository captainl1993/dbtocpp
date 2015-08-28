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
            //初始化所有的数据库结构
            DownDatas.download();
            //生成数据库数据结构
            Gencpp.GenStructs();
            Gencpp.GenIds();
            Gencpp.GenDBHandler();
            Gencpp.GenDBReader();
            Gencpp.GenRedisHandler();
        }
    }
}
