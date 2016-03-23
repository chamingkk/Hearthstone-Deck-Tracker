﻿#region

using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.LogReader.Interfaces;
using Hearthstone_Deck_Tracker.Utility.Logging;

#endregion

namespace Hearthstone_Deck_Tracker.LogReader.Handlers
{
	public class NetHandler
	{
		public void Handle(string logLine, IHsGameState gameState, IGame game)
		{
			var match = HsLogReaderConstants.ConnectionRegex.Match(logLine);
			if(match.Success)
			{
				game.MetaData.ServerAddress = match.Groups["address"].Value.Trim();
				game.MetaData.ClientId = match.Groups["client"].Value.Trim();
				game.MetaData.GameId = match.Groups["game"].Value.Trim();
				game.MetaData.SpectateKey = match.Groups["spectateKey"].Value.Trim();

				var region = Helper.GetRegionByServerIp(game.MetaData.ServerAddress);
				if(game.CurrentRegion == Region.UNKNOWN || region == Region.CHINA)
				{
					game.CurrentRegion = region;
					Log.Info("Set current region to" + region);
				}

				gameState.Reset();
				gameState.GameHandler.HandleGameStart();
			}
		}
	}
}