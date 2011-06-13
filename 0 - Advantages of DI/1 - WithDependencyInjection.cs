using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace Advantages
{
    /// <summary>
    /// Simulates a typical class
    /// </summary>
    public class MyClassWithDependencyInjection
    {
        /// <summary>
        /// Authentication provider.  Because we use an interface, we no longer 
        /// need to change this code after it's been tested
        /// </summary>
        private readonly IAuthenticate _authenticationProvider;

        /// <summary>
        /// Constructor to establish this class' dependencies.  Since the 
        /// class is not "complete" (can't operate) without an authentication
        /// provider, we require an object up front.
        /// </summary>
        /// <param name="authProvider"></param>
        public MyClassWithDependencyInjection(IAuthenticate authProvider)
        {
            if (authProvider == null)
                throw new ArgumentNullException("authProvider");
            _authenticationProvider = authProvider;
        }

        public void DoWork(string user, SecureString password)
        {
            if (!_authenticationProvider.Authenticate(user, password).Identity.IsAuthenticated)
                throw new SecurityException("Not authorized to perform this action");
            Thread.Sleep(3000);// Simulate some work
        }
    }

    /// <summary>
    /// New Interface introduced.  Our two classes don't have a real-world inheritance relationship,
    /// but the authentication process is identical, so we build a contract
    /// </summary>
    public interface IAuthenticate
    {
        IPrincipal Authenticate(string user, SecureString password);
    }

    class TestingAuthenticationProvider : IAuthenticate
    {
        public IPrincipal Authenticate(string user, SecureString password)
        {
            // Implementation for development
            return new GenericPrincipal(new GenericIdentity(user), new string[] { });
        }
    }

    class ProductionAuthenticationProvider : IAuthenticate
    {
        public IPrincipal Authenticate(string user, SecureString password)
        {
            if (user == "foo" && password.StringEquals("bar"))
                return new GenericPrincipal(new GenericIdentity(user), new string[] { });
            return new GenericPrincipal(new GenericIdentity(""), new string[] { }); ;
        }
    }

}
