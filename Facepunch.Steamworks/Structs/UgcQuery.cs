using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Steamworks.Data;

using QueryType = Steamworks.Ugc.Query;

namespace Steamworks.Ugc
{
	public struct Query
	{
		UgcType matchingType;
		UGCQuery queryType;
		AppId consumerApp;
		AppId creatorApp;
        string searchText;

        Query( UgcType type ) : this()
        {
	        matchingType = type;
        }
        
		public Query( UgcType type = UgcType.All, UGCQuery allRankedBy = UGCQuery.RankedByVote, string optionalSearchText = null ) : this(type)
		{
			queryType = allRankedBy;
			if(!string.IsNullOrEmpty( optionalSearchText ))
			searchText = optionalSearchText; 
		}
		
		public Query( UgcType type = UgcType.All,
		              UserUGCList userQueryType = UserUGCList.Published,
		              UserUGCListSortOrder userSortType = UserUGCListSortOrder.CreationOrderDesc,
		              SteamId user = default ) : this(type)
		{
			userType = userQueryType;
			userSort = userSortType;
			steamid = user;
			if ( steamid == 0 )
				steamid = SteamClient.SteamId;
		}
		
		public Query( UgcType type, params PublishedFileId[] files ) : this(type)
		{
			Files = files;
		}
		
		#region UserQuery
		SteamId? steamid;
		readonly UserUGCList userType;
		readonly UserUGCListSortOrder userSort;
		#endregion

		#region Files
		readonly PublishedFileId[] Files;
		#endregion

		public async Task<ResultPage?> GetPageAsync( int page )
		{
			if ( page <= 0 ) throw new System.Exception( "page should be > 0" );

			if ( consumerApp == 0 ) consumerApp = SteamClient.AppId;
			if ( creatorApp == 0 ) creatorApp = consumerApp;

			UGCQueryHandle_t handle;

			if ( Files != null )
			{
				handle = SteamUGC.Internal.CreateQueryUGCDetailsRequest( Files, (uint)Files.Length );
			}
			else if ( steamid.HasValue )
			{
				handle = SteamUGC.Internal.CreateQueryUserUGCRequest( steamid.Value.AccountId, userType, matchingType, userSort, creatorApp.Value, consumerApp.Value, (uint)page );
			}
			else
			{
				handle = SteamUGC.Internal.CreateQueryAllUGCRequest( queryType, matchingType, creatorApp.Value, consumerApp.Value, (uint)page );
			}

		    ApplyReturns(handle);

		    if (maxCacheAge.HasValue)
		    {
		        SteamUGC.Internal.SetAllowCachedResponse(handle, (uint)maxCacheAge.Value);
		    }

			ApplyConstraints( handle );

			var result = await SteamUGC.Internal.SendQueryUGCRequest( handle );
			if ( !result.HasValue )
				return null;

			if ( result.Value.Result != Steamworks.Result.OK )
				return null;

			return new ResultPage
			{
				Handle = result.Value.Handle,
				ResultCount = (int) result.Value.NumResultsReturned,
				TotalCount = (int)result.Value.TotalMatchingResults,
				CachedData = result.Value.CachedData,
				ReturnsKeyValueTags = WantsReturnKeyValueTags ?? false,
				ReturnsDefaultStats = WantsDefaultStats ?? true, //true by default
				ReturnsMetadata = WantsReturnMetadata ?? false,
				ReturnsChildren = WantsReturnChildren ?? false,
				ReturnsAdditionalPreviews = WantsReturnAdditionalPreviews ?? false,
			};
		}

	    #region SharedConstraints
		public QueryType WithType( UgcType type ) { matchingType = type; return this; }
		int? maxCacheAge;
		public QueryType AllowCachedResponse( int maxSecondsAge ) { maxCacheAge = maxSecondsAge; return this; }
		string language;
		public QueryType InLanguage( string lang ) { language = lang; return this; }

		int? trendDays;
		public QueryType WithTrendDays( int days ) { trendDays = days; return this; }

		List<string> requiredTags;
		bool? matchAnyTag;
		List<string> excludedTags;
		Dictionary<string, string> requiredKv;

		/// <summary>
		/// Found items must have at least one of the defined tags
		/// </summary>
		public QueryType MatchAnyTag() { matchAnyTag = true; return this; }

		/// <summary>
		/// Found items must have all defined tags
		/// </summary>
		public QueryType MatchAllTags() { matchAnyTag = false; return this; }

		public QueryType WithTag( string tag )
		{
			if ( requiredTags == null ) requiredTags = new List<string>();
			requiredTags.Add( tag );
			return this;
		}

		public QueryType AddRequiredKeyValueTag(string key, string value)
		{
			if (requiredKv == null) requiredKv = new Dictionary<string, string>();
			requiredKv.Add(key, value);
			return this;
		}

