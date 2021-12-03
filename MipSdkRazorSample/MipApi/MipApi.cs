using Microsoft.InformationProtection;

namespace MipSdkRazorSample.MipApi
{
    public class MipApi : IMipApi
    {
        public ContentLabel GetFileLabel(Stream inputStream)
        {
            throw new NotImplementedException();
        }

        public void InitializeMip(string clientId, string applicatonName, string appVersion)
        {
            MIP.Initialize(MipComponent.File);

            ApplicationInfo appInfo = new()
            {
                ApplicationId = clientId,
                ApplicationName = applicatonName,
                ApplicationVersion = appVersion
            };

            MipConfiguration mipConfig = new MipConfiguration(appInfo, "mip_data", Microsoft.InformationProtection.LogLevel.Trace, false);

            MipContext mipContext = MIP.CreateMipContext(mipConfig);
        }
    }
}
