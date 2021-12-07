using Microsoft.InformationProtection;
using Microsoft.InformationProtection.Exceptions;

namespace MipSdkRazorSample.Services
{
    public class ConsentDelegateImpl : IConsentDelegate
    {
        public Consent GetUserConsent(string url)
        {
            return Consent.Accept;
        }
    }
}
