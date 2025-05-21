using Microsoft.JSInterop;

namespace WWWeasySub.Handler
{
    public class LanguageHeaderHandler : DelegatingHandler
    {
        private readonly IJSRuntime _jsRuntime;

        public LanguageHeaderHandler(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Récupérer la langue depuis localStorage via JSInterop
            var cultureName = await _jsRuntime.InvokeAsync<string>("blazorCulture.get");
            if (!string.IsNullOrEmpty(cultureName))
            {
                if (request.Headers.Contains("Accept-Language"))
                    request.Headers.Remove("Accept-Language");

                request.Headers.Add("Accept-Language", cultureName);
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
