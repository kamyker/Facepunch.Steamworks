using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Steamworks.Data;
using UnityEngine;

namespace Steamworks.Ugc
{
	public struct SmartItem
	{
		public Item Item;

		public SmartItem( Item item ) : this()
		{
			Item = item;
		}

		/// <summary>
		/// The actual ID of this file
		/// </summary>
		public PublishedFileId Id => Item.Id;

		
		//TODO Check Title, Description, Tags if are set to null when using ReturnOnlyIds (and also all with details)
		/// <summary>
		/// The given title of this item
		/// </summary>
		public string Title => Item.Title;

		/// <summary>
		/// The description of this item, in your local language if available
		/// </summary>
		public string Description => Item.Description;

		/// <summary>
		/// A list of tags for this item, all lowercase
		/// </summary>
		public string[] Tags => Item.Tags;

		/// <summary>
		/// A dictionary of key value tags for this item, available only from WithKeyValueTags(true) queries
		/// </summary>
		public async Task<Dictionary<string, string>> KeyValueTags()
		{
			if ( Item.KeyValueTags == null )
			{
				Debug.Log( "Pulling workshop item KeyValueTags, consider using WithKeyValueTags with original query." );
				var newItem = await Item.GetQuery().WithDefaultStats( false ).WithKeyValueTags( true ).FirstOrNullAsync3Times();
				if( newItem.HasValue )
					Item.KeyValueTags = newItem.Value.KeyValueTags;
			}

			return Item.KeyValueTags;
		}

		/// <summary>
		/// App Id of the app that created this item
		/// </summary>
		public AppId CreatorApp => Item.details.CreatorAppID;

		/// <summary>
		/// App Id of the app that will consume this item.
		/// </summary>
		public AppId ConsumerApp => Item.details.ConsumerAppID;

		/// <summary>
		/// User who created this content
		/// </summary>
		public Friend Owner => new Friend( Item.details.SteamIDOwner );

		/// <summary>
		/// The bayesian average for up votes / total votes, between [0,1]
		/// </summary>
		public float Score => Item.details.Score;

		/// <summary>
		/// Time when the published item was created
		/// </summary>
		public DateTime Created => Epoch.ToDateTime( Item.details.TimeCreated );

		/// <summary>
		/// Time when the published item was last updated
		/// </summary>
		public DateTime Updated => Epoch.ToDateTime( Item.details.TimeUpdated );

		/// <summary>
		/// True if this is publically visible
		/// </summary>
		public bool IsPublic => Item.details.Visibility == RemoteStoragePublishedFileVisibility.Public;

		/// <summary>
		/// True if this item is only visible by friends of the creator
		/// </summary>
		public bool IsFriendsOnly => Item.details.Visibility == RemoteStoragePublishedFileVisibility.FriendsOnly;

		/// <summary>
		/// True if this is only visible to the creator
		/// </summary>
		public bool IsPrivate => Item.details.Visibility == RemoteStoragePublishedFileVisibility.Private;
		
		/// <summary>
		/// True if this item has been banned
		/// </summary>
		public bool IsBanned => Item.details.Banned;

		/// <summary>
		/// Whether the developer of this app has specifically flagged this item as accepted in the Workshop
		/// </summary>
		public bool IsAcceptedForUse => Item.details.AcceptedForUse;

		/// <summary>
		/// The number of upvotes of this item
		/// </summary>
		public uint VotesUp => Item.details.VotesUp;

		/// <summary>
		/// The number of downvotes of this item
		/// </summary>
		public uint VotesDown => Item.details.VotesDown;
        
		/// <summary>
		/// Dependencies/children of this item or collection, available only from WithDependencies(true) queries
		/// </summary>
		public async Task<PublishedFileId[]> Children()
		{
			if ( Item.Children == null )
			{
				Debug.Log( "Pulling workshop item Children, consider using WithChildren with original query." );
				var newItem = await Item.GetQuery().WithDefaultStats( false ).WithChildren( true ).FirstOrNullAsync3Times();
				if( newItem.HasValue )
					Item.Children = newItem.Value.Children;
			}

			return Item.Children;
		}

