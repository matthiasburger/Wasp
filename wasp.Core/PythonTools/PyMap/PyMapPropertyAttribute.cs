using System;

namespace wasp.Core.PythonTools.PyMap
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class PyMapPropertyAttribute : Attribute
    {
        public PyMapPropertyAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}