using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadosCatFeeding.Abstractions
{
    public interface IRepository<T>
    {
        public string ConnectionString { get; set; }

        T Get(int id);
        List<T> GetAll();
        void Delete(int id);
        void Update(int id, T info);
        int Create(T info);
    }
}
