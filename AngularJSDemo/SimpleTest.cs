using Bridge.AngularJS;
using Bridge.Html5;
using System;
using Bridge;

namespace SimpleTest
{
    public class TestModel
    {
        public string[] MyItems;
    }

    public static class TestClass
    {
        public static void StartUp()
        {
            var app = new AngularJSApp("MyApp");

            app.Controller<TestModel>("MyCtl", TestControl);
        }

        public static void TestControl([Name("$scope")] TestModel scope)
        {
            scope.MyItems = new string [] { "First", "Second" };
        }
    }
}
