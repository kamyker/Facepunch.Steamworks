using System;
using System.Text;

namespace Steamworks
{
	public class AuthTicket : IDisposable
	{
		public byte[] Data;
		public uint Handle;

		/// <summary>
		/// Cancels a ticket. 
		/// You should cancel your ticket when you close the game or leave a server.
		/// </summary>
		public void Cancel()
		{
			if ( Handle != 0 )
			{
				SteamUser.Internal.CancelAuthTicket( Handle );
			}

			Handle = 0;
			Data = null;
		}

		public void Dispose()
		{
			Cancel();
		}
		
		/// <summary>
		/// Converts the ticket from binary to hex string. Can be used with ISteamUserAuth.AuthenticateUserTicket webapi.
		/// </summary>
		/// <returns></returns>
		public string ToHexString()
		{
			var sb = new StringBuilder();
			foreach ( byte b in Data )
				sb.AppendFormat( "{0:x2}", b );
			return  sb.ToString();
		}
	}
}