using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EduSpring
{
    class Program
    {
        /// <summary>
        /// This mimics the ASP.NET Framework
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //These two lines are handled by the Spring.NET HttpModule
            var container = new IoCContainer();
            container.Initialize();

            // These two lines are also handled by the HttpModule by a special 
            // syntax in the spring configuration
            var service = new DoSomeWork();
            service.Worker = container.GetObject<IDoSomething>("myObject");

            // This is what the ASP.NET Framework would do
            Console.WriteLine("The output is: " + service.DoTheWork());
        }
    }

    /// <summary>
    /// This mimics an ASPX page (or service, or anything that does real work
    /// </summary>
    internal class DoSomeWork
    {
        // We use a property here.  We are not, and should not
        // be responsible for defining who does our bidding, only 
        // that we have a dependency on a worker, and that worker
        // needs to do it.
        public IDoSomething Worker { get; set; }
        public string DoTheWork()
        {
            return Worker.DoSomething(null);
        }
    }

    class IoCContainer
    {
        private readonly IDictionary<string, object> _allObjects = new Dictionary<string, object>();

        public T GetObject<T>(string objectName)
        {
            return (T)_allObjects[objectName];
        }

        public void Initialize()
        {
            // Wouldn't it be nice if we could configure this through 
            // app.config or web.config?  Spring does that!
            _allObjects.Add("myObject", new ClassA());
        }
    }

    internal interface IDoSomething
    {
        string DoSomething(object foo);
    }

    public class ClassA : IDoSomething
    {
        public string DoSomething(object foo)
        {
            return "fizz";
        }
    }
    public class ClassB : IDoSomething
    {
        public string DoSomething(object foo)
        {
            return "buzz";
        }
    }
}