		public QueryType WithoutTag( string tag )
		{
			if ( excludedTags == null ) excludedTags = new List<string>();
			excludedTags.Add( tag );
			return this;
		}

		void ApplyConstraints( UGCQueryHandle_t handle )
		{
			if ( requiredTags != null )
			{
				foreach ( var tag in requiredTags )
					SteamUGC.Internal.AddRequiredTag( handle, tag );
			}

			if ( excludedTags != null )
			{
				foreach ( var tag in excludedTags )
					SteamUGC.Internal.AddExcludedTag( handle, tag );
			}

			if ( requiredKv != null )
			{
				foreach ( var tag in requiredKv )
					SteamUGC.Internal.AddRequiredKeyValueTag( handle, tag.Key, tag.Value );
			}

			if ( matchAnyTag.HasValue )
			{
				SteamUGC.Internal.SetMatchAnyTag( handle, matchAnyTag.Value );
			}

			if ( trendDays.HasValue )
			{
				SteamUGC.Internal.SetRankedByTrendDays( handle, (uint)trendDays.Value );
			}

            if ( !string.IsNullOrEmpty( searchText ) )
            {
                SteamUGC.Internal.SetSearchText( handle, searchText );
            }
		}

        #endregion

        #region ReturnValues

	    bool? WantsReturnOnlyIDs;
	    public QueryType WithOnlyIDs(bool b) { WantsReturnOnlyIDs = b; return this; }
	    bool? WantsReturnKeyValueTags;
		public QueryType WithKeyValueTags(bool b) { WantsReturnKeyValueTags = b; return this; }
		[Obsolete( "Renamed to WithKeyValueTags" )]
        public QueryType WithKeyValueTag(bool b) { WantsReturnKeyValueTags = b; return this; }
	    bool? WantsReturnLongDescription;
	    public QueryType WithLongDescription(bool b) { WantsReturnLongDescription = b; return this; }
	    bool? WantsReturnMetadata;
	    public QueryType WithMetadata(bool b) { WantsReturnMetadata = b; return this; }
	    bool? WantsReturnChildren;
	    public QueryType WithChildren(bool b) { WantsReturnChildren = b; return this; }
	    bool? WantsReturnAdditionalPreviews;
	    public QueryType WithAdditionalPreviews(bool b) { WantsReturnAdditionalPreviews = b; return this; }
	    bool? WantsReturnTotalOnly;
	    public QueryType WithTotalOnly(bool b) { WantsReturnTotalOnly = b; return this; }
	    uint? WantsReturnPlaytimeStats;
	    public QueryType WithPlaytimeStats(uint unDays) { WantsReturnPlaytimeStats = unDays; return this; }

        private void ApplyReturns(UGCQueryHandle_t handle)
	    {
	        if (WantsReturnOnlyIDs.HasValue)
	        {
	            SteamUGC.Internal.SetReturnOnlyIDs(handle, WantsReturnOnlyIDs.Value);
	        }

	        if (WantsReturnKeyValueTags.HasValue)
	        {
	            SteamUGC.Internal.SetReturnKeyValueTags(handle, WantsReturnKeyValueTags.Value);
	        }

	        if (WantsReturnLongDescription.HasValue)
	        {
	            SteamUGC.Internal.SetReturnLongDescription(handle, WantsReturnLongDescription.Value);
	        }

	        if (WantsReturnMetadata.HasValue)
	        {
	            SteamUGC.Internal.SetReturnMetadata(handle, WantsReturnMetadata.Value);
	        }

	        if (WantsReturnChildren.HasValue)
	        {
	            SteamUGC.Internal.SetReturnChildren(handle, WantsReturnChildren.Value);
	        }

	        if (WantsReturnAdditionalPreviews.HasValue)
	        {
	            SteamUGC.Internal.SetReturnAdditionalPreviews(handle, WantsReturnAdditionalPreviews.Value);
	        }

	        if (WantsReturnTotalOnly.HasValue)
	        {
	            SteamUGC.Internal.SetReturnTotalOnly(handle, WantsReturnTotalOnly.Value);
	        }

	        if (WantsReturnPlaytimeStats.HasValue)
	        {
	            SteamUGC.Internal.SetReturnPlaytimeStats(handle, WantsReturnPlaytimeStats.Value);
	        }
	    }

        #endregion

		#region LoadingBehaviour

		bool? WantsDefaultStats; //true by default
		/// <summary>
		/// Set to false to disable, by default following stats are loaded: NumSubscriptions, NumFavorites, NumFollowers, NumUniqueSubscriptions, NumUniqueFavorites, NumUniqueFollowers, NumUniqueWebsiteViews, ReportScore, NumSecondsPlayed, NumPlaytimeSessions, NumComments, NumSecondsPlayedDuringTimePeriod, NumPlaytimeSessionsDuringTimePeriod
		/// </summary>
		public QueryType WithDefaultStats( bool b ) { WantsDefaultStats = b; return this; }

		#endregion
	}
}