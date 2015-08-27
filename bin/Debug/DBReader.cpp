#include "All.h"

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
			printf("Connection success!\n");
		}
		else
		{
			fprintf(stderr, "Connection failed!\n");
			if (mysql_errno(&conn))
			{
				fprintf(stderr, "Connection error %d: %s\n", mysql_errno(&conn),
				        mysql_error(&conn));
			}
			exit (EXIT_FAILURE);
		}
	}
	void DBReader::DBtoHandler()
	{
		
	}
	void DBReader::SetDBHandler(DBHandler* _Handler)
	{
		dbHandler = _Handler;
	}
    void DBReader::playertoHandler()
	{
		int res = mysql_query(&conn, "SELECT * from player");
		if (res)
		{
			fprintf(stderr, "SELECT error: %s\n", mysql_error(&conn));
		}
		else
		{
			res_ptr = mysql_use_result(&conn);
			if (res_ptr)
			{
				while ((sqlrow = mysql_fetch_row(res_ptr)))
				{
					mysql_field_count (&conn);
					player * p = new player();
					p->id= atoi(sqlrow[0]);
					p->account= sqlrow[1];
					p->passwd= sqlrow[2];
					p->nickname= sqlrow[3];
					dbHandler->readplayer(p);
				}
				if (mysql_errno (&conn))
				{
					fprintf(stderr, "Retrive error: %s\n", mysql_error(&conn));
				}
				mysql_free_result (res_ptr);
			}
		}
    }
    void DBReader::player_itemtoHandler()
	{
		int res = mysql_query(&conn, "SELECT * from player_item");
		if (res)
		{
			fprintf(stderr, "SELECT error: %s\n", mysql_error(&conn));
		}
		else
		{
			res_ptr = mysql_use_result(&conn);
			if (res_ptr)
			{
				while ((sqlrow = mysql_fetch_row(res_ptr)))
				{
					mysql_field_count (&conn);
					player_item * p = new player_item();
					p->id= atoi(sqlrow[0]);
					p->userid= atoi(sqlrow[1]);
					p->itemid= atoi(sqlrow[2]);
					dbHandler->readplayer_item(p);
				}
				if (mysql_errno (&conn))
				{
					fprintf(stderr, "Retrive error: %s\n", mysql_error(&conn));
				}
				mysql_free_result (res_ptr);
			}
		}
    }
}
