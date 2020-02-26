using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Steamworks.Data;


namespace Steamworks
{
	internal class ISteamNetworking : SteamInterface
	{
		
		internal ISteamNetworking( bool IsGameServer )
		{
			SetupInterface( IsGameServer );
		}
		
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_SteamNetworking_v006", CallingConvention = Platform.CC)]
		internal static extern IntPtr SteamAPI_SteamNetworking_v006();
		public override IntPtr GetUserInterfacePointer() => SteamAPI_SteamNetworking_v006();
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_SteamGameServerNetworking_v006", CallingConvention = Platform.CC)]
		internal static extern IntPtr SteamAPI_SteamGameServerNetworking_v006();
		public override IntPtr GetServerInterfacePointer() => SteamAPI_SteamGameServerNetworking_v006();
		
		
		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamNetworking_SendP2PPacket", CallingConvention = Platform.CC)]
		[return: MarshalAs( UnmanagedType.I1 )]
		private static extern bool _SendP2PPacket( IntPtr self, SteamId steamIDRemote, IntPtr pubData, uint cubData, P2PSend eP2PSendType, int nChannel );
		
		#endregion
		internal bool SendP2PPacket( SteamId steamIDRemote, IntPtr pubData, uint cubData, P2PSend eP2PSendType, int nChannel )
		{
			var returnValue = _SendP2PPacket( Self, steamIDRemote, pubData, cubData, eP2PSendType, nChannel );
			return returnValue;
		}
		
		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamNetworking_IsP2PPacketAvailable", CallingConvention = Platform.CC)]
		[return: MarshalAs( UnmanagedType.I1 )]
		private static extern bool _IsP2PPacketAvailable( IntPtr self, ref uint pcubMsgSize, int nChannel );
		
		#endregion
		internal bool IsP2PPacketAvailable( ref uint pcubMsgSize, int nChannel )
		{
			var returnValue = _IsP2PPacketAvailable( Self, ref pcubMsgSize, nChannel );
			return returnValue;
		}
		
		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamNetworking_ReadP2PPacket", CallingConvention = Platform.CC)]
		[return: MarshalAs( UnmanagedType.I1 )]
		private static extern bool _ReadP2PPacket( IntPtr self, IntPtr pubDest, uint cubDest, ref uint pcubMsgSize, ref SteamId psteamIDRemote, int nChannel );
		
		#endregion
		internal bool ReadP2PPacket( IntPtr pubDest, uint cubDest, ref uint pcubMsgSize, ref SteamId psteamIDRemote, int nChannel )
		{
			var returnValue = _ReadP2PPacket( Self, pubDest, cubDest, ref pcubMsgSize, ref psteamIDRemote, nChannel );
			return returnValue;
		}
		
		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamNetworking_AcceptP2PSessionWithUser", CallingConvention = Platform.CC)]
		[return: MarshalAs( UnmanagedType.I1 )]
		private static extern bool _AcceptP2PSessionWithUser( IntPtr self, SteamId steamIDRemote );
		
		#endregion
		internal bool AcceptP2PSessionWithUser( SteamId steamIDRemote )
		{
			var returnValue = _AcceptP2PSessionWithUser( Self, steamIDRemote );
			return returnValue;
		}
		
		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamNetworking_CloseP2PSessionWithUser", CallingConvention = Platform.CC)]
		[return: MarshalAs( UnmanagedType.I1 )]
		private static extern bool _CloseP2PSessionWithUser( IntPtr self, SteamId steamIDRemote );
		
		#endregion
		internal bool CloseP2PSessionWithUser( SteamId steamIDRemote )
		{
			var returnValue = _CloseP2PSessionWithUser( Self, steamIDRemote );
			return returnValue;
		}
		
		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamNetworking_CloseP2PChannelWithUser", CallingConvention = Platform.CC)]
		[return: MarshalAs( UnmanagedType.I1 )]
		private static extern bool _CloseP2PChannelWithUser( IntPtr self, SteamId steamIDRemote, int nChannel );
		
		#endregion
		internal bool CloseP2PChannelWithUser( SteamId steamIDRemote, int nChannel )
		{
			var returnValue = _CloseP2PChannelWithUser( Self, steamIDRemote, nChannel );
			return returnValue;
		}
		
		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamNetworking_GetP2PSessionState", CallingConvention = Platform.CC)]
		[return: MarshalAs( UnmanagedType.I1 )]
		private static extern bool _GetP2PSessionState( IntPtr self, SteamId steamIDRemote, ref P2PSessionState_t pConnectionState );
		
		#endregion
		internal bool GetP2PSessionState( SteamId steamIDRemote, ref P2PSessionState_t pConnectionState )
		{
			var returnValue = _GetP2PSessionState( Self, steamIDRemote, ref pConnectionState );
			return returnValue;
		}
		
		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamNetworking_AllowP2PPacketRelay", CallingConvention = Platform.CC)]
		[return: MarshalAs( UnmanagedType.I1 )]
		private static extern bool _AllowP2PPacketRelay( IntPtr self, [MarshalAs( UnmanagedType.U1 )] bool bAllow );
		
