using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Advantages
{
    public static class SecureStringExtensions
    {
        /// <summary>
        /// Determines equality of two SecureStrings.  WARNING: Use of this method introduces
        /// a brief window of time that the contents of the SecureStrings will be visible in plaintext in memory
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static bool StringEquals(this SecureString value1, SecureString value2)
        {
            return value1.CompareTo(value2) == 0;
        }

        /// <summary>
        /// Determines equality of a SecureString with a normal string.  WARNING: Use of this method introduces
        /// a brief window of time that the contents of the SecureString will be visible in plaintext in memory
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static bool StringEquals(this SecureString value1, string value2)
        {
            return value1.CompareTo(value2) == 0;
        }

        /// <summary>
        /// Compares two SecureStrings.  WARNING: Use of this method introduces
        /// a brief window of time that the contents of the SecureStrings will be visible in plaintext in memory
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static int CompareTo(this SecureString value1, SecureString value2)
        {
            ValidateInput(value1, value2);

            if (value1.Length != value2.Length) 
                return value1.Length.CompareTo(value2.Length);

            var bstrPtr1 = IntPtr.Zero;
            var bstrPtr2 = IntPtr.Zero;

            try {
                // Open a security hole: necessary to provide the comparison
                // These two lines will decrypt the securestrings
                bstrPtr1 = Marshal.SecureStringToBSTR(value1);
                bstrPtr2 = Marshal.SecureStringToBSTR(value2);

                var str1 = Marshal.PtrToStringBSTR(bstrPtr1);
                var str2 = Marshal.PtrToStringBSTR(bstrPtr2);

                return str1.CompareTo(str2);
            }
            finally {
                if (bstrPtr1 != IntPtr.Zero)
                    Marshal.ZeroFreeBSTR(bstrPtr1);  // Close the security hole             

                if (bstrPtr2 != IntPtr.Zero)
                    Marshal.ZeroFreeBSTR(bstrPtr2);  // Close the security hole              
            }
        }


        private static void ValidateInput(object value1, object value2)
        {
            if (value1 == null) {
                throw new ArgumentNullException("value1");
            }
            if (value2 == null) {
                throw new ArgumentNullException("value2");
            }
        }

        /// <summary>
        /// Compares a SecureString with a normal string.  WARNING: Use of this method introduces
        /// a brief window of time that the contents of the SecureString will be visible in plaintext in memory
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static int CompareTo(this SecureString value1, string value2)
        {
            ValidateInput(value1, value2);

            if (value1.Length != value2.Length) {
                return value1.Length.CompareTo(value2.Length);            
            }

            var bstrPtr = IntPtr.Zero;

            try {
                // Open a security hole: necessary to provide the comparison
                // This line will decrypt the securestring
                bstrPtr = Marshal.SecureStringToBSTR(value1);
                
                var str1 = Marshal.PtrToStringBSTR(bstrPtr);

                return str1.CompareTo(value2);
            }
            finally {
                if (bstrPtr != IntPtr.Zero) 
                    Marshal.ZeroFreeBSTR(bstrPtr);  // Close the security hole
            }
        }

        /// <summary>
        /// Appends a string onto a SecureString
        /// </summary>
        /// <param name="value"></param>
        /// <param name="appendText"></param>
        /// <returns>The SecureString passed in</returns>
        public static SecureString Append(this SecureString value, string appendText)
        {
            if (value == null) throw new ArgumentNullException("value");
            foreach (var character in appendText) 
                value.AppendChar(character);
            return value;
        }
    }
}
