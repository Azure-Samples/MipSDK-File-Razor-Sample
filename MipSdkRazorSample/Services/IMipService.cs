using Microsoft.Extensions.Configuration;
using Microsoft.InformationProtection;
using Microsoft.InformationProtection.Exceptions;
using Microsoft.InformationProtection.File;
using MipSdkRazorSample.Models;

namespace MipSdkRazorSample.Services
{
    public interface IMipService
    {        
        ContentLabel GetFileLabel(string userId, Stream inputStream);
        public IList<MipLabel> GetMipLabels(string userId);
        public int GetLabelSensitivityValue(string labelGuid);
        public bool IsLabeledOrProtected(Stream inputStream);
        public bool IsProtected(Stream inputStream);
        public MemoryStream ApplyMipLabel(Stream inputStream, string labelId);
        public Stream GetTemporaryDecryptedStream(Stream inputStream, string userId);          
    }
}
