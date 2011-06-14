using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FallaciesOfInterfaces
{

    class Program
    {
        static void Main(string[] args)
        {
            var worker = new UseThisInternallyForDev() as IDoSomething;
            Console.WriteLine(int.Parse(worker.DoSomething(null)));
            worker = new UseThisInTest();
            Console.WriteLine(int.Parse(worker.DoSomething(null)));
            worker = new UseThisInProduction();
            Console.WriteLine(int.Parse(worker.DoSomething(null)));
        }
    }


    internal interface IDoSomething
    {
        string DoSomething(object foo);
    }

    class UseThisInternallyForDev : IDoSomething
    {
        public string DoSomething(object foo)
        {
            return "3";
        }
    }
    class UseThisInTest : IDoSomething
    {
        public string DoSomething(object foo)
        {
            // Is the caller expecting a null return?
            return null;
        }
    }
    class UseThisInProduction : IDoSomething
    {        
        public string DoSomething(object foo)
        {
            // Implicitly we require foo must not be null
            // for the first time, we can get an exception
            return foo.GetType().FullName;
        }
    }



}
