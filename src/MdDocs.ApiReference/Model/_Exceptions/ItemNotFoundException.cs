using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grynwald.MdDocs.ApiReference.Model
{
    public class ItemNotFoundException : ModelException
    {
        public ItemNotFoundException(string message) : base(message)
        { }
    }
}