		/// <summary>
		/// Additional previews of this item or collection, available only from WithAdditionalPreviews(true) queries
		/// </summary>
		public async Task<UgcAdditionalPreview[]> AdditionalPreviews()
		{
			if ( Item.AdditionalPreviews == null )
			{
				Debug.Log( "Pulling workshop item AdditionalPreviews, consider using WithAdditionalPreviews with original query." );
				var newItem = await Item.GetQuery().WithDefaultStats( false ).WithAdditionalPreviews( true ).FirstOrNullAsync3Times();
				if( newItem.HasValue )
					Item.AdditionalPreviews = newItem.Value.AdditionalPreviews;
			}

			return Item.AdditionalPreviews;
		}

		public bool IsInstalled => (State & ItemState.Installed) == ItemState.Installed;
		public bool IsDownloading => (State & ItemState.Downloading) == ItemState.Downloading;
		public bool IsDownloadPending => (State & ItemState.DownloadPending) == ItemState.DownloadPending;
		public bool IsSubscribed => (State & ItemState.Subscribed) == ItemState.Subscribed;
		public bool NeedsUpdate => (State & ItemState.NeedsUpdate) == ItemState.NeedsUpdate;

		public string Directory => Item.Directory;

		/// <summary>
		/// Start downloading this item.
		/// If this returns false the item isn't getting downloaded.
		/// </summary>
		public bool Download( bool highPriority = false )
		{
			return Item.Download( highPriority );
		}

		/// <summary>
		/// If we're downloading, how big the total download is 
		/// </summary>
		public long DownloadBytesTotal => Item.DownloadBytesTotal;

		/// <summary>
		/// If we're downloading, how much we've downloaded
		/// </summary>
		public long DownloadBytesDownloaded  => Item.DownloadBytesDownloaded;

		/// <summary>
		/// If we're installed, how big is the install
		/// </summary>
		public long SizeBytes => Item.DownloadBytesDownloaded;

		/// <summary>
		/// If we're downloading our current progress as a delta betwen 0-1
		/// </summary>
		public float DownloadAmount => Item.DownloadAmount;

		internal ItemState State => Item.State;

		public static async Task<Item?> GetAsync( PublishedFileId id, bool withLongDescription = true, int maxAgeSeconds = 0  )
		{
			return await Item.GetAsync( id, withLongDescription, maxAgeSeconds );
		}

		/// <summary>
		/// Queries all favorite items (of current user) looking for this one.
		/// </summary>
		public async Task<bool> IsFavoriteAsync( )
		{
			return await Item.IsFavoriteAsync( );
		}

		/// <summary>
		/// A case insensitive check for tag
		/// </summary>
		public bool HasTag( string find ) => Item.HasTag( find );

		/// <summary>
		/// Allows the user to subscribe to this item
		/// </summary>
		public async Task<bool> Subscribe() => await Item.Subscribe();

		/// <summary>
		/// Allows the user to subscribe to download this item asyncronously
		/// If CancellationToken is default then there is 60 seconds timeout
		/// Progress will be set to 0-1
		/// </summary>
		public async Task<bool> DownloadAsync( Action<float> progress = null, Action<string> onError = null, int milisecondsUpdateDelay = 60, CancellationToken ct = default, bool highPriority = true )
		{
			return await Item.DownloadAsync( progress, onError, milisecondsUpdateDelay, ct, highPriority );
		}

		/// <summary>
		/// Allows the user to unsubscribe from this item
		/// </summary>
		public async Task<bool> Unsubscribe ()
		{
			return await Item.Unsubscribe();
		}

		/// <summary>
		/// Adds item to user favorite list
		/// </summary>
		public async Task<bool> AddFavorite()
		{
			return await Item.AddFavorite();
		}

		/// <summary>
		/// Removes item from user favorite list
		/// </summary>
		public async Task<bool> RemoveFavorite()
		{
			return await Item.RemoveFavorite();
		}

		/// <summary>
		/// Allows the user to rate a workshop item up or down.
		/// </summary>
		public async Task<Result?> Vote( bool up )
		{
			return await Item.Vote( up );
		}

		/// <summary>
		/// Gets the current users vote on the item
		/// </summary>
		public async Task<UserItemVote?> GetUserVote()
		{
			return await Item.GetUserVote();
		}

		/// <summary>
		/// Return a URL to view this item online
		/// </summary>
		public string Url => Item.Url;

		/// <summary>
		/// The URl to view this item's changelog
		/// </summary>
		public string ChangelogUrl => Item.ChangelogUrl;

		/// <summary>
		/// The URL to view the comments on this item
		/// </summary>
		public string CommentsUrl => Item.CommentsUrl;

		/// <summary>
		/// The URL to discuss this item
		/// </summary>
		public string DiscussUrl => Item.DiscussUrl;

