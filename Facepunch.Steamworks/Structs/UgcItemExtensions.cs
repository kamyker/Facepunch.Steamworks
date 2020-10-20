using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Steamworks.Data;

using QueryType = Steamworks.Ugc.Query;

namespace Steamworks.Ugc
{
	public static class ItemExtensions
	{
		public static string GetPreviewImageOrAdditionalPreviewImageUrl(this Item item)
		{
			if ( !string.IsNullOrEmpty( item.PreviewImageUrl ) )
				return item.PreviewImageUrl;
			if ( item.AdditionalPreviews != null )
			{
				foreach ( var p in item.AdditionalPreviews )
				{
					if ( p.ItemPreviewType == ItemPreviewType.Image && !string.IsNullOrEmpty( p.UrlOrVideoID ) )
						return p.UrlOrVideoID;
				}
			}
			return null;
		}

	}
}
