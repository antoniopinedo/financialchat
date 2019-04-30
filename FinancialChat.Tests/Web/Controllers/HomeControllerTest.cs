using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FinancialChat;
using FinancialChat.Controllers;
using Microsoft.QualityTools.Testing.Fakes;
using System.Web;
using System.Web.Fakes;
using System.Web.Hosting;
using System.IO;
using System.Security.Principal;
using System.Web.Routing;
using System.Security.Principal.Fakes;

namespace FinancialChat.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index_Authenticated()
        {
            // Arrange:            
            using (ShimsContext.Create())
            {
                using (HomeController controller = new HomeController())
                {
                    StubHttpContextBase stubHttpContext = new StubHttpContextBase();

                    controller.ControllerContext = new ControllerContext(stubHttpContext, new RouteData(), controller);
                    
                    // Shim the HttpContext.Current
                    var httpRequest = new HttpRequest("", "http://localhost", "");
                    var httpContext = new HttpContext(httpRequest, new HttpResponse(new StringWriter()));
                    var applicationState = httpContext.Application;
                    ShimHttpContext.CurrentGet = () => httpContext;
                    
                    // Stub the Current.User
                    StubIPrincipal principal = new StubIPrincipal();
                    principal.IdentityGet = () =>
                    {
                        return new StubIIdentity
                        {
                            NameGet = () => "antonio",
                            IsAuthenticatedGet = () => true
                        };
                    };
                    stubHttpContext.UserGet = () => principal;

                    // Act:
                    ActionResult result = controller.Index();

                    // Assert:
                    Assert.IsNotNull(result);
                }
            }
        }

        [TestMethod]
        public void Index_Not_Authenticated()
        {
            // Arrange:            
            using (ShimsContext.Create())
            {
                using (HomeController controller = new HomeController())
                {
                    StubHttpContextBase stubHttpContext = new StubHttpContextBase();

                    controller.ControllerContext = new ControllerContext(stubHttpContext, new RouteData(), controller);

                    // Shim the HttpContext.Current
                    var httpRequest = new HttpRequest("", "http://localhost", "");
                    var httpContext = new HttpContext(httpRequest, new HttpResponse(new StringWriter()));
                    var applicationState = httpContext.Application;
                    ShimHttpContext.CurrentGet = () => httpContext;

                    // Stub the Current.User
                    StubIPrincipal principal = new StubIPrincipal();
                    principal.IdentityGet = () =>
                    {
                        return new StubIIdentity
                        {
                            NameGet = () => "antonio",
                            IsAuthenticatedGet = () => false
                        };
                    };
                    stubHttpContext.UserGet = () => principal;

                    // Act:
                    ActionResult result = controller.Index();

                    // Assert:
                    Assert.IsNotNull(result);
                }
            }
        }
    }
}
