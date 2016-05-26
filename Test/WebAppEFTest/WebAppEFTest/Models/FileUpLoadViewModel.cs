using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewModel;

namespace WebAppEFTest.Models
{
    public class FileUpLoadViewModel : BaseViewModel
    {
        public HttpPostedFileBase fileUpload { get; set; }
    }
}