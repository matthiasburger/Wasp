using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Python.Runtime;

namespace Wasp.Core.PythonTools
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
                let attribute = property.GetCustomAttribute<PyPropertyAttribute>()?.Name
                let value = (attribute != null && pyDict.HasKey(attribute)) ? pyDict[attribute] : pyDict.HasKey(property.Name) ? pyDict[property.Name] : PyObject.None
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