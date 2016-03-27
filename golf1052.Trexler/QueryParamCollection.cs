using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace golf1052.Trexler
{
    public class QueryParamCollection : IDictionary<string, object>
    {
        private readonly Dictionary<string, object> dictionary = new Dictionary<string, object>();
        private readonly List<string> orderedKeys = new List<string>();

        public static QueryParamCollection Parse(string queryString)
        {
            QueryParamCollection result = new QueryParamCollection();
            if (string.IsNullOrEmpty(queryString))
            {
                return result;
            }

            queryString = queryString.TrimStart('?').Split('?').Last();

            var pairs = (
                from keyValue in queryString.Split('&')
                let pair = keyValue.Split('=')
                let key = pair[0]
                let value = pair.Length >= 2 ? TrexUri.DecodeQueryParamValue(pair[1]) : ""
                group value by key into g
                select new { Key = g.Key, Values = g.ToArray() });

            foreach (var g in pairs)
            {
                if (g.Values.Length == 1)
                {
                    result.Add(g.Key, g.Values[0]);
                }
                else
                {
                    result.Add(g.Key, g.Values);
                }
            }

            return result;
        }

        public string ToString(bool encodeSpaceAsPlus)
        {
            return string.Join("&", GetPairs(encodeSpaceAsPlus));
        }

        public override string ToString()
        {
            return ToString(false);
        }

        private IEnumerable<string> GetPairs(bool encodeSpaceAsPlus)
        {
            foreach (string key in orderedKeys)
            {
                object val = this[key];
                if (val == null)
                {
                    continue;
                }

                if (val is string || !(val is IEnumerable))
                {
                    yield return key + "=" + TrexUri.EncodeQueryParamValue(val, encodeSpaceAsPlus);
                }
                else
                {
                    foreach (var subval in val as IEnumerable)
                    {
                        if (subval == null)
                        {
                            continue;
                        }
                        yield return key + "=" + TrexUri.EncodeQueryParamValue(subval, encodeSpaceAsPlus);
                    }
                }
            }
        }

        public object this[string key]
        {
            get
            {
                return dictionary[key];
            }
            set
            {
                dictionary[key] = value;
                if (!orderedKeys.Contains(key))
                {
                    orderedKeys.Add(key);
                }
            }
        }

        public int Count { get { return dictionary.Count; } }

        public bool IsReadOnly { get { return false; } }

        public ICollection<string> Keys { get { return orderedKeys; } }

        public ICollection<object> Values
        {
            get
            {
                return orderedKeys.Select(key => dictionary[key]).ToArray();
            }
        }

        public void Add(KeyValuePair<string, object> item)
        {
            Add(item.Key, item.Value);
        }

        public void Add(string key, object value)
        {
            dictionary.Add(key, value);
            orderedKeys.Add(key);
        }

        public void Clear()
        {
            dictionary.Clear();
            orderedKeys.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return dictionary.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return dictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            ((ICollection)dictionary).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return orderedKeys.Select(key => new KeyValuePair<string, object>(key, dictionary[key])).GetEnumerator();
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return Remove(item.Key);
        }

        public bool Remove(string key)
        {
            orderedKeys.Remove(key);
            return dictionary.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
