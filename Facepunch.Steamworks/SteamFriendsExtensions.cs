using System;

namespace Steamworks
{
	public static class SteamFriendsExtensions
	{
		/// <summary>
		/// Works also for players that aren't friends
		/// </summary>
		public static void RequestName( this Friend player, Action<string> onRetrived )
		{
			if ( !SteamFriends.RequestUserInformation( player.Id, true ) )
			{
				onRetrived?.Invoke( player.Name );
			}
			else
			{
				var playerId = player.Id;
				SteamFriends.OnPersonaStateChange += SteamFriends_OnPersonaStateChange;

				void SteamFriends_OnPersonaStateChange( Friend friendChanged )
				{
					if ( playerId == friendChanged.Id )
					{
						SteamFriends.OnPersonaStateChange -= SteamFriends_OnPersonaStateChange;
						onRetrived?.Invoke( friendChanged.Name );
					}
				}
			}
		}
	}
}
