using System;

namespace Wasp.Core.PythonTools
{
    public class PyPropertyAttribute : Attribute
    {
        public PyPropertyAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}