		#endregion
		internal bool AllowP2PPacketRelay( [MarshalAs( UnmanagedType.U1 )] bool bAllow )
		{
			var returnValue = _AllowP2PPacketRelay( Self, bAllow );
			return returnValue;
		}
		
		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamNetworking_CreateListenSocket", CallingConvention = Platform.CC)]
		private static extern SNetListenSocket_t _CreateListenSocket( IntPtr self, int nVirtualP2PPort, SteamIPAddress nIP, ushort nPort, [MarshalAs( UnmanagedType.U1 )] bool bAllowUseOfPacketRelay );
		
		#endregion
		internal SNetListenSocket_t CreateListenSocket( int nVirtualP2PPort, SteamIPAddress nIP, ushort nPort, [MarshalAs( UnmanagedType.U1 )] bool bAllowUseOfPacketRelay )
		{
			var returnValue = _CreateListenSocket( Self, nVirtualP2PPort, nIP, nPort, bAllowUseOfPacketRelay );
			return returnValue;
		}
		
		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamNetworking_CreateP2PConnectionSocket", CallingConvention = Platform.CC)]
		private static extern SNetSocket_t _CreateP2PConnectionSocket( IntPtr self, SteamId steamIDTarget, int nVirtualPort, int nTimeoutSec, [MarshalAs( UnmanagedType.U1 )] bool bAllowUseOfPacketRelay );
		
		#endregion
		internal SNetSocket_t CreateP2PConnectionSocket( SteamId steamIDTarget, int nVirtualPort, int nTimeoutSec, [MarshalAs( UnmanagedType.U1 )] bool bAllowUseOfPacketRelay )
		{
			var returnValue = _CreateP2PConnectionSocket( Self, steamIDTarget, nVirtualPort, nTimeoutSec, bAllowUseOfPacketRelay );
			return returnValue;
		}
		
		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamNetworking_CreateConnectionSocket", CallingConvention = Platform.CC)]
		private static extern SNetSocket_t _CreateConnectionSocket( IntPtr self, SteamIPAddress nIP, ushort nPort, int nTimeoutSec );
		
		#endregion
		internal SNetSocket_t CreateConnectionSocket( SteamIPAddress nIP, ushort nPort, int nTimeoutSec )
		{
			var returnValue = _CreateConnectionSocket( Self, nIP, nPort, nTimeoutSec );
			return returnValue;
		}
		
		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamNetworking_DestroySocket", CallingConvention = Platform.CC)]
		[return: MarshalAs( UnmanagedType.I1 )]
		private static extern bool _DestroySocket( IntPtr self, SNetSocket_t hSocket, [MarshalAs( UnmanagedType.U1 )] bool bNotifyRemoteEnd );
		
		#endregion
		internal bool DestroySocket( SNetSocket_t hSocket, [MarshalAs( UnmanagedType.U1 )] bool bNotifyRemoteEnd )
		{
			var returnValue = _DestroySocket( Self, hSocket, bNotifyRemoteEnd );
			return returnValue;
		}
		
		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamNetworking_DestroyListenSocket", CallingConvention = Platform.CC)]
		[return: MarshalAs( UnmanagedType.I1 )]
		private static extern bool _DestroyListenSocket( IntPtr self, SNetListenSocket_t hSocket, [MarshalAs( UnmanagedType.U1 )] bool bNotifyRemoteEnd );
		
		#endregion
		internal bool DestroyListenSocket( SNetListenSocket_t hSocket, [MarshalAs( UnmanagedType.U1 )] bool bNotifyRemoteEnd )
		{
			var returnValue = _DestroyListenSocket( Self, hSocket, bNotifyRemoteEnd );
			return returnValue;
		}
		
		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamNetworking_SendDataOnSocket", CallingConvention = Platform.CC)]
		[return: MarshalAs( UnmanagedType.I1 )]
		private static extern bool _SendDataOnSocket( IntPtr self, SNetSocket_t hSocket, [In,Out] IntPtr[]  pubData, uint cubData, [MarshalAs( UnmanagedType.U1 )] bool bReliable );
		
		#endregion
		internal bool SendDataOnSocket( SNetSocket_t hSocket, [In,Out] IntPtr[]  pubData, uint cubData, [MarshalAs( UnmanagedType.U1 )] bool bReliable )
		{
			var returnValue = _SendDataOnSocket( Self, hSocket, pubData, cubData, bReliable );
			return returnValue;
		}
		
		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamNetworking_IsDataAvailableOnSocket", CallingConvention = Platform.CC)]
		[return: MarshalAs( UnmanagedType.I1 )]
		private static extern bool _IsDataAvailableOnSocket( IntPtr self, SNetSocket_t hSocket, ref uint pcubMsgSize );
		
