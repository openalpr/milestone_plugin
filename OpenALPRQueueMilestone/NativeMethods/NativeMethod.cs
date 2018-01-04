using OpenALPRQueueConsumer.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenALPRQueueConsumer.NativeMethods
{
    internal static class NativeMethod
    {
        /// <summary>
        /// Attempts to log a user on to the local computer.
        /// </summary>
        /// <param name="username">This is the name of the user account to log on to. 
        /// If you use the user principal name (UPN) format, user@DNSdomainname, the 
        /// domain parameter must be <c>null</c>.</param>
        /// <param name="domain">Specifies the name of the domain or server whose 
        /// account database contains the lpszUsername account. If this parameter 
        /// is <c>null</c>, the user name must be specified in UPN format. If this 
        /// parameter is ".", the function validates the account by using only the 
        /// local account database.</param>
        /// <param name="password">The password</param>
        /// <param name="logonType">The logon type</param>
        /// <param name="logonProvider">The logon provides</param>
        /// <param name="userToken">The out parameter that will contain the user 
        /// token when method succeeds.</param>
        /// <returns><c>True</c> when succeeded; otherwise <c>false</c>.</returns>
        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool LogonUser(string username, string domain, string password, LogonType logonType, LogonProvider logonProvider, out IntPtr userToken);

        /// <summary>
        /// Creates a new access token that duplicates one already in existence.
        /// </summary>
        /// <param name="token">Handle to an access token.</param>
        /// <param name="impersonationLevel">The impersonation level.</param>
        /// <param name="duplication">Reference to the token to duplicate.</param>
        /// <returns></returns>
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool DuplicateToken(IntPtr token, int impersonationLevel, ref IntPtr duplication);

        /// <summary>
        /// Closes an open object handle.
        /// </summary>
        /// <param name="hObject">A handle to an open object.</param>
        /// <returns><c>True</c> when succeeded; otherwise <c>false</c>.</returns>
        [DllImport("kernel32.dll")]
        internal static extern bool CloseHandle(IntPtr hObject);


        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern bool CheckRemoteDebuggerPresent(IntPtr hProcess, ref bool isDebuggerPresent);

    }
}
