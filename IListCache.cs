using System;
using System.Collections.Generic;

namespace Jubotech.Caching
{
	public interface IListCache
	{
		List<TDto> GetListOf<TEntity, TDto>(Func<TEntity, bool> predicate = null, string additionalKey = "") 
			where TEntity : class 
			where TDto : class, new();

		List<TDto> GetNewListOf<TEntity, TDto>(Func<TEntity, bool> predicate = null, string additionalKey = "")
			where TEntity : class
			where TDto : class, new();
        
		List<TEntity> GetListOf<TEntity>(Func<TEntity, bool> predicate = null, string additionalKey = "")
			where TEntity : class;

		void ClearListCache();
	}
}