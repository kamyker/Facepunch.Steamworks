﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace Steamworks
{
	internal static class Platform
    {
#if PLATFORM_WIN64
		public const int StructPlatformPackSize = 8;
		public const string LibraryName = "steam_api64";
		public const CallingConvention CC = CallingConvention.Cdecl;
#elif PLATFORM_WIN32
		public const int StructPlatformPackSize = 8;
		public const string LibraryName = "steam_api";
		public const CallingConvention CC = CallingConvention.Cdecl;
#elif PLATFORM_POSIX32
		public const int StructPlatformPackSize = 4;
		public const string LibraryName = "libsteam_api";
		public const CallingConvention CC = CallingConvention.Cdecl;
#elif PLATFORM_POSIX64
		public const int StructPlatformPackSize = 4;
		public const string LibraryName = "libsteam_api";
		public const CallingConvention CC = CallingConvention.Cdecl;
#endif

		public const int StructPackSize = 4;
	}
}
