using System.Collections.Generic;
using System.Linq;
using DynamicLinqCore;


using SqlKata;
using SqlKata.Compilers;
using wasp.Core;
using wasp.WebApi.Data.Models;


namespace wasp.TestRun
{
    public class Program
    {
        public static void Main()
        {
            Console.Write(DeviceInformation.GetComputerName());
        }
    }
}