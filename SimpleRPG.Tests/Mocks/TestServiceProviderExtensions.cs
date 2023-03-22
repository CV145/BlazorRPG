using Blazorise.Bootstrap;
using Blazorise;
using Bunit;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleRPG.Tests.Mocks
{
    public static class TestServiceProviderExtensions
    {
        public static void AddBlazoriseServices(this TestServiceProvider services)
        {
            services.AddSingleton<IClassProvider>(new BootstrapClassProvider());
            services.AddSingleton<IStyleProvider>(new BootstrapStyleProvider());
            services.AddSingleton<IJSRunner>(new BootstrapJSRunner(new MockJSRuntime()));
            services.AddSingleton<IJSRuntime>(new MockJSRuntime());
            services.AddSingleton<IComponentMapper>(new ComponentMapper());
            services.AddSingleton<IThemeGenerator>(new BootstrapThemeGenerator());
            services.AddSingleton<IIconProvider>(new MockIconProvider());
            services.AddSingleton<BlazoriseOptions>(new BlazoriseOptions());
        }

        class MockJSRuntime : IJSRuntime
        {
            public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object[] args)
            {
                return new ValueTask<TValue>();
            }

            public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object[] args)
            {
                return new ValueTask<TValue>();
            }
        }

        class MockIconProvider : IIconProvider
        {
            public bool IconNameAsContent => false;

            public string GetIconName(IconName name)
            {
                return string.Empty;
            }

            public string GetIconName(string customName)
            {
                return string.Empty;
            }

            public string Icon(object name, IconStyle iconStyle)
            {
                return string.Empty;
            }

            public void SetIconName(IconName name, string newName)
            {
            }
        }
    }
}
