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
        
        public static async Task<Item?> FirstOrNullAsync3Times( this Query q )
        {
            for ( int i = 0; i < 3; i++ )
            {
                var res = await q.FirstOrNullAsync();
                if ( res.HasValue )
                    return res;
            }
            return null;
        }
    }
}