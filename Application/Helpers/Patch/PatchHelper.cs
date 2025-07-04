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
        public void PatchIfNotNull<TSource, TTarget>(TSource source, TTarget target)
        {
            if (source == null || target == null) return;

            var sourceProps = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var targetProps = typeof(TTarget).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                             .Where(p => p.CanWrite).ToDictionary(p => p.Name);

            foreach (var sp in sourceProps)
            {
                if (!targetProps.TryGetValue(sp.Name, out var tp)) continue;
                if (!tp.CanWrite || tp.PropertyType != sp.PropertyType) continue;

                var value = sp.GetValue(source);
                if (value == null) continue;

                if (sp.PropertyType.IsValueType)
                {
                    var defaultValue = Activator.CreateInstance(sp.PropertyType);
                    if (value.Equals(defaultValue)) continue;
                }

                tp.SetValue(target, value);
            }
        }

        public Dictionary<string, object> GetPatchedValues<T>(T dto)
        {
            var patchedProperties = new Dictionary<string, object>();
            var type = typeof(T);

            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!prop.CanRead || prop.GetIndexParameters().Length > 0)
                    continue;

                var value = prop.GetValue(dto);
                var defaultValue = GetDefault(prop.PropertyType);

                // Check if value is different from default
                if (value != null && !Equals(value, defaultValue))
                {
                    patchedProperties[prop.Name] = value;
                }
            }

            return patchedProperties;
        }

        private object? GetDefault(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}
