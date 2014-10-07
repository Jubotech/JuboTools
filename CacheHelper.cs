using System;
using Jubo.GlobalHorseDirectory.Common;

namespace Jubotech.Caching
{
    /// <summary>
    /// Provides utility methods for Caching
    /// </summary>
    public class CacheHelper : ICacheHelper
    {
        /// <summary>
        /// Generates a key as [The T type name] + [IF ANY: an additional key provided by the caller]
        /// -> The additional key enables caching of multiple lists of the same Types
        /// </summary>
        /// <typeparam name="T">Type for which the name will be used to generate the key</typeparam>
        /// <param name="additionalKey">additional key to append to the Type name</param>
        /// <returns>Key generated from Type name + additional key if any</returns>
        public string GenerateKey<T>(string additionalKey) where T : class
        {
            var extraKey = GetExtraKey(additionalKey);
            var key = typeof(T).Name + extraKey;

            return key;
        }
        public string GetExtraKey(string additionalKey)
        {

            var extraKey = !string.IsNullOrEmpty(additionalKey) ? string.Format("_{0}", additionalKey) : string.Empty;

            return extraKey;
        }
        public void ValidatePredicate<TEntity>(Func<TEntity, bool> predicate, string additionalKey) where TEntity : class
        {
            if (predicate != null && string.IsNullOrEmpty(additionalKey))
            {
                throw new CacheException(
                    "If the 'predicate' parameter is defined when calling GetListOf<>(...), the parameter 'additionalKey' must also be defined!");
            }
        }
        public Func<TEntity, bool> SetPredicateIfNeeded<TEntity>(Func<TEntity, bool> predicate) where TEntity : class
        {
            return predicate ?? (e => true);
        }
    }
}