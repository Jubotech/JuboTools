ListCache - Fetch, cache, and reuse with 1 line of code!
========================================================

With ListCache you can query your database using Linq-to-Entity, retrieve and cache your entities all in one line of code.
 
But, manipulating entities directly is not always recommended. ListCache can also help. You can query your database using Linq-to-Entity, retrieve your entities, automatically map them to a collection of DTO objects (thanks Omu.ValueInjecter!), cache then return this collection of DTO objects, again all in one line of code!

Subsequent identical calls to the ListCache methods for the same entities or DTO objects simply return the data from the cache. The key of the cached data is the Type name of the entity or DTO object retrieved. If a Linq query predicate is used to filter the data, then the caller needs to specify a key explicitely.


Retrieving and caching a collection of Entity Framework entities
================================================================
```
IListCache listCache  = new ListCache();

// Retrieves all the countries from the database, caches then returns the collection.
List<country> countryEntities1 = listCache.GetListOf<country>(); 

// This time, the countries are retrieved from the cache.
List<country> countryEntities2 = listCache.GetListOf<country>(); 
```

Retrieving and caching a collection of DTO objects created out of your Entity Framework entities
================================================================================================
```
IListCache listCache = new ListCache();

// Retrieves a country entity list, then generates, caches 
// and return a List of CountryDto objects out of it. 
// Also the country entity list gets cached in the process.
List<CountryDto> countryDtos1 = listCache.GetListOf<country, CountryDto>(); 

// This time, the CountryDto collection is retrieved from the cache.
List<CountryDto> countryDtos2 = listCache.GetListOf<country, CountryDto>();

// Retrieves the cached country entities.
List<country> countryEntities = listCache.GetListOf<country>(); 
```

Retrieving and caching a filtered collection of DTO objects created out of your Entity Framework entities
=========================================================================================================
```
IListCache listCache = new ListCache();

// Retrieves a filtered country entity list (countries with a name starting with "U"), 
// then generates, caches and return a List of CountryDto objects out of it. 
// Also the filtered country entity list gets cached in the process.
List<CountryDto> countryDtos1 = 
					listCache.GetListOf<country, CountryDto>(c => c.Name.StartsWith("U"), 
															 "CountriesStartingWithU"); 

// In both statements below, the CountryDto filtered collection is retrieved from the cache.
List<CountryDto> countryDtos2 = 
					listCache.GetListOf<country, CountryDto>(c => c.Name.StartsWith("U"), 
															 "CountriesStartingWithU"); 
List<CountryDto> countryDtos3 = 
					listCache.GetListOf<country, CountryDto>("CountriesStartingWithU");
```

Clears the cache
=========================================================================================================
```
IListCache listCache = new ListCache();
// ...
listCache.ClearListCache();
```
