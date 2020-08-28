using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Steamworks.Data;

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
					SteamFriends.OnPersonaStateChange -= SteamFriends_OnPersonaStateChange;
					if ( playerId == friendChanged.Id )
					{
						onRetrived?.Invoke( friendChanged.Name );
					}
				}
			}
		}
	}
}
