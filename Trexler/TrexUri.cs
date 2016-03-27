using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace golf1052.Trexler
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
            return new TrexUri(uri).AppendPathSegments(segments).ToString();
        }
        
        public static string GetRoot(string url)
        {
            Uri uri = new Uri(url);
            return uri.GetComponents(UriComponents.SchemeAndServer | UriComponents.UserInfo, UriFormat.Unescaped);
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

        private static string CleanSegment(string segment)
        {
            string unescaped = Uri.UnescapeDataString(segment);
            return Uri.EscapeUriString(unescaped).Replace("?", "%3F");
        }

        public TrexUri AppendPathSegment(string segment)
        {
            if (segment == null)
            {
                return this;
            }
            
            if (!Path.EndsWith("/"))
            {
                Path += "/";
            }
            Path += CleanSegment(segment.TrimStart('/'));
            return this;
        }

        public TrexUri AppendPathSegments(params string[] segments)
        {
            foreach (string segment in segments)
            {
                AppendPathSegment(segment);
            }
            return this;
        }

        public TrexUri AppendPathSegments(IEnumerable<string> segments)
        {
            foreach (string segment in segments)
            {
                AppendPathSegment(segment);
            }
            return this;
        }

        public TrexUri SetQueryParam(string name, object value)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name", "Query parameter name cannot be null.");
            }

            if (value != null && value.GetType() == typeof(string))
            {
                // if we get an empty string don't append it
                if (string.IsNullOrEmpty((string)value))
                {
                    return this;
                }
            }
            QueryParams[name] = value;
            return this;
        }

        public TrexUri SetQueryParams(IEnumerable<KeyValuePair<string, object>> values)
        {
            if (values == null)
            {
                return this;
            }

            foreach (var pair in values)
            {
                SetQueryParam(pair.Key, pair.Value);
            }
            return this;
        }
        
        public TrexUri RemoveQueryParam(string name)
        {
            QueryParams.Remove(name);
            return this;
        }

        public TrexUri RemoveQueryParams(params string[] names)
        {
            foreach (string name in names)
            {
                QueryParams.Remove(name);
            }
            return this;
        }
        
        public TrexUri RemoveQueryParams(IEnumerable<string> names)
        {
            foreach (string name in names)
            {
                QueryParams.Remove(name);
            }
            return this;
        }

        public TrexUri ResetToRoot()
        {
            Path = GetRoot(Path);
            QueryParams.Clear();
            return this;
        }

        public string ToString(bool encodeSpaceAsPlus)
        {
            string url = Path;
            string query = QueryParams.ToString(encodeSpaceAsPlus);
            if (query.Length > 0)
            {
                url += "?" + query;
            }
            return url;
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public static implicit operator string(TrexUri uri)
        {
            return uri.ToString();
        }

        public static implicit operator TrexUri(string uri)
        {
            return new TrexUri(uri);
        }
    }
}
