using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Steamworks.Data;
using QueryType = Steamworks.Ugc.Query;

namespace Steamworks.Ugc
{
    public static class QueryExtensions
    {
        public static async Task<Item?> FirstOrNullAsync( this Query q )
        {
            using ( var result = await q.GetPageAsync( 1 ) )
            {
                if ( !result.HasValue )
                    return null;
                if ( result.Value.ResultCount == 0 ) 
                    return null;

                return result.Value.Entries.First();
            }
        }
    }
}