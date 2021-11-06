using System.Threading.Tasks;

using Shouldly;

using WorkflowDemo.Models.TokenAuth;
using WorkflowDemo.Web.Host.Controllers;

using Xunit;

namespace WorkflowDemo.Web.Tests
{
    public class HomeController_Tests : WorkflowDemoWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}