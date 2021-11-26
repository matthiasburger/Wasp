using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Python.Runtime;

namespace wasp.Core.PythonTools.PyMap
{
    public static class PyMapping
    {
        private readonly struct PropertyValueMapping
        {
            public PropertyInfo Property { get; init; }
            public PyObject Value { get; init; }
        }

        public static T MapTo<T>(this PyDict pyDict) where T : new()
        {
            T targetObject = new();

            IEnumerable<PropertyValueMapping> properties = from property in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                let noMapAttribute = property.GetCustomAttribute<PyMapIgnoreAttribute>()
                where noMapAttribute is null
                let attribute = property.GetCustomAttribute<PyMapPropertyAttribute>()?.Name
                let hasKey = attribute != null && pyDict.HasKey(attribute)
                let value = hasKey ? pyDict[attribute] : pyDict.HasKey(property.Name) ? pyDict[property.Name] : PyObject.None
                where value != null && !value.IsNone() && property.SetMethod != null
                select new PropertyValueMapping { Property = property, Value = value };

            foreach (PropertyValueMapping property in properties.ToArray())
            {
                Type propertyType = property.Property.PropertyType;
                object valueToSet = property.Value.AsManagedObject(propertyType);
                property.Property.SetValue(targetObject, valueToSet);
            }

            return targetObject;
        }
    }
}