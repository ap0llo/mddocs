using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc.Pages
{
    public interface IPage
    {
        OutputPath OutputPath { get; }

        void Save();
    }
}
