﻿using System;
using System.Reflection;

namespace WDE.Common.Types
{
    public readonly struct ImageUri
    {
        public string? Uri { get; }
        
        public ImageUri(string relativePath)
        {
            Uri = relativePath;
        }

        public static ImageUri Empty { get; } = new ImageUri("Icons/empty.png");

        public bool Equals(ImageUri other)
        {
            return Uri == other.Uri;
        }

        public override bool Equals(object? obj)
        {
            return obj is ImageUri other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Uri?.GetHashCode() ?? 0;
        }
    }
}