using System;

namespace Jubotech.Caching
{
    public interface ICacheHelper
    {
		string GenerateKey<T>(string additionalKey) where T : class;
        
		string GetExtraKey(string additionalKey);
        
		void ValidatePredicate<TEntity>(Func<TEntity, bool> predicate, string additionalKey) where TEntity : class;
        
		Func<TEntity, bool> SetPredicateIfNeeded<TEntity>(Func<TEntity, bool> predicate) where TEntity : class;
    }
}