using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Bunit;
using BlazorRPG.Pages;
using SimpleRPG.Tests.Mocks;

namespace SimpleRPG.Tests.Pages
{
    public class MainScreenTests
    {
        [Fact]
        public void SimpleRender()
        {
            //arrange
            using var ctx = new TestContext();
            ctx.Services.AddBlazoriseServices();
            ctx.Services.AddSingleton<GameSessionTests>(session);

            //act
            var cut = ctx.RenderComponent<MainScreen>();

            // assert
            var expected = @"<th scope=""col"" class="""" style="""" blazor:onclick=""2"" rowspan=""2"">";
            Assert.Contains(expected, cut.Markup);
            Assert.Contains("Player Data", cut.Markup);
            Assert.Contains("DarthPedro", cut.Markup);
            Assert.Contains("Fighter", cut.Markup);
        }
    }
}
