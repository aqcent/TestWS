using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestWS.Managers;

namespace TestWS.Extensions
{
    public static class CacheManagerExtensions
    {
        public static T CacheResult<T>(this ICacheManager cacheManager, Func<T> dataGetter, string cacheKey, TimeSpan? expirationSpan = null)
            where T : class
        {
            var resultModel = cacheManager.Get<T>(cacheKey);
            if (resultModel == null)
            {
                resultModel = dataGetter();
                if (resultModel != null)
                {
                    if (expirationSpan.HasValue)
                        cacheManager.Insert(resultModel, cacheKey, expirationSpan.Value);
                    else
                        cacheManager.Insert(resultModel, cacheKey);
                }
            }
            return resultModel;
        }
        public static IEnumerable<T> CacheResult<T>(this ICacheManager cacheManager, Func<IEnumerable<T>> dataGetter, string cacheKey, TimeSpan? expirationSpan = null)
           where T : class
        {
            var resultModel = cacheManager.Get<IEnumerable<T>>(cacheKey);
            if (resultModel.Any())
            {
                resultModel = dataGetter();
                if (resultModel != null)
                {
                    if (expirationSpan.HasValue)
                        cacheManager.Insert(resultModel, cacheKey, expirationSpan.Value);
                    else
                        cacheManager.Insert(resultModel, cacheKey);
                }
            }
            return resultModel;
        }
        public static T CacheResult<T>(this ICacheManager cacheManager, Func<T> dataGetter, string cacheKey, DateTime expirationTime)
            where T : class
        {
            var resultModel = cacheManager.Get<T>(cacheKey);
            if (resultModel == null)
            {
                resultModel = dataGetter();
                if (resultModel != null)
                {
                    cacheManager.Insert(resultModel, cacheKey, expirationTime);
                }
            }
            return resultModel;
        }
        public static IEnumerable<T> CacheResult<T>(this ICacheManager cacheManager, Func<IEnumerable<T>> dataGetter, string cacheKey, DateTime expirationTime)
           where T : class
        {
            var resultModel = cacheManager.Get<IEnumerable<T>>(cacheKey);
            if (resultModel.Any())
            {
                resultModel = dataGetter();
                if (resultModel != null)
                {
                    cacheManager.Insert(resultModel, cacheKey, expirationTime);
                }
            }
            return resultModel;
        }
    }
}