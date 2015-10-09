using Bridge;
using Bridge.AngularJS;
using Bridge.AngularJS.Route;
using Bridge.AngularJS.Services;
using Bridge.Html5;
using System;
using System.Collections.Generic;
using Bridge.AngularJS.Resource;

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
                "phonecatFilters",
                "phonecatServices"
            };
            var app = Angular.Module("phonecatApp", appDepend);

            app.Config<RouteProvider>(RouteProviderFn);

            var catCtl = Angular.Module("phonecatControllers");
            catCtl.Controller<PhoneListScopeModel,PhoneQueryModel>
                ("PhoneListCtrl", PhoneListCtrlFn);

            catCtl.Controller<PhoneDetailsScopeModel, PhoneModel, 
                PhoneQueryModel>("PhoneDetailCtrl", PhoneDetailCtrlFn);

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

            InitServices();
        }

        public static void InitServices()
        {
            var phonecatServices = Angular.Module("phonecatServices",
                                   new string[] { "ngResource" });

            phonecatServices.Factory<Func<Func<string, object, ResourceActions,
                Resource>, Resource>>("phoneService", PhoneServicesFactoryFn);
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
            PhoneQueryModel phoneService) // this MUST match the service name
        {
            scope.Phones = phoneService.Query();

            scope.OrderProp = "age";
        }

        public static void PhoneDetailCtrlFn(
            [Name("$scope")] PhoneDetailsScopeModel scope,
            [Name("$routeParams")] PhoneModel routeParams,
            PhoneQueryModel phoneService) // this MUST match the service name
        {
            scope.Phone = phoneService.Get(
                new { Id = routeParams.Id },
                (phone) =>
                {
                    scope.MainImageUrl = phone.Images[0];
                }
            );

            scope.SetImage = (imageUrl) =>
            {
                scope.MainImageUrl = imageUrl;
            };
        }

        public static Resource PhoneServicesFactoryFn(
            [Name("$resource")]
            Func<string, object, ResourceActions, Resource>
            resource)
        {
            return resource("data/:id.json", new object { },
                new ResourceActions
            {
                Query = new ActionInfo()
                {
                    Method = "GET",
                    Params = new { Id = "phones" },
                    IsArray = true
                }
            });
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

    // We ignore this because it is just a wrapper to the service's 'query'
    // action.
    [Ignore]
    public class PhoneQueryModel
    {
        public PhoneModel[] Query() {
            return default(PhoneModel[]);
        }

        public PhoneDetailsModel Get(object phoneId,
            Action<PhoneDetailsModel> phoneTask)
        {
            return default(PhoneDetailsModel);
        }
    }

    public class PhoneDetailsScopeModel
    {
        public PhoneDetailsModel Phone;
        public string MainImageUrl;

        public Action<string> SetImage;

        /*
         * One could think on making the SetImage function like this:
         *public void SetImage(string imageUrl)
         *{
         * this.MainImageUrl = imageUrl;
         *}
         * But it will not work with AngularJS: it has to be defined on the
         * scope definition (see above).
         */
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
