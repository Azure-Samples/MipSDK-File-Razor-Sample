using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.InformationProtection;
using MipSdkRazorSample.Models;
using MipSdkRazorSample.Services;
using System.Security.Claims;

namespace MipSdkRazorSample.Pages.DataImport
{
    public class IndexModel : PageModel
    {
        private readonly MipSdkRazorSample.Data.MipSdkRazorSampleContext _context;
        private readonly IMipService _mipApi;
        private readonly string? _userId;



        [BindProperty]
        public IFormFile? Upload { get; set; }

        public IndexModel(MipSdkRazorSample.Data.MipSdkRazorSampleContext context)
        {
            _context = context;

            _mipApi = _context.GetService<IMipService>();
            _userId = _context.GetService<IHttpContextAccessor>().HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Subject.Name;
        }

        public DataPolicy DataPolicy { get; set; }
        public IList<MipLabel> MipLabels { get; set; }  
        public string? Result { get; set; }

        public async Task OnPostAsync()
        {
            MemoryStream uploadStream = new();

            DataPolicy = _context.DataPolicy.First(d => d.PolicyName == "Download Policy");

            if (Upload != null)
            {
                await Upload.CopyToAsync(uploadStream);
            }

            if (_mipApi != null)
            {
                uploadStream.Position = 0;

                if (_mipApi != null)
                {
                    if (_userId != null)
                    {
                        ContentLabel label;

                        try
                        {
                            label = _mipApi.GetFileLabel(_userId, uploadStream);

                            if(_mipApi.IsLabeledOrProtected(uploadStream) == false)
                            {
                                Result = String.Format("Uploaded file isn't labeled.");

                                // Do Processing

                            }

                            else if (_mipApi.GetLabelSensitivityValue(label.Label.Id) <= _mipApi.GetLabelSensitivityValue(DataPolicy.MinLabelIdForAction))
                            {
                                // Do Processing


                                if (label.Label.Parent.Id == null)
                                    Result = String.Format("Successfully to uploaded file with label: {0}", label.Label.Name);
                                else
                                    Result = String.Format("Successfully to uploaded file with label: {0} - {1}", label.Label.Parent.Name, label.Label.Name);

                            }
                            else
                            {
                                // Write Error


                                if (label.Label.Parent.Id == null)
                                    Result = String.Format("Failed to upload file. Service doesn't permit {0}", label.Label.Name);
                                else
                                    Result = String.Format("Failed to upload file. Service doesn't permit {0} - {1}", label.Label.Parent.Name, label.Label.Name);
                            }
                        }
                                                 
                        catch(Microsoft.InformationProtection.Exceptions.NoAuthTokenException)
                        {

                        }

                        catch (Microsoft.InformationProtection.Exceptions.NoPermissionsException)
                        {

                        }

                        catch (Microsoft.InformationProtection.Exceptions.AccessDeniedException)
                        {
                            Result = "User doesn't have rights to uploaded file.";
                        }
                    }
                }
            }
        }         
    }
}