using Microsoft.InformationProtection;
using Microsoft.InformationProtection.Exceptions;

namespace MipSdkRazorSample.MipApi
{
    public class AuthDelegateImpl : IAuthDelegate
    {
        public string AcquireToken(Identity identity, string authority, string resource, string claims)
        {
            throw new NotImplementedException();
        }
    }
}
