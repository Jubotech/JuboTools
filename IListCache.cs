using System;
using System.Collections.Generic;

namespace Jubotech.Caching
{
    public interface IListCache
    {
        /// <summary>
        /// Gets a List of TDto from the cache or creates then returns it if it doesn't exist. The TDto objects are created out of a cached list of TEntity objects and are mapped automatically.
        /// Note: If the 'predicate' parameter is defined when calling GetListOf(...), the parameter 'additionalKey' must also be defined
        /// </summary>
        /// <typeparam name="TEntity">Type of the entity</typeparam>
        /// <typeparam name="TDto">Type of the DTO</typeparam>
        /// <param name="predicate"></param>
        /// <param name="additionalKey">Additional Key if you need to cache different lists of the same TDto type (since the default key is the TDto type name)</param>
        /// <returns>List of TDto objects</returns>
        List<TDto> GetListOf<TEntity, TDto>(Func<TEntity, bool> predicate = null, string additionalKey = "") where TEntity : class where TDto : class, new();

		/// <summary>
        /// Creates a NEW list of TDto objects out of a cached list of TEntity objects
        /// Note: If the 'predicate' parameter is defined when calling GetListOf(...), the parameter 'additionalKey' must also be defined
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="additionalKey">Additional Key if you need to use a specific entry in the TEntity objects cache</param>
        /// <returns>New List TDto objects</returns>
        List<TDto> GetNewListOf<TEntity, TDto>(Func<TEntity, bool> predicate = null, string additionalKey = "")
            where TEntity : class
            where TDto : class, new();

        
        /// <summary>
        /// Gets a List of TEntity from the cache or creates then returns it if it doesn't exist.
        /// Note: If the 'predicate' parameter is defined when calling GetListOf(...), the parameter 'additionalKey' must also be defined
        /// </summary>
        /// <typeparam name="TEntity">Type of the entity</typeparam>
        /// <param name="predicate"></param>
        /// <param name="additionalKey">Additional Key if you need to cache different lists of the same TEntity type (since the default key is the TEntity type name)</param>
        /// <returns>List of TEntity objects</returns>
        List<TEntity> GetListOf<TEntity>(Func<TEntity, bool> predicate = null, string additionalKey = "")
            where TEntity : class;

        /// <summary>
        /// Clears the cache
        /// </summary>
        void ClearListCache();
    }
}