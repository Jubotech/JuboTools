using System;

namespace Jubotech.Caching
{
    public interface ICacheHelper
    {
        /// <summary>
        /// Generates a key as [The T type name] + [IF ANY: an additional key provided by the caller]
        /// -> The additional key enables caching of multiple lists of the same Types
        /// </summary>
        /// <typeparam name="T">Type for which the name will be used to generate the key</typeparam>
        /// <param name="additionalKey">additional key to append to the Type name</param>
        /// <returns>Key generated from Type name + additional key if any</returns>
        string GenerateKey<T>(string additionalKey) where T : class;
        string GetExtraKey(string additionalKey);
        void ValidatePredicate<TEntity>(Func<TEntity, bool> predicate, string additionalKey) where TEntity : class;
        Func<TEntity, bool> SetPredicateIfNeeded<TEntity>(Func<TEntity, bool> predicate) where TEntity : class;
    }
}