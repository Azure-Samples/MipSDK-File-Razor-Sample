using System.ComponentModel.DataAnnotations;

namespace MipSdkRazorSample.Models
{
    public class DataPolicy
    {
        public enum PolicyDirection
        {
            Upload,
            Download
        }

        public int ID { get; set; }
        
        [Display(Name = "Policy Name")]
        public string PolicyName { get; set; } = string.Empty;

        public PolicyDirection Direction { get;set; }
        
        [Display(Name = "Label Id for Action")]        
        public string MinLabelIdForAction { get; set; } = string.Empty;

    }
}
