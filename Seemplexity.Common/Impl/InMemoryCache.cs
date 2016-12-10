using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Seemplexity.Common.Abstract;

namespace Seemplexity.Common.Impl
{
    public class InMemoryCache : ICache
    {
        public static TimeSpan CacheTimeout { get; set; } = TimeSpan.FromMinutes(30);

        /// <summary>
        /// Check for item in cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            return MemoryCache.Default[key] != null;
        }

        public bool Get<T>(string key, out T value)
        {
            try
            {
                if (!Exists(key))
                {
                    value = default(T);
                    return false;
                }

                object val = MemoryCache.Default[key];
                if (val.Equals(DBNull.Value))
                {
                    value = default(T);
                    return true;
                }
                value = (T)MemoryCache.Default[key];
            }
            catch
            {
                value = default(T);
                return false;
            }

            return true;
        }

        private static string GetCacheKey(IEnumerable elems)
        {
            var stringBuilder = new StringBuilder();

            foreach (var elem in elems)
            {
                if (elem is IEnumerable && !(elem is string))
                    stringBuilder.AppendFormat("{0}_", GetCacheKey((IEnumerable)elem));
                else
                    stringBuilder.AppendFormat("{0}_", elem.ToString());
            }

            var len = stringBuilder.Length;
            return len > 0 ? stringBuilder.ToString(0, len - 1) : String.Empty;
        }

        public T Get<T>(Func<T> func, [CallerMemberName]string callerMemberName = "")
        {
            T result;
            if (!Get(callerMemberName, out result))
            {
                result = func();
                Add(callerMemberName, result);
            }

            return result;
        }

        public T2 Get<T1, T2>(Func<T1, T2> func, T1 param1, [CallerMemberName]string callerMemberName = "")
        {
            T2 result;
            var cacheKey = GetCacheKey(new[] { callerMemberName, param1.ToString() });

            if (!Get(cacheKey, out result))
            {
                result = func(param1);
                Add(cacheKey, result);
            }

            return result;
        }

        public T3 Get<T1, T2, T3>(Func<T1, T2, T3> func, T1 param1, T2 param2, [CallerMemberName] string callerMemberName = "")
        {
            T3 result;
            var cacheKey = GetCacheKey(new[] { callerMemberName, param1.ToString(), param2.ToString() });

            if (!Get(cacheKey, out result))
            {
                result = func(param1, param2);
                Add(cacheKey, result);
            }

            return result;
        }

        public T4 Get<T1, T2, T3, T4>(Func<T1, T2, T3, T4> func, T1 param1, T2 param2, T3 param3, [CallerMemberName] string callerMemberName = "")
        {
            T4 result;
            var cacheKey = GetCacheKey(new[] { callerMemberName, param1.ToString(), param2.ToString(), param3.ToString() });

            if (!Get(cacheKey, out result))
            {
                result = func(param1, param2, param3);
                Add(cacheKey, result);
            }

            return result;
        }

        public T5 Get<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5> func, T1 param1, T2 param2, T3 param3, T4 param4, [CallerMemberName] string callerMemberName = "")
        {
            T5 result;
            var cacheKey = GetCacheKey(new[] { callerMemberName, param1.ToString(), param2.ToString(), param3.ToString(), param4.ToString() });

            if (!Get(cacheKey, out result))
            {
                result = func(param1, param2, param3, param4);
                Add(cacheKey, result);
            }

            return result;
        }

        public void Add<T>(string key, T value)
        {
            Add(key, value, CacheTimeout);
        }

        public void Add<T>(string key, T value, TimeSpan cacheTimeout)
        {
            object val;
            if (value == null)
                val = DBNull.Value;
            else
                val = value;

            MemoryCache.Default.Add(key, val, DateTime.Now.Add(cacheTimeout));
        }
    }
}
