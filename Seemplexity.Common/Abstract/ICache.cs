using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Seemplexity.Common.Abstract
{
    public interface ICache
    {
        bool Exists(string key);
        bool Get<T>(string key, out T value);
        void Add<T>(string key, T value);
        void Add<T>(string key, T value, TimeSpan timeout);

        T Get<T>(Func<T> func, [CallerMemberName]string callerMemberName = "");
        T2 Get<T1, T2>(Func<T1, T2> func, T1 param1, [CallerMemberName]string callerMemberName = "");
        T3 Get<T1, T2, T3>(Func<T1, T2, T3> func, T1 param1, T2 param2, [CallerMemberName]string callerMemberName = "");
        T4 Get<T1, T2, T3, T4>(Func<T1, T2, T3, T4> func, T1 param1, T2 param2, T3 param3, [CallerMemberName]string callerMemberName = "");
        T5 Get<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5> func, T1 param1, T2 param2, T3 param3, T4 param4, [CallerMemberName]string callerMemberName = "");

        //void Delete(String key);
        //void Delete<T>(Expression<T> expr);
    }
}
