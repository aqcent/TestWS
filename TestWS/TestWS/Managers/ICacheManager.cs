using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWS.Managers
{
    public interface ICacheManager
    {
        T Get<T>(string key) where T : class;
        void Insert<T>(T item, string key);
        void Insert<T>(T item, string key, DateTime expirationTime);
        void Insert<T>(T item, string key, TimeSpan expirationSpan);
        void Clear();
    }
}
