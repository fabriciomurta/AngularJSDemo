using Bridge;
using Bridge.AngularJS;
using Bridge.Html5;
using System;
using System.Collections.Generic;

namespace AngularJSDemo
{
    /// <summary>
    /// Checkpoints:
    /// Alpha Bravo Charlie Delta Echo Foxtrot Golf Hotel India
    /// Juliet Kilo Lima Mike November Oscar Papa Quebec Romeo
    /// Sierra Tango Uniform Victor Whiskey X-ray Yankee Zulu
    /// http://home.earthlink.net/~malcolmhamer/alpha.htm
    /// </summary>
    public class AngularJSDemo
    {

        private class ControllerDataObjectStructure
        {
            public string Message { get; set; }
            public string Foo { get; set; }
            public string Bar { get; set; }
            public string[] Checkpoints { get; set; }
        }

        static AngularJSDemo()
        {
            Console.Log("Checkpoint Yankee: [ctor] nothing at this point will like AngularJS.");
        }

        public static AngularJSApp hwbApp;

        /// <summary>
        /// AngularJS requires to be defined on document parse.
        /// TODO: create an onParse attribute
        /// </summary>
        public static void StartUp()
        {
            Console.Log("Checkpoint Charlie: I must be printed BEFORE Checkpoint 1.");

            AngularJSDemo.hwbApp = new AngularJSApp("hwbApp");

            var controllerData = new { message = "Hello, AngularJS message defined in Bridge's C#! :D" };
            AngularJSDemo.hwbApp.Controller("hwbctl", controllerData);
            AngularJSDemo.hwbApp.Directive("brdEntryPoint", AngularJSDemo.dynMehTemplate);

            var checkpoints = new List<string> { "Alpha", "Baker", "Charlie", "Delta" };

            var controllerStrongData = new ControllerDataObjectStructure()
            {
                Message = "Hello, AngularJS message defined in Bridge's C#'s strongly typed class! :D",
                Foo = "Foo fighters from strong C#.",
                Bar = "Strong C# 777 slot",
                Checkpoints = checkpoints.ToArray()
            };
            AngularJSDemo.hwbApp.Controller("hwbSctl", controllerStrongData);
            AngularJSDemo.hwbApp.Directive("brdEntryPointForThree", AngularJSDemo.ThreeWayFunction);

            Document.DocumentElement.setNGApp(AngularJSDemo.hwbApp);
        }

        /// <summary>
        /// This function determines the format of the contents of the entry point where the dynamic
        /// code will be injected.
        /// </summary>
        /// <returns></returns>
        public static object dynMehTemplate()
        {
            var span = new SpanElement();
            span.setNGController("hwbctl");
            span.InnerHTML = "AJS says by directive: [{{message}}]";
            return new { template = span.OuterHTML };
        }
        public static object ThreeWayFunction()
        {
            var span = new SpanElement();
            span.setNGController("hwbSctl");
            span.InnerHTML = "AJS for three scopined: [msg:{{Message}}][foo:{{Foo}}][bar:{{Bar}}]";
            return new { template = span.OuterHTML };
        }

        // This one must be called after the tag has been defined in the HTML DOM, yet before
        // HTML DOM has been loaded (DOMContentLoaded)
        public static void StartUpIdPhase()
        {
            if (AngularJSDemo.hwbApp == null) {
                AngularJSDemo.StartUp();
            }

            var x = Document.GetElementById("entryPoint");
            if (x != null)
            {
                //x.SetAttribute("ng-controller", "hwbctl"); // same as below
                x.setNGController("hwbctl");
                x.InnerHTML = "AJS says by element ID: [{{message}}]";
            }

            Document.Body.AppendChild(AngularJSDemo.GetRepeatRegion());
        }

        public static Element GetRepeatRegion() {
            var itemsSpan = new SpanElement();
            itemsSpan.InnerHTML = "Checkpoint";
            var itemsPara = new ParagraphElement();
            itemsPara.InnerHTML = "{{checkpoint}}";

            var itemsLI = new LIElement();
            itemsLI.setNGRepeat("checkpoint", "Checkpoints");
            itemsLI.AppendChild(itemsSpan);
            itemsLI.AppendChild(itemsPara);

            var itemsUL = new UListElement();
            itemsUL.AppendChild(itemsLI);

            var itemsSubSpan = new SpanElement()
            {
                InnerHTML = "[{{checkpoint}}] "
            };

            itemsSubSpan.setNGRepeat("checkpoint", "Checkpoints", "cpFilter");

            var itemsSearchBox = new InputElement();
            itemsSearchBox.setNGModel("cpFilter");

            var itemsDiv = new DivElement();
            itemsDiv.setNGController("hwbSctl");
            itemsDiv.AppendChild(itemsUL);
            itemsDiv.AppendChild(itemsSearchBox);
            itemsDiv.AppendChild(itemsSubSpan);

            return itemsDiv;
        }

        /// <summary>
        /// Trying to define AngularJS controller/app after DOM is loaded will not work.
        /// Angular is supposed to be defined during the page composition, not after it is ready.
        /// </summary>
        [Ready]
        public static void UpdateControls()
        {
            Console.Log("Checkpoint Zulu: [rdy] nothing at this point will like AngularJS.");

            var lbl = new SpanElement() { InnerHTML = "We have Bridge script running." };
            Document.Body.AppendChild(lbl);
        }
    }
}