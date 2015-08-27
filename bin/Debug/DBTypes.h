#pragma once
namespace DBProduce
{
	struct player
	{
		int id;
		std::string account;
		std::string passwd;
		std::string nickname;
	};
	struct player_item
	{
		int id;
		int userid;
		int itemid;
	};
}