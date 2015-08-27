#pragma once

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
			static void SetDBHandler(DBHandler* _Handler);
			static void playertoHandler();
			static void player_itemtoHandler();
	};

}
