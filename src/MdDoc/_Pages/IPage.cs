using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc
{
    interface IPage
    {
        string Name { get; }

        void Save();
    }
}
