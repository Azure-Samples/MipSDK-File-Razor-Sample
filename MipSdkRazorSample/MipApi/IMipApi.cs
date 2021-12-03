using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Graph;
using Microsoft.InformationProtection;
using Microsoft.InformationProtection.Exceptions;
using Microsoft.InformationProtection.File;


namespace MipSdkRazorSample.MipApi
{
    public interface IMipApi
    {
        void InitializeMip(string clientId, string appName, string appVersion);

        ContentLabel GetFileLabel(Stream inputStream);
    }
}
