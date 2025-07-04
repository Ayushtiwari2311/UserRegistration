using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers.Patch
{
    public interface IPatchHelper
    {
        void PatchIfNotNull<TSource, TTarget>(TSource source, TTarget target);
        Dictionary<string, object> GetPatchedValues<T>(T dto);
    }
}
