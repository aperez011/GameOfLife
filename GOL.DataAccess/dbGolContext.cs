using GOL.Entities;
using GOL.Utilities.DB;
using LiteDB;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;

namespace GOL.DataAccess
{
    public class dbGolContext
    {

        private readonly LiteDatabase _db;
        public dbGolContext()
        {
            try
            {
                var db = new LiteDatabase(@"GOL.db");
                if (db != null)
                    _db = db;
            }
            catch (Exception ex)
            {
                throw new Exception("Can find or create LiteDb database.", ex);
            }
        }

        public IEnumerable<T> FindAll<T>() where T : EntityBase, new()
        {
            return _db.GetCollection<T>(typeof(T).Name)
                .FindAll();
        }

        public T FindOne<T>(Guid gid) where T : EntityBase, new()
        {
            return _db.GetCollection<T>(typeof(T).Name)
                .Find(x => x.GID == gid).FirstOrDefault() ?? new T();
        }

        public IEnumerable<T> FindBy<T>(Expression<Func<T, bool>> condition) where T : EntityBase, new()
        {
            return _db.GetCollection<T>(typeof(T).Name)
                .Find(condition);
        }

        public int Insert<T>(T forecast) where T : EntityBase, new()
        {
            return _db.GetCollection<T>(typeof(T).Name)
                .Insert(forecast);
        }

        public bool Update<T>(T forecast) where T : EntityBase, new()
        {
            return _db.GetCollection<T>(typeof(T).Name)
                .Update(forecast);
        }

        /*public int Delete<T>(int id) where T : EntityBase, new()
        {
            return _db.GetCollection<T>(typeof(T).Name)
                .Delete(x => x.Id == id);
        }*/

    }
}