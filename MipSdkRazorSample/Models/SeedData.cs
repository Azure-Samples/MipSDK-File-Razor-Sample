using Microsoft.EntityFrameworkCore;
using MipSdkRazorSample.Data;

namespace MipSdkRazorSample.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MipSdkRazorSampleContext(serviceProvider.GetRequiredService<DbContextOptions<MipSdkRazorSampleContext>>()))
            {
                if (context == null || context.DataPolicy == null)
                {
                    throw new ArgumentNullException("Null MipSdkRazorSampleContext");
                }

                if(context.DataPolicy.Any())
                {
                    return;
                }

                context.DataPolicy.AddRange(
                    new DataPolicy
                    {
                        Direction = DataPolicy.PolicyDirection.Download,
                        PolicyName = "Download Policy",
                        MinLabelIdForAction = "000"
                    },
                    new DataPolicy
                    {
                        Direction = DataPolicy.PolicyDirection.Upload,
                        PolicyName = "Upload Policy",
                        MinLabelIdForAction = "000"
                    });
                context.SaveChanges();
            }
        }
    }
}