		#endregion
		internal bool IsDataAvailableOnSocket( SNetSocket_t hSocket, ref uint pcubMsgSize )
		{
			var returnValue = _IsDataAvailableOnSocket( Self, hSocket, ref pcubMsgSize );
			return returnValue;
		}
		
		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamNetworking_RetrieveDataFromSocket", CallingConvention = Platform.CC)]
		[return: MarshalAs( UnmanagedType.I1 )]
		private static extern bool _RetrieveDataFromSocket( IntPtr self, SNetSocket_t hSocket, [In,Out] IntPtr[]  pubDest, uint cubDest, ref uint pcubMsgSize );
		
		#endregion
		internal bool RetrieveDataFromSocket( SNetSocket_t hSocket, [In,Out] IntPtr[]  pubDest, uint cubDest, ref uint pcubMsgSize )
		{
			var returnValue = _RetrieveDataFromSocket( Self, hSocket, pubDest, cubDest, ref pcubMsgSize );
			return returnValue;
		}
		
		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamNetworking_IsDataAvailable", CallingConvention = Platform.CC)]
		[return: MarshalAs( UnmanagedType.I1 )]
		private static extern bool _IsDataAvailable( IntPtr self, SNetListenSocket_t hListenSocket, ref uint pcubMsgSize, ref SNetSocket_t phSocket );
		
		#endregion
		internal bool IsDataAvailable( SNetListenSocket_t hListenSocket, ref uint pcubMsgSize, ref SNetSocket_t phSocket )
		{
			var returnValue = _IsDataAvailable( Self, hListenSocket, ref pcubMsgSize, ref phSocket );
			return returnValue;
		}
		
		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamNetworking_RetrieveData", CallingConvention = Platform.CC)]
		[return: MarshalAs( UnmanagedType.I1 )]
		private static extern bool _RetrieveData( IntPtr self, SNetListenSocket_t hListenSocket, [In,Out] IntPtr[]  pubDest, uint cubDest, ref uint pcubMsgSize, ref SNetSocket_t phSocket );
		
		#endregion
		internal bool RetrieveData( SNetListenSocket_t hListenSocket, [In,Out] IntPtr[]  pubDest, uint cubDest, ref uint pcubMsgSize, ref SNetSocket_t phSocket )
		{
			var returnValue = _RetrieveData( Self, hListenSocket, pubDest, cubDest, ref pcubMsgSize, ref phSocket );
			return returnValue;
		}
		
		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamNetworking_GetSocketInfo", CallingConvention = Platform.CC)]
		[return: MarshalAs( UnmanagedType.I1 )]
		private static extern bool _GetSocketInfo( IntPtr self, SNetSocket_t hSocket, ref SteamId pSteamIDRemote, ref int peSocketStatus, ref SteamIPAddress punIPRemote, ref ushort punPortRemote );
		
		#endregion
		internal bool GetSocketInfo( SNetSocket_t hSocket, ref SteamId pSteamIDRemote, ref int peSocketStatus, ref SteamIPAddress punIPRemote, ref ushort punPortRemote )
		{
			var returnValue = _GetSocketInfo( Self, hSocket, ref pSteamIDRemote, ref peSocketStatus, ref punIPRemote, ref punPortRemote );
			return returnValue;
		}
		
		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamNetworking_GetListenSocketInfo", CallingConvention = Platform.CC)]
		[return: MarshalAs( UnmanagedType.I1 )]
		private static extern bool _GetListenSocketInfo( IntPtr self, SNetListenSocket_t hListenSocket, ref SteamIPAddress pnIP, ref ushort pnPort );
		
		#endregion
		internal bool GetListenSocketInfo( SNetListenSocket_t hListenSocket, ref SteamIPAddress pnIP, ref ushort pnPort )
		{
			var returnValue = _GetListenSocketInfo( Self, hListenSocket, ref pnIP, ref pnPort );
			return returnValue;
		}
		
		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamNetworking_GetSocketConnectionType", CallingConvention = Platform.CC)]
		private static extern SNetSocketConnectionType _GetSocketConnectionType( IntPtr self, SNetSocket_t hSocket );
		
		#endregion
		internal SNetSocketConnectionType GetSocketConnectionType( SNetSocket_t hSocket )
		{
			var returnValue = _GetSocketConnectionType( Self, hSocket );
			return returnValue;
		}
		
		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamNetworking_GetMaxPacketSize", CallingConvention = Platform.CC)]
		private static extern int _GetMaxPacketSize( IntPtr self, SNetSocket_t hSocket );
		
		#endregion
		internal int GetMaxPacketSize( SNetSocket_t hSocket )
		{
			var returnValue = _GetMaxPacketSize( Self, hSocket );
			return returnValue;
		}
		
	}
}
