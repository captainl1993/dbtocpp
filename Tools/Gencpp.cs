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
namespace DBProduce
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
namespace DBProduce
{
	class DBIds
    {
");
            foreach (KeyValuePair<String, List<DBField>> table in DownDatas.tables)
            {

                sb.Append(@"        static int " + table.Key + @";");
            }
            sb.Append(@"
    };
}");
            OutPut.Out("DBIds.h", sb.ToString());
        }
        public static void GenDBHandler()
        {
            sb.Clear();
            sb.Append(@"#pragma once

namespace DBProduce
{
	class DBHandler
	{
		public:");
            foreach (KeyValuePair<String, List<DBField>> table in DownDatas.tables)
            {
                sb.Append(@"
			virtual void read" + table.Key + @"(DBProduce::" + table.Key + @"* _" + table.Key + @")=0;");
            }
            sb.Append(@"
	};

}
");
            OutPut.Out("DBHandler.h", sb.ToString());
            //cpp文件------------------------------------------------------------
            sb.Clear();
            sb.Append(@"#include ""All.h""

namespace DBProduce
{");
            foreach (KeyValuePair<String, List<DBField>> table in DownDatas.tables)
            {
                sb.Append(@"
	void DBHandler::read" + table.Key + @"(DBProduce::" + table.Key + @"* _" + table.Key + @")
	{
		
	}
");
            }
            sb.Append(@"
}
");
            OutPut.Out("DBHandler.cpp", sb.ToString());
        }
        public static void GenDBReader()
        {
            sb.Clear();
            sb.Append(@"#pragma once

namespace DBProduce
{
	class DBReader
	{
		private:
			static MYSQL conn;
			static MYSQL_RES *res_ptr;
			static MYSQL_ROW sqlrow;
			static DBHandler* dbHandler;
		public:
			static void connection(const char* host, const char* user,
			        const char* password, const char* database);
			static void DBtoHandler();
			static void SetDBHandler(DBHandler* _Handler);");

            foreach (KeyValuePair<String, List<DBField>> table in DownDatas.tables)
            {
                sb.Append(@"
			static void " + table.Key + @"toHandler();");
            }
            sb.Append(@"
	};

}
");
            OutPut.Out("DBReader.h", sb.ToString());
            //cpp-------------------------------------------------------------------
            sb.Clear();
            sb.Append(@"#include ""All.h""

namespace DBProduce
{
	MYSQL DBReader::conn;
	MYSQL_RES* DBReader::res_ptr = nullptr;
	MYSQL_ROW DBReader::sqlrow = nullptr;
	DBHandler* DBReader::dbHandler = nullptr;
	void DBReader::connection(const char* host, const char* user,
	        const char* password, const char* database)
	{
		mysql_init (&conn);
		
		if (mysql_real_connect(&conn, host, user, password, database, 0, NULL,
		        0))
		{
			printf(""Connection success!\n"");
		}
		else
		{
			fprintf(stderr, ""Connection failed!\n"");
			if (mysql_errno(&conn))
			{
				fprintf(stderr, ""Connection error %d: %s\n"", mysql_errno(&conn),
				        mysql_error(&conn));
			}
			exit (EXIT_FAILURE);
		}
	}
	void DBReader::DBtoHandler()
	{");
            foreach (KeyValuePair<String, List<DBField>> table in DownDatas.tables)
            {
                sb.Append(@"
        " + table.Key + @"toHandler();");
            }
            sb.Append(@"
	}
	void DBReader::SetDBHandler(DBHandler* _Handler)
	{
		dbHandler = _Handler;
	}");
            foreach (KeyValuePair<String, List<DBField>> table in DownDatas.tables)
            {
                sb.Append(@"
    void DBReader::" + table.Key + @"toHandler()
	{
		int res = mysql_query(&conn, ""SELECT * from " + table.Key + @""");
		if (res)
		{
			fprintf(stderr, ""SELECT error: %s\n"", mysql_error(&conn));
		}
		else
		{
			res_ptr = mysql_use_result(&conn);
			if (res_ptr)
			{
				while ((sqlrow = mysql_fetch_row(res_ptr)))
				{
					mysql_field_count (&conn);
					" + table.Key + @" * p = new " + table.Key + @"();");

                for (int i=0;i< table.Value.Count;i++)
                {
                    sb.Append(@"
					p->" + table.Value[i].name + "= ");
                    if (table.Value[i].type == "float" || table.Value[i].type == "double")
                    {
                        sb.Append("atof(sqlrow[" + i + "]);");
                    }
                    else if (table.Value[i].type == "short"
                            || table.Value[i].type == "long"
                            || table.Value[i].type == "int")
                    {
                        sb.Append("atoi(sqlrow[" + i + "]);");
                    }
                    else
                    {
                        sb.Append("sqlrow[" + i + "];");
                    }
                }
                    sb.Append(@"
					dbHandler->read" + table.Key + @"(p);
				}
				if (mysql_errno (&conn))
				{
					fprintf(stderr, ""Retrive error: %s\n"", mysql_error(&conn));
				}
				mysql_free_result (res_ptr);
			}
		}
    }");
            }
            sb.Append(@"
}
");
            OutPut.Out("DBReader.cpp", sb.ToString());
        }
    }
}
