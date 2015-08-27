#pragma once

namespace DBProduce
{
	class DBHandler
	{
		public:
			virtual void readplayer(DBProduce::player* _player)=0;
			virtual void readplayer_item(DBProduce::player_item* _player_item)=0;
	};

}
