using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Steamworks.Data;
using UnityEngine.XR;

namespace Steamworks
{
	/// <summary>
	/// Functions to control music playback in the steam client.
	/// This gives games the opportunity to do things like pause the music or lower the volume, 
	/// when an important cut scene is shown, and start playing afterwards.
	/// Nothing uses Steam Music though so this can probably get fucked
	/// </summary>
	public class SteamHTMLSurface : SteamClientClass<SteamHTMLSurface>
	{
		internal static ISteamHTMLSurface Internal => Interface as ISteamHTMLSurface;

		internal override void InitializeInterface ( bool server )
		{
			SetInterface( server, new ISteamHTMLSurface( server ) );

			InstallEvents();
		}

		internal static void InstallEvents ()
		{
			Dispatch.Install<HTML_StartRequest_t>( x => OnHTML_StartRequested?.Invoke( x ) );
			Dispatch.Install<HTML_NeedsPaint_t>( x => OnHTML_NeedsPaint?.Invoke( (x.PBGRA, x.UnWide,x.UnTall) ) );
		}

		private static event Action<HTML_StartRequest_t> OnHTML_StartRequested;

		/// <summary>
		/// Called when a browser surface has a pending paint. This is where you get the actual image data to render to the screen.
		/// </summary>
		public static event Action<(string image, uint width, uint height)> OnHTML_NeedsPaint;

		public static void CloseBrowser ( uint handle )
		{
			Internal.RemoveBrowser( handle );
			Internal.Shutdown();
		}

		public static async Task<uint?> OpenBrowser (
			string url,
			uint pxWidth,
			uint pxHeight,
			IMouseWheelSetter mouseWheelSetter = null,
			string userAgent = null,
			string userCSS = null,
			string postData = null )
		{
			uint? handle = null;

			Internal.Init();

			HTML_BrowserReady_t? result = await Internal.CreateBrowser( userAgent, userCSS );

			if ( !result.HasValue )
			{
				Internal.Shutdown();
				return null;
			}
			HTML_BrowserReady_t browser = result.Value;
			handle = browser.UnBrowserHandle;

			mouseWheelSetter.OnMouseWheelSet += SetMouseWheel;

			Internal.SetSize( handle.Value, pxWidth, pxHeight );

			bool requestDone = false;

			Action<HTML_StartRequest_t> RequestFinished = x =>
				{
					requestDone = true;
					Internal.AllowStartRequest( handle.Value, true );
				};

			try
			{
				OnHTML_StartRequested += RequestFinished;

				while ( !requestDone )
					await Task.Delay( 1 );
			}
			finally
			{
				OnHTML_StartRequested -= RequestFinished;
			}

			//triggers HTML_StartRequest_t
			Internal.LoadURL( handle.Value, url, postData );

			return handle;

			void SetMouseWheel ( int pxDelta )
			{
				Internal.MouseWheel( handle.Value, pxDelta );
			}
		}

		public interface IMouseWheelSetter
		{
			event Action<int> OnMouseWheelSet;
		}
	}
}