// Copyright OpenALPR Technology, Inc. 2018

/*
    Note:
    Don't copy the VideoOS.Platform.dll to the MIPPlugins directory. 
    The reason is that the plug-in must use the version that included in the XProtect VMS.
 */


using OpenALPRPlugin.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoOS.Platform;
using VideoOS.Platform.Data;

namespace OpenALPRPlugin.Background
{
    internal class BookmarksFinder
    {
        private readonly string optSearchStr;
        private readonly string[] optUsers;
        private readonly Item[] items;
        private readonly Guid[] kinds;

        internal BookmarksFinder(Item[] items, Guid[] Kinds, bool myOwnBookmarks, string optSearchStr)
        {
            if (items == null || items.Length == 0 || items[0] == null)
                throw new ArgumentNullException(nameof(items), "items cannot be null or empty.");

            this.items = items;
            this.kinds = Kinds;

            this.optSearchStr = optSearchStr;
            optUsers = myOwnBookmarks ? new string[] { $@"{Environment.MachineName}\{Environment.UserName}" } : null;
        }

        ~BookmarksFinder()
        {
        }

        internal async Task<Bookmark[]> Search(DateTime startTime, DateTime endTime, int bookmarksCount)
        {
            if (startTime <= endTime)
            {
                long timeLimitUSec = (endTime.Ticks - startTime.Ticks) / 10;
                return await BookmarkSearch(startTime, timeLimitUSec, bookmarksCount);
            }

            return new Bookmark[0];
        }

        //BookmarkSearchTime searches for bookmarks within a specific time interval
        //Search for bookmarks in a time interval. The call is synchronous, so it may take some time to return. 
        //Returns:    Array of Bookmarks found. Null indicates an error 
        private async Task<Bookmark[]> BookmarkSearch(DateTime startTime, long period, int bookmarksCount)
        {
            string searchStr = optSearchStr;
            List<Bookmark> bookmarks = new List<Bookmark>();

            if (string.IsNullOrEmpty(optSearchStr))
                searchStr = OpenALPRBackgroundPlugin.openalprRefrence;

            try
            {
                Logger.Log.Info("*********Search bookmark queries*********");
                foreach (string query in Helper.Queries(searchStr.Split(' ')))
                {
                    bookmarks.AddRange(BookmarkService.Instance.BookmarkSearchTime(
                        items[0].FQID.ServerId,
                        startTime,
                        period,
                        bookmarksCount + 1,
                        kinds,
                        new FQID[] { items[0].FQID },
                        optUsers,
                        query).ToList());

                    Logger.Log.Info(query);
                }

                Logger.Log.Info("*********Search bookmarks result*********");
                foreach (Bookmark bookmark in bookmarks.ToArray())
                {
                    Logger.Log.Info($"{bookmark.BookmarkFQID} - {bookmark.Description}");
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Search bookmarks error", ex);
            }

            try
            {
                return await Task.Run(() =>
                        bookmarks.ToArray()
                    );
                    //BookmarkService.Instance.BookmarkSearchTime(
                    //items[0].FQID.ServerId,         //The ServerId of the management server to be searched for Bookmarks.
                    //startTime,                      //Start time of the search interval. Mandatory
                    //period,                         //Period of time to search within (in microseconds). Mandatory
                    //bookmarksCount + 1,             //Maximum number of bookmarks to be returned in the result. Mandatory
                    //kinds,                          //The Kinds to be searched for. Null => all kinds
                    //new FQID[] { items[0].FQID },   //Array of Item identifications to search. Null => Any
                    //optUsers,                       //Array of User names (the users that created the bookmark). Null => Ignored
                    //searchStr.Replace(" ", "%")));                    //Search string. To appear in either of the fields 'Reference', 'Header', 'Description'. Null => Ignored
            }
            catch (Exception ex)
            {
                Logger.Log.Error(null, ex);
            }

            return new Bookmark[0];
        }

        internal async Task<Bookmark[]> Next(FQID bookmarkFQID, DateTime startTime, DateTime endTime, int bookmarksCount)
        {
            if (startTime <= endTime)
            {
                long timeLimitUSec = (endTime.Ticks - startTime.Ticks) / 10;
                return await BookmarkSearch(bookmarkFQID, timeLimitUSec, bookmarksCount);
            }

            return new Bookmark[0];
        }

        private async Task<Bookmark[]> BookmarkSearch(FQID bookmarkFQID, long period, int bookmarksCount)
        {
            string searchStr = optSearchStr;
            if (string.IsNullOrEmpty(optSearchStr))
                searchStr = OpenALPRBackgroundPlugin.openalprRefrence;

            try
            {
                return await Task.Run(() =>
                                BookmarkService.Instance.BookmarkSearchFromBookmark(
                                    bookmarkFQID,
                                    period,
                                    bookmarksCount + 1,
                                    kinds,
                                    new FQID[] { items[0].FQID },
                                    optUsers,
                                    searchStr));
            }
            catch (Exception ex)
            {
                Logger.Log.Error(null, ex);
            }

            return new Bookmark[0];
        }

        internal static async Task<Bookmark> CreateBookmark(FQID cameraFqid, DateTime timeBegin, DateTime timeEnd, string reference, string hedaer, string description)
        {
            try
            {
                return await Task.Run(() =>
                            BookmarkService.Instance.BookmarkCreate(
                                cameraFqid,
                                timeBegin,
                                timeBegin,
                                timeEnd,
                                reference,
                                hedaer,
                                description));
            }
            catch (Exception ex)
            {
                Logger.Log.Error(null, ex);
            }

            return null;
        }

        internal static async Task<bool> UpdateBookmark(FQID bookmarkFQID, string newHeader, string newDescription)
        {
            try
            {
                Bookmark bookmarkFetched = await Task.Run(() => BookmarkService.Instance.BookmarkGet(bookmarkFQID));
                if (bookmarkFetched != null)
                {
                    bookmarkFetched.Header = newHeader;
                    bookmarkFetched.Description = newDescription;
                    Bookmark bookmarkUpdated = await Task.Run(() => BookmarkService.Instance.BookmarkUpdate(bookmarkFetched));
                    return bookmarkUpdated != null && bookmarkUpdated.BookmarkFQID == bookmarkFQID;
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(null, ex);
            }

            return false;
        }

        internal static async Task<bool> DeleteBookmark(FQID bookmarkFQID)
        {
            try
            {
                await Task.Run(() => BookmarkService.Instance.BookmarkDelete(bookmarkFQID));
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log.Error(null, ex);
            }

            return false;
        }

        internal static async Task<Bookmark> GetBookmark(FQID bookmarkFQID)
        {
            try
            {
                return await Task.Run(() => BookmarkService.Instance.BookmarkGet(bookmarkFQID));
            }
            catch (Exception ex)
            {
                Logger.Log.Error(null, ex);
            }

            return null;
        }
    }
}