// ReSharper disable All
//------------------------------------------------------------------------------
// <copyright file="VirtualPathUtility.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

/*
 * VirtualPathUtility class
 *
 * Copyright (c) 2004 Microsoft Corporation
 */

using Jali.Core.Decompiled.System.Web.Util;

namespace Jali.Core.Decompiled.System.Web
{

    /*
     * Code to perform virtual path operations
     */
    internal static class VirtualPathUtility
    {

        /* Discover virtual path type */

        public static bool IsAbsolute(string virtualPath)
        {
            VirtualPath virtualPathObject = VirtualPath.Create(virtualPath);
            return !virtualPathObject.IsRelative && virtualPathObject.VirtualPathStringIfAvailable != null;
        }

        public static bool IsAppRelative(string virtualPath)
        {
            VirtualPath virtualPathObject = VirtualPath.Create(virtualPath);
            return virtualPathObject.VirtualPathStringIfAvailable == null;
        }

        public static string ToAbsolute(string virtualPath)
        {
            VirtualPath virtualPathObject = VirtualPath.CreateNonRelative(virtualPath);
            return virtualPathObject.VirtualPathString;
        }


        /* Get pieces of virtual path */
        public static string GetFileName(string virtualPath)
        {
            VirtualPath virtualPathObject = VirtualPath.CreateNonRelative(virtualPath);
            return virtualPathObject.FileName;
        }

        public static string GetDirectory(string virtualPath)
        {
            VirtualPath virtualPathObject = VirtualPath.CreateNonRelative(virtualPath);

            virtualPathObject = virtualPathObject.Parent;

            return virtualPathObject?.VirtualPathStringWhicheverAvailable;
        }

        public static string GetExtension(string virtualPath)
        {
            VirtualPath virtualPathObject = VirtualPath.Create(virtualPath);
            return virtualPathObject.Extension;
        }

        /* Canonicalize virtual paths */
        public static string AppendTrailingSlash(string virtualPath)
        {
            return UrlPath.AppendSlashToPathIfNeeded(virtualPath);
        }

        public static string RemoveTrailingSlash(string virtualPath)
        {
            return UrlPath.RemoveSlashFromPathIfNeeded(virtualPath);
        }

        /* Work with multiple virtual paths */
        public static string Combine(string basePath, string relativePath)
        {
            VirtualPath virtualPath = VirtualPath.Combine(VirtualPath.CreateNonRelative(basePath),
                VirtualPath.Create(relativePath));
            return virtualPath.VirtualPathStringWhicheverAvailable;
        }
    }


}
