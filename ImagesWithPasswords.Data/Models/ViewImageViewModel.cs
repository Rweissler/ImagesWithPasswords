using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagesWithPasswords.Web.Models
{
    public class ViewImageViewModel
    {
        public bool PasswordCheck { get; set; }
        public string ImagePath { get; set; }
        public int ImageViewCount { get; set; }
        public int ImageId { get; set; }
        public string ErrorMessage { get; set; }
    }
}
