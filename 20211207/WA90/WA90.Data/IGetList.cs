using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WA90.Data
{
    public interface IGetList<T>
    {
        IEnumerable<T> GetList();
    }
}
