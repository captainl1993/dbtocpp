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
        static String vesion="V0.1";
        static StringBuilder sb = new StringBuilder();
        public static void GenStructs()
        {
            sb.Clear();
            sb = GenCommon.GenHeader(sb, "DBTypes.h", vesion, "生成所有的数据表结构类，一张表对应一个类。");
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
            sb = GenCommon.GenHeader(sb, "DBIds.h", vesion, "用于管理对应数据表的最大id，每次插入id+1。");

            sb.Append(@"#pragma once
namespace DBProduce
{
	class DBIds
    {
    public:");
            foreach (KeyValuePair<String, List<DBField>> table in DownDatas.tables)
            {

                sb.Append(@"
        static int " + table.Key + @";");
            }
            sb.Append(@"
    };
}");
            OutPut.Out("DBIds.h", sb.ToString());
            //cpp文件------------------------------------------------------------------------------

            sb.Clear();
            sb = GenCommon.GenHeader(sb, "DBIds.cpp", vesion, "用于管理对应数据表的最大id，每次插入id+1。");

            sb.Append(@"#include ""All.h""
namespace DBProduce
{
");
            foreach (KeyValuePair<String, List<DBField>> table in DownDatas.tables)
            {
                sb.Append(@"
    int DBIds::" + table.Key + @"=0;");
            }
            sb.Append(@"
}");
            OutPut.Out("DBIds.cpp", sb.ToString());
        }
        public static void GenDBHandler()
        {
            sb.Clear();
            sb = GenCommon.GenHeader(sb, "DBHandler.h", vesion, "数据库处理接口，任何调用车可以继承这个接口实现数据库的处理方法实现。");

            sb.Append(@"#pragma once

namespace DBProduce
{
	class DBHandler
	{
		public:
			DBHandler();
			virtual ~DBHandler();");
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
            sb = GenCommon.GenHeader(sb, "DBHandler.cpp", vesion, "数据库处理接口，任何调用车可以继承这个接口实现数据库的处理方法实现。");
            sb.Append(@"#include ""All.h""

namespace DBProduce
{
	DBHandler::DBHandler()
	{

	}
	DBHandler::~DBHandler()
	{

	}");
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
            sb = GenCommon.GenHeader(sb, "DBReader.h", vesion, "数据库统一读取实现，读取数据库所有的数据，并调用设置的处理接口进行处理。");

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
			static void Disconnect();
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
            sb = GenCommon.GenHeader(sb, "DBReader.cpp", vesion, "数据库统一读取实现，读取数据库所有的数据，并调用设置的处理接口进行处理。");

            sb.Append(@"#include ""All.h""

namespace DBProduce
{
	MYSQL DBReader::conn;
	MYSQL_RES* DBReader::res_ptr = nullptr;
	MYSQL_ROW DBReader::sqlrow;
	DBHandler* DBReader::dbHandler = nullptr;
	void DBReader::connection(const char* host, const char* user,
	        const char* password, const char* database)
	{
		mysql_init (&conn);
		
		if (mysql_real_connect(&conn, host, user, password, database, 0, NULL,
		        0))
		{
            mysql_query(&conn, ""SET NAMES UTF8"");
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
	void DBReader::Disconnect()
	{
        mysql_close (&conn);
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

                for (int i = 0; i < table.Value.Count; i++)
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

                if(p->id>DBIds::" + table.Key + @")
                {
                    DBIds::" + table.Key + @"=p->id;
                }
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

        public static void GenRedisHandler()
        {
            sb.Clear();
            sb = GenCommon.GenHeader(sb, "RedisHandler.h", vesion, "继承自DBHandler，以redis数据入库的方式来处理数据读出的数据。");
            sb.Append(@"#pragma once
namespace DBProduce
{
	class RedisHandler: public DBHandler
	{
		private:
			RedisDBEngine redisengine;
		public:
			RedisHandler();
			virtual ~RedisHandler();

			void connect(std::string _ip, short _port);
			//清空redis中所有的数据，慎用
			void flashAll();");
            foreach (KeyValuePair<String, List<DBField>> table in DownDatas.tables)
            {
                sb.Append(@"
			void read" + table.Key + @"(DBProduce::" + table.Key + @"* _" + table.Key + @");");
            }
            sb.Append(@"
	};

}
");
            OutPut.Out("RedisHandler.h", sb.ToString());
            //CPP文件---------------------------------------------------------------------------
            sb.Clear();
            sb = GenCommon.GenHeader(sb, "RedisHandler.cpp", vesion, "继承自DBHandler，以redis数据入库的方式来处理数据读出的数据。");

            sb.Append(@"#include ""All.h""

namespace DBProduce
{
	RedisHandler::RedisHandler()
	{
	}
	RedisHandler::~RedisHandler()
	{
		
	}
	void RedisHandler::connect(std::string _ip, short _port)
	{
		redisengine.connect(_ip, _port);
	}
	void RedisHandler::flashAll()
	{
		std::string cmd = ""flushall"";
		redisengine.excuteCommoned(cmd);
	}");

            foreach (KeyValuePair<String, List<DBField>> table in DownDatas.tables)
            {
                sb.Append(@"
	void RedisHandler::read" + table.Key + @"(DBProduce::" + table.Key + @"* _" + table.Key + @")
	{");
                sb.Append(@"
        std::string cmd = ""hmset  " + table.Key + @":"" + std::to_string(_" + table.Key + @"->id) ");
                for (int i = 1; i < table.Value.Count; i++)
                {
                    if (TypesChange.dbtocpp(table.Value[i].type) == "std::string")
                    {
                        sb.Append(@"
        + "" " + table.Value[i].name + @" "" + _" + table.Key + @"->" + table.Value[i].name + @" ");
                    }
                    else
                    {
                        sb.Append(@"
        + "" " + table.Value[i].name + @" "" + std::to_string(_" + table.Key + @"->" + table.Value[i].name + @") ");                    
                    }
                }
                sb.Append(@";
		redisengine.excuteCommoned(cmd);
		delete _" + table.Key + @";
	}");
            }

            sb.Append(@"
}
");
            OutPut.Out("RedisHandler.cpp", sb.ToString());
        }

    }
}
