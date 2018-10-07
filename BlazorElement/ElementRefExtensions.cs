using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor;
using Microsoft.JSInterop;

namespace BlazorElement
{
    public static class ElementRefExtensions
    {
        public static Task Focus(this ElementRef elementRef)
        {
            return JSRuntime.Current.InvokeAsync<object>("blazorElement.focusElement", elementRef);
        }

        public static Task Blur(this ElementRef elementRef)
        {
            return JSRuntime.Current.InvokeAsync<object>("blazorElement.blurElement", elementRef);
        }
    }
}