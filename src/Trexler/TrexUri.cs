using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trexler
{
    public class TrexUri
    {
        public string Path { get; private set; }

        public QueryParamCollection QueryParams { get; private set; }

        public TrexUri(string baseUri)
        {
            if (baseUri == null)
            {
                throw new ArgumentNullException("baseUri cannot be null");
            }

            string[] parts = baseUri.Split('?');
            Path = parts[0];
            QueryParams = QueryParamCollection.Parse(parts.Length > 1 ? parts[1] : "");
        }

        public static string Combine(string uri, params string[] segments)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri cannot be null");
            }

            return new TrexUri(uri)
        }

        public static string DecodeQueryParamValue(string value)
        {
            return Uri.UnescapeDataString((value ?? "").Replace("+", " "));
        }

        public static string EncodeQueryParamValue(object value, bool encodeSpaceAsPlus)
        {
            string result = Uri.EscapeDataString((value ?? "").ToInvariantString());
            return encodeSpaceAsPlus ? result.Replace("%20", "+") : result;
        }

        public TrexUri AppendPathSegment(string segment)
        {
            if (segment == null)
            {
                throw new ArgumentNullException("segment cannot be null");
            }
            
            if (!Path.EndsWith("/"))
            {
                Path += "/";
            }
        }
    }
}