		/// <summary>
		/// The URL to view this items stats online
		/// </summary>
		public string StatsUrl => Item.StatsUrl;

		public async Task<ulong> NumSubscriptions() { await PullDefaultStats(); return Item.NumSubscriptions; }
		public async Task<ulong> NumFavorites() { await PullDefaultStats(); return Item.NumFavorites; }
		public async Task<ulong> NumFollowers() { await PullDefaultStats(); return Item.NumFollowers; }
		public async Task<ulong> NumUniqueSubscriptions() { await PullDefaultStats(); return Item.NumUniqueSubscriptions; }
		public async Task<ulong> NumUniqueFavorites() { await PullDefaultStats(); return Item.NumUniqueFavorites; }
		public async Task<ulong> NumUniqueFollowers() { await PullDefaultStats(); return Item.NumUniqueFollowers; }
		public async Task<ulong> NumUniqueWebsiteViews() { await PullDefaultStats(); return Item.NumUniqueWebsiteViews; }
		public async Task<ulong> ReportScore() { await PullDefaultStats(); return Item.ReportScore; }
		public async Task<ulong> NumSecondsPlayed() { await PullDefaultStats(); return Item.NumSecondsPlayed; }
		public async Task<ulong> NumPlaytimeSessions() { await PullDefaultStats(); return Item.NumPlaytimeSessions; }
		public async Task<ulong> NumComments() { await PullDefaultStats(); return Item.NumComments; }
		public async Task<ulong> NumSecondsPlayedDuringTimePeriod() { await PullDefaultStats(); return Item.NumSecondsPlayedDuringTimePeriod; }
		public async Task<ulong> NumPlaytimeSessionsDuringTimePeriod() { await PullDefaultStats(); return Item.NumPlaytimeSessionsDuringTimePeriod; }

		async Task PullDefaultStats()
		{
			if ( Item.AreDefaultStatsPulled == false )
			{
				Debug.Log( "Pulling workshop item DefaultStats, consider using WithDefaultStats with original query." );
				var newItem = await Item.GetQuery().WithDefaultStats( true ).FirstOrNullAsync3Times();
				if ( !newItem.HasValue )
					return;
				var newItemVal = newItem.Value;
				Item.NumFavorites = newItemVal.NumFavorites;
				Item.NumFollowers = newItemVal.NumFollowers;
				Item.NumUniqueSubscriptions = newItemVal.NumUniqueSubscriptions;
				Item.NumUniqueFavorites = newItemVal.NumUniqueFavorites;
				Item.NumUniqueFollowers = newItemVal.NumUniqueFollowers;
				Item.NumUniqueWebsiteViews = newItemVal.NumUniqueWebsiteViews;
				Item.ReportScore = newItemVal.ReportScore;
				Item.NumSecondsPlayed = newItemVal.NumSecondsPlayed;
				Item.NumPlaytimeSessions = newItemVal.NumPlaytimeSessions;
				Item.NumComments = newItemVal.NumComments;
				Item.NumSecondsPlayedDuringTimePeriod = newItemVal.NumSecondsPlayedDuringTimePeriod;
				Item.NumPlaytimeSessionsDuringTimePeriod = newItemVal.NumPlaytimeSessionsDuringTimePeriod;
			}
		}

		/// <summary>
		/// The URL to the preview image for this item, available from all queries
		/// </summary>
		public string PreviewImageUrl => Item.PreviewImageUrl;

		/// <summary>
		/// The metadata string for this item, only available from queries WithMetadata(true)
		/// </summary>
		public async Task<string> Metadata()
		{
			if ( Item.Metadata == null )
			{
				Debug.Log( "Pulling workshop item Metadata, consider using WithMetadata with original query." );
				var newItem = await Item.GetQuery().WithDefaultStats( false ).WithMetadata( true ).FirstOrNullAsync3Times();
				if( newItem.HasValue )
					Item.Metadata = newItem.Value.Metadata;
			}

			return Item.Metadata;
		}
		
		/// <summary>
		/// Edit this item
		/// </summary>
		public Ugc.Editor Edit()
		{
			return Item.Edit();
		}

		public async Task<bool> AddDependency( PublishedFileId child )
		{
			return await Item.AddDependency( child );
		}

		public async Task<bool> RemoveDependency( PublishedFileId child )
		{
			return await Item.RemoveDependency( child );
		}

		public Result Result => Item.Result;
	}
}