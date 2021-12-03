using Microsoft.InformationProtection;
using Microsoft.InformationProtection.Exceptions;

namespace MipSdkRazorSample.MipApi
{
    public class ConsentDelegateImpl : IConsentDelegate
    {
        public Consent GetUserConsent(string url)
        {
            return Consent.Accept;
        }
    }
}
