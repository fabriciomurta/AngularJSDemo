using Bridge;
using Bridge.AngularJS;
using Bridge.AngularJS.Route;
using Bridge.AngularJS.Services;
using Bridge.Html5;
using System;
using System.Collections.Generic;

namespace PhoneCat
{
    public class PhoneCat
    {
        public PhoneCat()
        {
        }

        public static void Init() {
            var app = Angular.Module("phonecatApp", new string[] { "ngRoute", "phonecatControllers" });

            app.Config<RouteProvider>(RouteProviderFn);

            var catCtl = Angular.Module("phonecatControllers");
            catCtl.Controller<PhoneListModel, Http<PhoneModel[]>>("PhoneListCtrl", PhoneListCtrlFn);
            catCtl.Controller<PhoneModel, PhoneModel>("PhoneDetailCtrl", PhoneDetailCtrlFn);
        }

        public static void RouteProviderFn(
            [Name("$routeProvider")] RouteProvider routeProvider)
        {
            routeProvider.When("/phones", new MappingInformation
            {
                TemplateUrl = "partials/phone-list.html",
                Controller = "PhoneListCtrl"
            }).When("/phones/:id", new MappingInformation
            {
                TemplateUrl = "partials/phone-detail.html",
                Controller = "PhoneDetailCtrl"
            }).Otherwise(new MappingInformation
            {
                RedirectTo = "/phones"
            });
        }

        public static void PhoneListCtrlFn(
            [Name("$scope")] PhoneListModel scope,
            [Name("$http")] Http<PhoneModel[]> http)
        {
            var httpResult = http.Get("data/phones.json");
            httpResult.Success((data) => { scope.Phones = data; });

            scope.OrderProp = "age";
        }

        public static void PhoneDetailCtrlFn(
            [Name("$scope")] PhoneModel scope,
            [Name("$routeParams")] PhoneModel routeParams)
        {
            scope.Id = routeParams.Id;
        }
    }

    public class PhoneModel
    {
        public int Age;
        public string Id;
        public string ImageUrl;
        public string Name;
        public string Snippet;
    }

    public class PhoneListModel
    {
        public PhoneModel[] Phones;
        public string OrderProp;
    }
}