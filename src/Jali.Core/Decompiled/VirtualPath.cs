// ReSharper disable All
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using Jali.Core;
using Jali.Core.Decompiled.System.Web.Util;

namespace Jali.Core.Decompiled.System.Web
{
    internal sealed class VirtualPath : IComparable
    {
        private string _appRelativeVirtualPath;
        private string _virtualPath;

        // const masks into the BitVector32
        private const int isWithinAppRootComputed = 0x00000001;
        private const int isWithinAppRoot = 0x00000002;
        private const int appRelativeAttempted = 0x00000004;

        private int flags;

        internal static VirtualPath RootVirtualPath = VirtualPath.Create("/");

        private VirtualPath() { }

        // This is called to set the appropriate virtual path field when we already know
        // that the path is generally well formed.
        private VirtualPath(string virtualPath)
        {
            _virtualPath = virtualPath;
        }

        int IComparable.CompareTo(object obj)
        {

            VirtualPath virtualPath = obj as VirtualPath;

            // Make sure we're compared to another VirtualPath
            if (virtualPath == null)
                throw new ArgumentException();

            // Check if it's the same object
            if (virtualPath == this)
                return 0;

            return string.Compare(
                this.VirtualPathString, virtualPath.VirtualPathString, StringComparison.OrdinalIgnoreCase);
        }

        public string VirtualPathString
        {
            get
            {
                if (_virtualPath != null) return _virtualPath;

                var message = $"App relative virtual paths not supported. Your is: '{_appRelativeVirtualPath}'";
                throw new InvalidOperationException(_appRelativeVirtualPath);
            }
        }

        internal string VirtualPathStringNoTrailingSlash
        {
            get
            {
                return UrlPath.RemoveSlashFromPathIfNeeded(VirtualPathString);
            }
        }

        // Return the virtual path string if we have it, otherwise null
        internal string VirtualPathStringIfAvailable
        {
            get
            {
                return _virtualPath;
            }
        }


        // Return the app relative virtual path string if we have it, otherwise null
        internal string AppRelativeVirtualPathStringIfAvailable
        {
            get
            {
                return _appRelativeVirtualPath;
            }
        }
        // Return the virtual string that's either app relative or not, depending on which
        // one we already have internally.  If we have both, we return absolute
        internal string VirtualPathStringWhicheverAvailable
        {
            get
            {
                return _virtualPath != null ? _virtualPath : _appRelativeVirtualPath;
            }
        }

        public string Extension
        {
            get
            {
                return UrlPath.GetExtension(VirtualPathString);
            }
        }

        public string FileName
        {
            get
            {
                return UrlPath.GetFileName(VirtualPathStringNoTrailingSlash);
            }
        }

        public VirtualPath Combine(VirtualPath relativePath)
        {
            if (relativePath == null) throw new ArgumentNullException(nameof(relativePath));

            // If it's not relative, return it unchanged
            if (!relativePath.IsRelative)
                return relativePath;

            // The base of the combine should never be relative
            FailIfRelativePath();

            // Get either _appRelativeVirtualPath or _virtualPath
            string virtualPath = VirtualPathStringWhicheverAvailable;

            // Combine it with the relative
            virtualPath = UrlPath.Combine(virtualPath, relativePath.VirtualPathString);

            // Set the appropriate virtual path in the new object
            return new VirtualPath(virtualPath);
        }

        // This simple version of combine should only be used when the relative
        // path is known to be relative.  It's more efficient, but doesn't do any
        // sanity checks.
        internal VirtualPath SimpleCombine(string relativePath)
        {
            return SimpleCombine(relativePath, false /*addTrailingSlash*/);
        }

        internal VirtualPath SimpleCombineWithDir(string directoryName)
        {
            return SimpleCombine(directoryName, true /*addTrailingSlash*/);
        }

        private VirtualPath SimpleCombine(string filename, bool addTrailingSlash)
        {

            // The left part should always be a directory
            Debug.Assert(HasTrailingSlash);

            // The right part should not start or end with a slash
            Debug.Assert(filename[0] != '/' && !UrlPath.HasTrailingSlash(filename));

            // Use either _appRelativeVirtualPath or _virtualPath
            string virtualPath = VirtualPathStringWhicheverAvailable + filename;
            if (addTrailingSlash)
                virtualPath += "/";

            // Set the appropriate virtual path in the new object
            VirtualPath combinedVirtualPath = new VirtualPath(virtualPath);

            // Copy some flags over to avoid having to recalculate them
            combinedVirtualPath.CopyFlagsFrom(this, isWithinAppRootComputed | isWithinAppRoot | appRelativeAttempted);
#if DBG
            combinedVirtualPath.ValidateState();
#endif
            return combinedVirtualPath;
        }

