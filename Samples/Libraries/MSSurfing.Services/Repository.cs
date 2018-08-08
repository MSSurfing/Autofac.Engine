using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSSurfing.Services
{
    #region IRepository.cs file content
    public partial interface IRepository<T>
    {
        IQueryable<T> Table { get; }

        T Get(object id);

        bool Insert(T entity);

        bool Update(T entity);

        bool Delete(T entity);
    }
    #endregion

    #region Batch Repository.cs file content
    public partial class BatchRepository<T> : IRepository<T> where T : BaseEntity
    {
        #region Fields
        private static readonly object r_lock = new object();
        private List<T> _data;
        #endregion

        #region Ctor
        public BatchRepository()
        {
            _data = new List<T>();
        }
        #endregion

        #region Properties
        public virtual bool StoreInDb { get => true; }
        #endregion

        #region Utilities
        protected void LoadDbSet() {            /* Todo */        }
        protected void SaveChangeInDb(int intervalSeconds = 3600, int maxTotal = 100000) {            /* Todo */        }
        #endregion

        #region Methods


        public IQueryable<T> Table => _data.AsQueryable();

        public T Get(object id)
        {
            return _data.FirstOrDefault(e => e.Id.Equals(id));
        }

        public bool Insert(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            if (_data.Any(e => e.Id.Equals(entity.Id)))
                return false;

            lock (r_lock)
                _data.Add(entity);

            return true;
        }

        public bool Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var item = _data.FirstOrDefault(e => e.Id.Equals(entity.Id));
            if (item == null)
                return false;

            lock (r_lock)
            {
                _data.Remove(item);
                _data.Add(entity);
            }
            return true;
        }

        public bool Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var item = _data.FirstOrDefault(e => e.Id.Equals(entity.Id));
            if (item == null)
                return false;

            lock (r_lock)
                _data.Remove(item);

            return true;
        }
        #endregion
    }
    #endregion
}
