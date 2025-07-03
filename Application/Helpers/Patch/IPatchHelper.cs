using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers.Patch
{
    internal interface IPatchHelper
    {
        void PatchIfNotNull<T>(T source, T destination);
    }
}
