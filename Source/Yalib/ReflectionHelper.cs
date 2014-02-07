using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web;
using System.IO;

namespace Hlt
{
    public static class ReflectionHelper
    {
        public static string GetAssemblyFullPath(Assembly asmb)
        {
            return System.IO.Path.GetDirectoryName(asmb.Location);
//            Uri codeBaseUri = new Uri(asmb.CodeBase);
//            return codeBaseUri.AbsolutePath; // format: c:/aaa/bbb/ccc
        }

        public static object GetProperty(object theObject, string propertyName)
        {
            var aType = theObject.GetType();
            return aType.InvokeMember(propertyName, BindingFlags.GetProperty, null, theObject, null);
        }

        public static void SetProperty(object theObject, string propertyName, object newValue)
        {
            var aType = theObject.GetType();
            aType.InvokeMember(propertyName, BindingFlags.SetProperty, null, theObject, new object[] { newValue });
        }

        public static bool HasProperty(object theObject, string propertyName)
        {
            var aType = theObject.GetType();
            return aType.GetProperty(propertyName) != null;
        }


    }
}