using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;
using System.Text;
using UnityEngine;
using CompressionLevel = System.IO.Compression.CompressionLevel;

namespace Steamworks.Data
{
	public static class LobbyExtensions
	{
		/// <summary>
		/// Get data associated with this lobby
		/// </summary>
		public static string GetDataDecompressed( this Lobby lobby, string key )
		{
			var value = SteamMatchmaking.Internal.GetLobbyData( lobby.Id, key );
			if ( string.IsNullOrEmpty( value ) )
				return value;

			value = Decompress( value );

			return value;
		}

		/// <summary>
		/// Get data associated with this lobby
		/// </summary>
		public static bool SetDataCompressed( this Lobby lobby, string key, string value )
		{
			if ( key.Length > 255 )
				throw new System.ArgumentException( "Key should be < 255 chars", nameof( key ) );

			value = Compress( value );
			
			if ( value.Length > 8192 )
				throw new System.ArgumentException( "Value should be < 8192 chars", nameof( key ) );

			return SteamMatchmaking.Internal.SetLobbyData( lobby.Id, key, value );
		}

		public static bool IsOwnedByLocalClient( this Lobby lobby )
		{
			return lobby.IsOwnedBy(SteamClient.SteamId);
		}
		
		public static string Compress( string uncompressedString )
		{
			byte[] compressedBytes;

			using ( var uncompressedStream = new MemoryStream( Encoding.UTF8.GetBytes( uncompressedString ) ) )
			{
				using ( var compressedStream = new MemoryStream() )
				{
					using ( var compressorStream = new DeflateStream( compressedStream, CompressionLevel.Fastest, true ) )
					{
						uncompressedStream.CopyTo( compressorStream );
					}

					compressedBytes = compressedStream.ToArray();
				}
			}

			return Convert.ToBase64String( compressedBytes );
		}

		public static string Decompress( string compressedString )
		{
			byte[] decompressedBytes;

			var compressedStream = new MemoryStream(Convert.FromBase64String(compressedString));

			using ( var decompressorStream = new DeflateStream( compressedStream, CompressionMode.Decompress ) )
			{
				using ( var decompressedStream = new MemoryStream() )
				{
					decompressorStream.CopyTo( decompressedStream );

					decompressedBytes = decompressedStream.ToArray();
				}
			}

			return Encoding.UTF8.GetString( decompressedBytes );
		}
	}
}