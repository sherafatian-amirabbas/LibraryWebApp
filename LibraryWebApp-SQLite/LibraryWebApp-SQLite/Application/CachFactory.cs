using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryWebApp_SQLite.Application
{
    public interface ICacheable<T>
    {
        /// <summary>
        /// To cache an object
        /// </summary>
        /// <param name = "obj" > The object to be cached</param>
        /// <param name="key">
        /// a key need to be provided for restore the object later. If it is not provided, it will be generated inside 
        /// the method.
        /// </param>
        /// <returns>A key is generated for further use (to restore the cached object)</returns>
        object Cache(T obj, object key = default(object));

        /// <summary>
        /// To restore the cached object using the already generated key
        /// </summary>
        /// <param name="key">pre-generated key (when caching the object) to fetched the cached object</param>
        /// <returns>the cached object</returns>
        T Restore(object key);

        /// <summary>
        /// To remove the object from being cached any more
        /// </summary>
        /// <param name="key">object is correspondent to the provided key will be removed from the cache storage</param>
        /// <returns></returns>
        T Flush(object key);
    }


    public class CachFactory<T> : ICacheable<T>
    {
        static Dictionary<object, T> _defaultStorage = new Dictionary<object, T>();
        IDictionary _storage = null;



        #region Constructors

        public CachFactory() : this(_defaultStorage)
        { }


        /// <summary>
        /// To construct a factory to cash object
        /// </summary>
        /// <param name="Storage">It is used as a source to enable caching and cache objects to restore later</param>
        public CachFactory(IDictionary storage)
        {
            SetDataSource(storage);
        }

        #endregion

        public void SetDataSource(IDictionary storage)
        {
            _storage = storage == null ? _defaultStorage : storage;
        }


        // ----------------------------------------------- ICacheable

        public virtual object Cache(T obj, object key = default(object))
        {
            key = key == default(object) ? Guid.NewGuid() : key;

            if (_storage.Contains(key))
                _storage.Remove(key);

            _storage.Add(key, obj);

            return key;
        }
        
        public virtual T Restore(object key)
        {
            if (!_storage.Contains(key)) return default(T);
            else return (T)_storage[key];
        }

        public virtual T Flush(object key)
        {
            T t;
            if (_storage.Contains(key))
            {
                t = (T)_storage[key];
                _storage.Remove(key);
            }
            else t = default(T);

            return t;
        }
    }
}