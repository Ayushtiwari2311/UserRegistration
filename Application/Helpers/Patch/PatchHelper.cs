using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers.Patch
{
    public class PatchHelper : IPatchHelper
    {
        public void PatchIfNotNull<T>(T source, T destination)
        {
            if (source == null || destination == null) return;

            var type = typeof(T);
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                            .Where(p => p.CanRead && p.CanWrite);

            foreach (var prop in props)
            {
                var value = prop.GetValue(source);

                if (value == null) continue;

                if (prop.PropertyType.IsValueType)
                {
                    // Skip default values for value types (e.g., 0, DateTime.MinValue)
                    var defaultValue = Activator.CreateInstance(prop.PropertyType);
                    if (value.Equals(defaultValue))
                        continue;
                }

                prop.SetValue(destination, value);
            }
        }
    }
}
