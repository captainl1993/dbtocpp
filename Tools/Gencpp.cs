using dbtocpp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbtocpp.Tools
{
    public static class Gencpp
    {
        static StringBuilder sb = new StringBuilder();
        public static void GenStructs()
        {
            sb.Clear();
            sb.Append(@"#pragma once
namespace DBproduce
{
");
            foreach (KeyValuePair<String, List<DBField>> table in DownDatas.tables)
            {
                sb.Append(@"	struct " + table.Key + @"
	{");
                foreach (DBField field in table.Value)
                {
                    sb.Append(@"
		" + TypesChange.dbtocpp(field.type) + @" " + field.name + @";");
                }
                sb.Append(@"
	};
");
            }
            sb.Append(@"}");
            OutPut.Out("DBTypes.h", sb.ToString());
        }
        public static void GenIds()
        {
            sb.Clear();
            sb.Append(@"#pragma once
namespace DBproduce
{
	class DBIds
    {
");
            foreach (KeyValuePair<String, List<DBField>> table in DownDatas.tables)
            {

                sb.Append(@"        static int " + table.Key + @"=0;
");
            }
            sb.Append(@"    }
}");
            OutPut.Out("DBIds.h", sb.ToString());
        }

    }
}
