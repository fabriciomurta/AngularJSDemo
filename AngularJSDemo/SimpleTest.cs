using Bridge.Html5;
using System;
using Bridge;

namespace TestBed
{

    public static class TestClass
    {
        public class ModelClass
        {
            public string SpringField;
            public string SpringProp { get; set; }
        }

        public static void Main()
        {
            var flanders = new ModelClass();
            flanders.SpringField = "homer";
            flanders.SpringProp = "marjorie";

            Script.Write("console.log('SpringField:'+flanders.SpringField);");
            Script.Write("console.log('springField:'+flanders.springField);");
            Script.Write("console.log('SpringProp:'+flanders.SpringProp);");
            Script.Write("console.log('springProp:'+flanders.springProp);");

            InternalMechanism<ModelClass>(ParFuncSample);
        }

        public static void InternalMechanism<T>(Action<T> parfunc)
        {
            // No matter what happens here, this is library-specific limitation
            // and is not expressed thru bridge constraints.
            Script.Write("var parm = {}");
            Script.Write("parfunc(parm)");
        }

        public static void ParFuncSample(ModelClass param)
        {
        }
    }
}
