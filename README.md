Caching Utility
===============

ListCache is the main class to use.

Each instance of this class maintains a Dictionary variable which holds collections of objects (EF entities, or DTO objects).
Once a collection is retrieved using one of the GetListOf method, this collection is cached in the Dictionary variable then the next identical call to the GetListOf method will return the cached version of the collection.

The DTO objects are retrieved from EF entities, then mapped automatically before being cached.