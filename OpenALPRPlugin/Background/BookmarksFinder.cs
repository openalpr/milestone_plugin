/*
    Developed by Khayralla AL-Athari
    email: khayralla_m@yahoo.com
    Dec 2017

    Note:
    Don't copy the VideoOS.Platform.dll to the MIPPlugins directory. 
    The reason is that the plug-in must use the version that included in the XProtect VMS.
 */


using OpenALPRPlugin.Utility;
using System;
using System.Threading.Tasks;
using VideoOS.Platform;
using VideoOS.Platform.Data;

namespace OpenALPRPlugin.Background
{
    internal class BookmarksFinder
    {
        private string optSearchStr;
        private string[] optUsers;
        private Item[] items;
        private Guid[] Kinds;

        internal BookmarksFinder(Item[] items, Guid[] Kinds, bool myOwnBookmarks, string optSearchStr)
        {
            if (items == null || items.Length == 0 || items[0] == null)
                throw new ArgumentNullException(nameof(items), "items cannot be null or empty.");

            this.items = items;
            this.Kinds = Kinds;

            this.optSearchStr = optSearchStr;
            optUsers = myOwnBookmarks ? new string[] { $@"{Environment.MachineName}\{Environment.UserName}" } : null;
        }

        ~BookmarksFinder()
        {
        }

        internal async Task<Bookmark[]> Search(DateTime startlocalTime, DateTime endlocalTime, int bookmarksCount)
        {
            if (startlocalTime <= endlocalTime)
            {
                long timeLimitUSec = (long)(endlocalTime - startlocalTime).TotalMilliseconds * 10000; // see https://developer.milestonesys.com/s/question/0D50O00003uvfSbSAI/retrieving-bookmarks-in-smart-client-plugin
                return await BookmarkSearch(startlocalTime, timeLimitUSec, bookmarksCount);
            }

            return new Bookmark[0];
        }

        //BookmarkSearchTime searches for bookmarks within a specific time interval
        //Search for bookmarks in a time interval. The call is synchronous, so it may take some time to return. 
        //Returns:    Array of Bookmarks found. Null indicates an error 
        private async Task<Bookmark[]> BookmarkSearch(DateTime startTime, long period, int bookmarksCount)
        {
            string searchStr = optSearchStr;
            if (string.IsNullOrEmpty(optSearchStr))
                searchStr = OpenALPRBackgroundPlugin.openalprRefrence;

            try
            {
                return await Task.Run(() =>
                    BookmarkService.Instance.BookmarkSearchTime(
                    items[0].FQID.ServerId,         //The ServerId of the management server to be searched for Bookmarks.
                    startTime,                      //Start time of the search interval. Mandatory
                    period,                         //Period of time to search within (in microseconds). Mandatory
                    bookmarksCount,                 //Maximum number of bookmarks to be returned in the result. Mandatory
                    Kinds,                          //The Kinds to be searched for. Null => all kinds
                    new FQID[] { items[0].FQID },   //Array of Item identifications to search. Null => Any
                    optUsers,                       //Array of User names (the users that created the bookmark). Null => Ignored
                    searchStr));                    //Search string. To appear in either of the fields 'Reference', 'Header', 'Description'. Null => Ignored
            }
            catch (Exception ex)
            {
                Logger.Log.Error(null, ex);
            }

            return new Bookmark[0];
        }

        internal async Task<Bookmark[]> Next(FQID bookmarkFQID, DateTime startlocalTime, DateTime endlocalTime, int bookmarksCount)
        {
            if (startlocalTime <= endlocalTime)
            {
                long timeLimitUSec = (long)(endlocalTime - startlocalTime).TotalMilliseconds * 10000; // see https://developer.milestonesys.com/s/question/0D50O00003uvfSbSAI/retrieving-bookmarks-in-smart-client-plugin
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
                                    bookmarksCount,
                                    Kinds,
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

        internal static async Task<bool> UpdateBookmark(FQID bookmarkFQID, string newReference, string newHeader, string newDescription)
        {
            try
            {
                var bookmarkFetched = await Task.Run(() => BookmarkService.Instance.BookmarkGet(bookmarkFQID));
                if (bookmarkFetched != null)
                {
                    bookmarkFetched.Reference = newReference;
                    bookmarkFetched.Header = newHeader;
                    bookmarkFetched.Description = newDescription;
                    var bookmarkUpdated = await Task.Run(() => BookmarkService.Instance.BookmarkUpdate(bookmarkFetched));
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