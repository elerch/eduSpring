using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace Advantages
{
    class Advantages
    {
        static void Main(string[] args)
        {
            // Without DI
            new MyClass().DoWork("foo", new SecureString().Append("bar"));
            
            // With DI
            var testSystemProvider = new TestingAuthenticationProvider();
            var productionSystemProvider = new ProductionAuthenticationProvider();
            // The choice of test/production is now made here, outside of the class.  
            // The class shouldn't "care" who does the authentication work, it's job is to do other work
            // The new statement in the next line would be done through an IoC container (e.g. Spring) in a 
            // real environment.
            new MyClassWithDependencyInjection(testSystemProvider).DoWork("foo", new SecureString().Append("bar"));

            // Switch the constructor (in Spring.NET, done through a config file change) and now we're running against 
            // "production".  No recompile necessary
            new MyClassWithDependencyInjection(productionSystemProvider).DoWork("foo", new SecureString().Append("bar"));

            // We can also do this without a container.  Here, we'll get type information from the command line.
            // Change the provider type via project properties to see how this works.
            // The next line is basically what an IoC container would do
            var myAuthProvider = (IAuthenticate)Activator.CreateInstance(Type.GetType(args[0]));
            new MyClassWithDependencyInjection(myAuthProvider).DoWork("foo", new SecureString().Append("bar"));
        }
    }

    /// <summary>
    /// Simulates a typical class
    /// </summary>
    public class MyClass
    {
        /// <summary>
        /// Authentication provider.  We can't change this out for the production version 
        /// without changing source code and recompiling
        /// </summary>
        private readonly MyAuthenticationProvider authenticationProvider = new MyAuthenticationProvider();

        // Comment the line above, uncomment and recompile for production.  
        // 
        // By recompiling, you won't know if you're testing the same
        // code that exists in production (did you just change this line, or something else too?)  Versions might 
        // be different, etc.
        //private readonly MyProductionAuthenticationProvider authenticationProvider = new MyProductionAuthenticationProvider();

        public void DoWork(string user, SecureString password)
        {
            if(!authenticationProvider.Authenticate(user,password).Identity.IsAuthenticated)
                throw new SecurityException("Not authorized to perform this action");
            Thread.Sleep(3000);// Simulate some work
        }
    }

    class MyAuthenticationProvider
    {
        public IPrincipal Authenticate(string user, SecureString password)
        {
            // Implementation for development
            return new GenericPrincipal(new GenericIdentity(user), new string[] {});
        }
    }

    class MyProductionAuthenticationProvider
    {
        public IPrincipal Authenticate(string user, SecureString password)
        {
            if(user == "foo" && password.StringEquals("bar"))
                return new GenericPrincipal(new GenericIdentity(user), new string[] { });
            return new GenericPrincipal(new GenericIdentity(""), new string[] { }); ;
        }
    }
}
