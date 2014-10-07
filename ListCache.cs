using System;
using System.Collections.Generic;
using System.Linq;
using Jubotech.Common; // Contains the entities and DTO objects
using Jubotech.DAL;    // Contains the Data Access Layer (Generic Repository + UnitOfWork) 
using Omu.ValueInjecter;

namespace Jubotech.Caching
{
    /// <summary>
    /// Each instance of this class maintains a Dictionary variable which holds collections of objects (EF entities, or DTO objects)
    /// Once a collection is retrieved using one of the GetListOf method, this collection is cached in the Dictionary variable
    /// then the next identical call to the GetListOf method will return the cached version of the collection
    /// </summary>
    public class ListCache : IListCache
    {
        private readonly object _lock;
        private readonly IUnitOfWork  _unitOfWork;
        private readonly ICacheHelper _cacheHelper;
        private Dictionary<string, IEnumerable<object>> _cachedLists;

        public ListCache()
        {
            _lock        = new object();
            _cachedLists = new Dictionary<string, IEnumerable<object>>();
            _unitOfWork  = IoC.Resolve<IUnitOfWork>();
            _cacheHelper = IoC.Resolve<ICacheHelper>();
        }

        /// <summary>
        /// Gets a List of TEntity from the cache or creates, then returns it if it doesn't exist.
        /// Note: If the 'predicate' parameter is defined when calling GetListOf(...), the parameter 'additionalKey' must also be defined
        /// </summary>
        /// <typeparam name="TEntity">Type of the entity</typeparam>
        /// <param name="predicate"></param>
        /// <param name="additionalKey">Additional Key if you need to cache different lists of the same TEntity type (since the default key is the TEntity type name)</param>
        /// <returns>List of TEntity objects</returns>
        public List<TEntity> GetListOf<TEntity>(Func<TEntity, bool> predicate = null, string additionalKey = "") where TEntity : class
        {
            var key = _cacheHelper.GenerateKey<TEntity>(additionalKey);
            _cacheHelper.ValidatePredicate(predicate, additionalKey);
            predicate = _cacheHelper.SetPredicateIfNeeded(predicate);
            
            if (!_cachedLists.ContainsKey(key))
            {
                try
                {
                    var listEntities = _unitOfWork.Context.Set<TEntity>().Where(predicate).ToList();

                    lock (_lock)
                    {
                        _cachedLists[key] = listEntities;
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteErrorLog(ex);
                }
            }

            return _cachedLists[key] as List<TEntity>;
        }
		
        /// <summary>
        /// Gets a List of TDto from the cache or creates then returns it if it doesn't exist. The TDto objects are created out of a cached list of TEntity objects and are mapped automatically.
        /// Note: If the 'predicate' parameter is defined when calling GetListOf(...), the parameter 'additionalKey' must also be defined
        /// </summary>
        /// <typeparam name="TEntity">Type of the entity</typeparam>
        /// <typeparam name="TDto">Type of the DTO</typeparam>
        /// <param name="predicate"></param>
        /// <param name="additionalKey">Additional Key if you need to cache different lists of the same TDto type (since the default key is the TDto type name)</param>
        /// <returns>List of TDto objects</returns>
        public List<TDto> GetListOf<TEntity, TDto>(Func<TEntity, bool> predicate = null, string additionalKey = "") where TEntity : class where TDto : class, new()
        {
            var key = _cacheHelper.GenerateKey<TDto>(additionalKey);
            _cacheHelper.ValidatePredicate(predicate, additionalKey);
            predicate = _cacheHelper.SetPredicateIfNeeded(predicate);
            
            if (!_cachedLists.ContainsKey(key))
            {
                lock (_lock)
                {
                    _cachedLists[key] = GetListOfDtoFromEntity<TEntity, TDto>(predicate);
                }
            }

            return _cachedLists[key] as List<TDto>;
        }
		
        /// <summary>
        /// Creates a NEW list of TDto objects out of a cached list of TEntity objects
        /// Note: If the 'predicate' parameter is defined when calling GetListOf(...), the parameter 'additionalKey' must also be defined
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="additionalKey">Additional Key if you need to use a specific entry in the TEntity objects cache</param>
        /// <returns>New List TDto objects</returns>
        public List<TDto> GetNewListOf<TEntity, TDto>(Func<TEntity, bool> predicate = null, string additionalKey = "") where TEntity : class where TDto : class, new()
        {
            var dtos = new List<TDto>();

            var cachedEntities = GetListOf<TEntity>(predicate, additionalKey);
            cachedEntities.ToList().ForEach(e =>
            {
                var dto = new TDto();
                dto.InjectFrom(e);
                dtos.Add(dto);
            });

            return dtos;
        }
		
        /// <summary>
        /// Clears the cache
        /// </summary>
        public void ClearListCache()
        {
            _cachedLists = null;
            LogHelper.WriteInfoLog("ListCache._cachedLists cleared!");
        }

        private IEnumerable<TDto> GetListOfDtoFromEntity<TEntity, TDto>(Func<TEntity, bool> predicate)
            where TEntity : class
            where TDto : class, new()
        {
            var listEntities = _unitOfWork.Context.Set<TEntity>().Where(predicate).ToList();
            var dtos = new List<TDto>();

            listEntities.ToList().ForEach(entity =>
            {
                var dto = new TDto();
                dto.InjectFrom(entity);
                dtos.Add(dto);
            });

            return dtos;
        }
    }
}