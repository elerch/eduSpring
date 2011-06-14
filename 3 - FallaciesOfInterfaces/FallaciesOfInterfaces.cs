using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FallaciesOfInterfaces
{

    class Program
    {
        static void Main(string[] args)
        {
            var myStringBuilder = new StringBuilder("foo");
            var myString = myStringBuilder.ToString();
            int result;

            // I can assume myGuid.Value.ToString() == GuidText
            // After all, I never modify the value...

            // In dev, this works great!  It returns "1", which is parsed
            var worker = new UseThisInternallyForDev() as IDoSomething;
            Console.WriteLine(int.Parse(worker.DoSomething(null)));
            Debug.Assert(myStringBuilder.ToString() == myString);

            // Now in test, null doesn't work anymore. The implementation
            // returns null, which will no longer parse to an int
            // Interfaces don't protect us from this oversight
            worker = new UseThisInTest();
            Console.WriteLine(int.Parse(worker.DoSomething(null)));
            // It fails, so we put in a null coalesce to cover the condition: 
            //Console.WriteLine(int.Parse(worker.DoSomething(null) ?? "2" )); 
            Debug.Assert(myStringBuilder.ToString() == myString);

            worker = new UseThisInProduction();
            // Now we've got some solid code, but we get to production, and it 
            // still fails!  For this implementation we can't pass in null,
            // and the return value we get can't be parsed to an int either..
            Console.WriteLine(int.Parse(worker.DoSomething(null) ?? "3"));
            // So we modify to this:            
            //if (int.TryParse(worker.DoSomething(myStringBuilder), out result))
            //    Console.WriteLine(result);
            //else
            //    Console.WriteLine("3");
            Debug.Assert(myStringBuilder.ToString() == myString);


            // Now we've handled everything that could possibly go wrong,
            // so we roll this out to a new environment and...
            worker = new UseThisOnASeparateInstall();
            if (int.TryParse(worker.DoSomething(myStringBuilder), out result))
                Console.WriteLine(result);
            else
                Console.WriteLine("4");

            // Assertion failure.  In this environment, our dependency has mucked 
            // with our object on us!
            Debug.Assert(myStringBuilder.ToString() == myString);

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
            return "1";
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
    class UseThisOnASeparateInstall : IDoSomething
    {
        public string DoSomething(object foo)
        {
            var myStringBuilder = foo as StringBuilder;
            if (myStringBuilder != null) {
                myStringBuilder.Clear();
                myStringBuilder.Append("bar");
            }
            
            return foo.ToString();
        }
    }


}
