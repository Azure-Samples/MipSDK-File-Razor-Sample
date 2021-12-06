using Microsoft.Extensions.Configuration;
using Microsoft.InformationProtection;
using Microsoft.InformationProtection.File;

namespace MipSdkRazorSample.MipApi
{
    public class MipApi : IMipApi
    {
        private readonly AuthDelegateImpl   _authDelegate;
        private readonly IConfiguration     _configuration;
        private readonly string             _defaultEngineId;
        private readonly IFileProfile       _fileProfile;
        private List<IFileEngine>           _fileEngines;        
        private readonly MipConfigSettings  _mipConfigSettings;
        private readonly MipContext         _mipContext;
        
        public MipApi(IConfiguration configuration)
        {
            _fileEngines = new List<IFileEngine>();

            _configuration = configuration;
            //Fix this.

            ApplicationInfo appInfo = new ApplicationInfo()
            {
                ApplicationId = _configuration.GetSection("AzureAd").GetValue<string>("ClientId"),
                ApplicationName = _configuration.GetSection("MipConfig").GetValue<string>("ApplicationName"),
                ApplicationVersion = _configuration.GetSection("MipConfig").GetValue<string>("Version")
            };

            _defaultEngineId = _configuration.GetSection("AzureAd").GetValue<string>("ClientId");

            _authDelegate = new AuthDelegateImpl(_configuration);

            MIP.Initialize(MipComponent.File);

            MipConfiguration mipConfig = new(appInfo, "mip_data", Microsoft.InformationProtection.LogLevel.Trace, false);

            _mipContext = MIP.CreateMipContext(mipConfig);

            FileProfileSettings profileSettings = new FileProfileSettings(_mipContext,CacheStorageType.InMemory,new ConsentDelegateImpl());

            _fileProfile = MIP.LoadFileProfileAsync(profileSettings).Result;
        }

        public ContentLabel GetFileLabel(string userId, Stream inputStream)
        {
            IFileEngine engine = GetDelegatedEngine(userId);
                        
            IFileHandler handler = engine.CreateFileHandlerAsync(inputStream, "fileUpload.xlsx", true).Result;
            
            // TODO: Implement auth
            return handler.Label;                        
        }

        public bool IsLabeledOrProtected(Stream inputStream)
        {
            IFileStatus status = FileHandler.GetFileStatus(inputStream, "UploadedFile,", _mipContext);
            bool isLabeled = status.IsLabeled();
            bool isProtected = status.IsProtected();
                        
            return (isLabeled || isProtected);  
        }

        public int GetLabelSensitivityValue(string labelGuid)
        {
            IFileEngine engine = GetEngine(_defaultEngineId);

            return engine.GetLabelById(labelGuid).Sensitivity;
        }

        private IFileEngine GetEngine(string engineId)
        {
            IFileEngine engine;

            if (_fileEngines.Count() == 0 || _fileEngines.Where(e => e.Settings.EngineId == engineId) == null)
            {
                // Fix the engineId
                FileEngineSettings settings = new FileEngineSettings(engineId, _authDelegate, "", "en-US")
                {
                    Cloud = Cloud.Commercial
                };

                // Add async? 
                engine = _fileProfile.AddEngineAsync(settings).Result;
                _fileEngines.Add(engine);
            }
            else
            {
                engine = _fileEngines.Where(e => e.Settings.EngineId == engineId).First();
            }

            return engine;
        }

        private IFileEngine GetDelegatedEngine(string userId)
        {
            IFileEngine engine;

            if (_fileEngines.Count() == 0 || _fileEngines.Where(e => e.Settings.EngineId == userId) == null)
            {
                // Fix the engineId
                FileEngineSettings settings = new FileEngineSettings(userId, _authDelegate, "", "en-US")
                {
                    Cloud = Cloud.Commercial,
                    DelegatedUserEmail = userId
                };

                // Add async? 
                engine = _fileProfile.AddEngineAsync(settings).Result;
                _fileEngines.Add(engine);
            }
            else
            {
                engine = _fileEngines.Where(e => e.Settings.EngineId == userId).First();
            }

            return engine;
        }

    }
}
