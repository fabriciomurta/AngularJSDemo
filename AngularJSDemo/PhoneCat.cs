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
            var appDepend = new string[]
            {
                "ngRoute",
                "phonecatControllers",
                "phonecatFilters"
            };
            var app = Angular.Module("phonecatApp", appDepend);

            app.Config<RouteProvider>(RouteProviderFn);

            var catCtl = Angular.Module("phonecatControllers");
            catCtl.Controller<PhoneListScopeModel, Http<PhoneModel[]>>("PhoneListCtrl", PhoneListCtrlFn);
            catCtl.Controller<PhoneDetailsScopeModel, PhoneModel, Http<PhoneDetailsModel>>("PhoneDetailCtrl", PhoneDetailCtrlFn);

            var catFlt = Angular.Module("phonecatFilters");

            // The following is equivalent to defining methods
            // - string mb(string text) { return "sometext"; }
            // and
            // - Func<string, string> ma() { return mb; }
            // Then calling .Filter("text", ma);
            catFlt.Filter("checkmark", () =>
            {
                return (input) =>
                {
                    return (input == "true") ? "\u2713" : "\u2718";
                };
            });
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
            [Name("$scope")] PhoneListScopeModel scope,
            [Name("$http")] Http<PhoneModel[]> http)
        {
            var httpResult = http.Get("data/phones.json");
            httpResult.Success((data) => { scope.Phones = data; });

            scope.OrderProp = "age";
        }

        public static void PhoneDetailCtrlFn(
            [Name("$scope")] PhoneDetailsScopeModel scope,
            [Name("$routeParams")] PhoneModel routeParams,
            [Name("$http")] Http<PhoneDetailsModel> http)
        {
            var httpResult = http.Get("data/" + routeParams.Id + ".json");

            httpResult.Success((data) => { scope.Phone = data; });
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

    public class PhoneListScopeModel
    {
        public PhoneModel[] Phones;
        public string OrderProp;
    }

    public class PhoneDetailsScopeModel
    {
        public PhoneDetailsModel Phone;
    }

    public class PhoneDetailsModel
    {
        public string Id;
        public string Name;
        public string AdditionalFeatures;
        public AndroidModel Android;
        public string[] Availability;
        public BatteryInfoModel Battery;
        public CameraInfoModel Camera;
        public ConnectivityInfoModel Connectivity;
        public string Description;
        public DisplayInfoModel Display;
        public HardwareInfoModel Hardware;
        public string[] Images;
        public SizeAndWeightInfoModel SizeAndWeight;
        public StorageInfoModel Storage;
    }

    public class AndroidModel
    {
        public string Os;
        public string Ui;
    }

    public class BatteryInfoModel
    {
        public string StandbyTime;
        public string TalkTime;
        public string Type;
    }

    public class CameraInfoModel
    {
        public string[] Features;
        public string Primart;
    }

    public class ConnectivityInfoModel
    {
        public string Bluetooth;
        public string Cell;
        public bool Gps;
        public bool Infrared;
        public string Wifi;
    }

    public class DisplayInfoModel
    {
        public string ScreenResolution;
        public string ScreenSize;
        public bool TouchScreen;
    }

    public class HardwareInfoModel
    {
        public bool Accelerometer;
        public string AudioJack;
        public string Cpu;
        public bool FmRadio;
        public bool PhysicalKeyboard;
        public string Usb;
    }

    public class SizeAndWeightInfoModel
    {
        public string[] Dimensions;
        public string Weight;
    }

    public class StorageInfoModel
    {
        public string Flash;
        public string Ram;
    }
}
