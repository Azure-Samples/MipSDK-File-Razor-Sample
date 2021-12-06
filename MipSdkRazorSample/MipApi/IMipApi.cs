using Microsoft.Extensions.Configuration;
using Microsoft.InformationProtection;
using Microsoft.InformationProtection.Exceptions;
using Microsoft.InformationProtection.File;


namespace MipSdkRazorSample.MipApi
{
    public interface IMipApi
    {        
        ContentLabel GetFileLabel(string userId, Stream inputStream);

        public bool IsLabeledOrProtected(Stream inputStream);

        public int GetLabelSensitivityValue(string labelGuid);
        
    }
}