        public VirtualPath MakeRelative(VirtualPath toVirtualPath)
        {
            VirtualPath resultVirtualPath = new VirtualPath();

            // Neither path can be relative
            FailIfRelativePath();
            toVirtualPath.FailIfRelativePath();

            // Set it directly since we know the slashes are already ok
            resultVirtualPath._virtualPath = UrlPath.MakeRelative(this.VirtualPathString, toVirtualPath.VirtualPathString);
            return resultVirtualPath;
        }

        internal bool HasTrailingSlash
        {
            get
            {
                if (_virtualPath != null)
                {
                    return UrlPath.HasTrailingSlash(_virtualPath);
                }
                else
                {
                    return UrlPath.HasTrailingSlash(_appRelativeVirtualPath);
                }
            }
        }


        internal void FailIfRelativePath()
        {
            if (this.IsRelative)
            {
                throw new ArgumentException($"Relative paths not permitted. Yours is: {_virtualPath}");
            }
        }

        public bool IsRelative
        {
            get
            {
                // Note that we don't need to check for "~/", since _virtualPath never contains
                // app relative paths (_appRelativeVirtualPath does)
                return _virtualPath != null && _virtualPath[0] != '/';
            }
        }

        public bool IsRoot
        {
            get
            {
                return _virtualPath == "/";
            }
        }

        public VirtualPath Parent
        {
            get
            {
                // Getting the parent doesn't make much sense on relative paths
                FailIfRelativePath();

                // "/" doesn't have a parent, so return null
                if (IsRoot)
                    return null;

                // Get either _appRelativeVirtualPath or _virtualPath
                string virtualPath = VirtualPathStringWhicheverAvailable;

                // Get rid of the ending slash, otherwise we end up with Parent("/app/sub/") == "/app/sub/"
                virtualPath = UrlPath.RemoveSlashFromPathIfNeeded(virtualPath);

                // But if it's just "~", use the absolute path instead to get the parent
                if (virtualPath == "~")
                    virtualPath = VirtualPathStringNoTrailingSlash;

                int index = virtualPath.LastIndexOf('/');
                Debug.Assert(index >= 0);

                // e.g. the parent of "/blah" is "/"
                if (index == 0)
                    return RootVirtualPath;

                // 

                // Get the parent
                virtualPath = virtualPath.Substring(0, index + 1);

                // Set the appropriate virtual path in the new object
                return new VirtualPath(virtualPath);
            }
        }

        internal static VirtualPath Combine(VirtualPath v1, VirtualPath v2)
        {
            if (v1 == null) throw new ArgumentNullException(nameof(v1));

            return v1.Combine(v2);
        }

        public static bool operator ==(VirtualPath v1, VirtualPath v2)
        {
            return VirtualPath.Equals(v1, v2);
        }

        public static bool operator !=(VirtualPath v1, VirtualPath v2)
        {
            return !VirtualPath.Equals(v1, v2);
        }

        public static bool Equals(VirtualPath v1, VirtualPath v2)
        {

            // Check if it's the same object
            if ((Object)v1 == (Object)v2)
            {
                return true;
            }

            if ((Object)v1 == null || (Object)v2 == null)
            {
                return false;
            }

            return EqualsHelper(v1, v2);
        }

        public override bool Equals(object value)
        {

            if (value == null)
                return false;

            VirtualPath virtualPath = value as VirtualPath;
            if ((object)virtualPath == null)
            {
                Debug.Assert(false);
                return false;
            }

            return EqualsHelper(virtualPath, this);
        }

