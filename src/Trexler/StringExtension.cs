using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trexler
{
    public static class StringExtension
    {
        public static bool IsUri(this string s)
        {
            return s != null && Uri.IsWellFormedUriString(s, UriKind.Absolute);
        }

        public static TrexUri AppendPathSegment(this string uri, string segment)
        {
            return new TrexUri(uri).AppendPathSegment(segment);
        }

        public static TrexUri AppendPathSegments(this string uri, params string[] segments)
        {
            return new TrexUri(uri).AppendPathSegments(segments);
        }

        public static TrexUri AppendPathSegments(this string uri, IEnumerable<string> segments)
        {
            return new TrexUri(uri).AppendPathSegments(segments);
        }

        public static TrexUri SetQueryParam(this string uri, string name, object value)
        {
            return new TrexUri(uri).SetQueryParam(name, value);
        }

        public static TrexUri SetQueryParams(this string uri, IEnumerable<KeyValuePair<string, object>> values)
        {
            return new TrexUri(uri).SetQueryParams(values);
        }

        public static TrexUri RemoveQueryParam(this string uri, string name)
        {
            return new TrexUri(uri).RemoveQueryParam(name);
        }

        public static TrexUri RemoveQueryParams(this string uri, params string[] names)
        {
            return new TrexUri(uri).RemoveQueryParams(names);
        }

        public static TrexUri RemoveQueryParams(this string uri, IEnumerable<string> names)
        {
            return new TrexUri(uri).RemoveQueryParams(names);
        }
    }
}