        private static bool EqualsHelper(VirtualPath v1, VirtualPath v2)
        {
            return v1.VirtualPathString.Equals(v2.VirtualPathString, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return VirtualPathString.ToUpperInvariant().GetHashCode();
        }

        public override String ToString()
        {
            return VirtualPathString;
        }

        // Copy a set of flags from another VirtualPath object
        private void CopyFlagsFrom(VirtualPath virtualPath, int mask)
        {
            flags |= virtualPath.flags & mask;
        }

        internal static string GetVirtualPathString(VirtualPath virtualPath)
        {
            return virtualPath?.VirtualPathString;
        }

        internal static string GetVirtualPathStringNoTrailingSlash(VirtualPath virtualPath)
        {
            return virtualPath?.VirtualPathStringNoTrailingSlash;
        }

        // Default Create method
        public static VirtualPath Create(string virtualPath)
        {
            return Create(virtualPath, VirtualPathOptions.AllowAllPath);
        }

        public static VirtualPath CreateTrailingSlash(string virtualPath)
        {
            return Create(virtualPath, VirtualPathOptions.AllowAllPath | VirtualPathOptions.EnsureTrailingSlash);
        }

        public static VirtualPath CreateAllowNull(string virtualPath)
        {
            return Create(virtualPath, VirtualPathOptions.AllowAllPath | VirtualPathOptions.AllowNull);
        }

        public static VirtualPath CreateAbsolute(string virtualPath)
        {
            return Create(virtualPath, VirtualPathOptions.AllowAbsolutePath);
        }

        public static VirtualPath CreateNonRelative(string virtualPath)
        {
            return Create(virtualPath, VirtualPathOptions.AllowAbsolutePath);
        }

        public static VirtualPath CreateAbsoluteTrailingSlash(string virtualPath)
        {
            return Create(virtualPath, VirtualPathOptions.AllowAbsolutePath | VirtualPathOptions.EnsureTrailingSlash);
        }

        public static VirtualPath CreateNonRelativeTrailingSlash(string virtualPath)
        {
            return Create(virtualPath, VirtualPathOptions.AllowAbsolutePath |
                VirtualPathOptions.EnsureTrailingSlash);
        }

        public static VirtualPath CreateAbsoluteAllowNull(string virtualPath)
        {
            return Create(virtualPath, VirtualPathOptions.AllowAbsolutePath | VirtualPathOptions.AllowNull);
        }

        public static VirtualPath CreateNonRelativeAllowNull(string virtualPath)
        {
            return Create(virtualPath, VirtualPathOptions.AllowAbsolutePath | VirtualPathOptions.AllowNull);
        }

        public static VirtualPath CreateNonRelativeTrailingSlashAllowNull(string virtualPath)
        {
            return Create(virtualPath, VirtualPathOptions.AllowAbsolutePath |
                VirtualPathOptions.AllowNull | VirtualPathOptions.EnsureTrailingSlash);
        }

        public static VirtualPath Create(string virtualPath, VirtualPathOptions options)
        {

            // Trim it first, so that blank strings (e.g. "  ") get treated as empty
            if (virtualPath != null)
                virtualPath = virtualPath.Trim();

            // If it's empty, check whether we allow it
            if (String.IsNullOrEmpty(virtualPath))
            {
                if ((options & VirtualPathOptions.AllowNull) != 0)
                    return null;

                throw new ArgumentNullException(nameof(virtualPath));
            }

            // Dev10 767308: optimize for normal paths, and scan once for
            //     i) invalid chars
            //    ii) slashes
            //   iii) '.'

            bool slashes = false;
            bool dot = false;
            int len = virtualPath.Length;
            for (int i = 0; i < len; i++)
            {
                switch (virtualPath[i])
                {
                    // need to fix slashes ?
                    case '/':
                        if (i > 0 && virtualPath[i - 1] == '/')
                            slashes = true;
                        break;
                    case '\\':
                        slashes = true;
                        break;
                    // contains "." or ".."
                    case '.':
                        dot = true;
                        break;
                    // invalid chars
                    case '\0':
                        throw new ArgumentException($"Invalid virtual path: '{virtualPath}'", nameof(virtualPath));
                    default:
                        break;
                }
            }
            if (slashes)
            {
                // If we're supposed to fail on malformed path, then throw
                if ((options & VirtualPathOptions.FailIfMalformed) != 0)
                {
                    throw new ArgumentException($"Invalid virtual path: '{virtualPath}'", nameof(virtualPath));
                }
                // Flip ----lashes, and remove duplicate slashes                
                virtualPath = UrlPath.FixVirtualPathSlashes(virtualPath);
            }

            // Make sure it ends with a trailing slash if requested
            if ((options & VirtualPathOptions.EnsureTrailingSlash) != 0)
                virtualPath = UrlPath.AppendSlashToPathIfNeeded(virtualPath);

            VirtualPath virtualPathObject = new VirtualPath();


            if (virtualPath[0] != '/')
            {
                if ((options & VirtualPathOptions.AllowRelativePath) == 0)
                {
                    var message =
                        $"Relative paths are not permitted for this operation. Your is: '{virtualPath}'";

                    throw new ArgumentException(message, nameof(virtualPath));
                }

                // Don't Reduce relative paths, since the Reduce method is broken (e.g. "../foo.aspx" --> "/foo.aspx!")
                // 
                virtualPathObject._virtualPath = virtualPath;
            }
            else
            {
                if ((options & VirtualPathOptions.AllowAbsolutePath) == 0)
                {
                    var message = $"Absolute paths not permitted. Yours is: {virtualPath}";
                    throw new ArgumentException(message, nameof(virtualPath));
                }

                if (dot)
                    virtualPath = UrlPath.ReduceVirtualPath(virtualPath);

                virtualPathObject._virtualPath = virtualPath;
            }

            return virtualPathObject;
        }

        [Flags]
        internal enum VirtualPathOptions
        {
            AllowNull = 1,
            EnsureTrailingSlash = 2,
            AllowAbsolutePath = 4,
            //AllowAppRelativePath = 8,
            AllowRelativePath = 16,
            FailIfMalformed = 32,
            AllowAllPath = AllowRelativePath | AllowAbsolutePath,
        }
    }
}
