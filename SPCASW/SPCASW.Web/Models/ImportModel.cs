using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web;
using System.Linq;
using System.Configuration;

namespace SPCASW.Web.Models
{
    public class ImportFileModel
    {
        
        [ValidateMaxFileSize()]
        [FileTypes("xml")]
        [Display(Name = "File Name")]
        public HttpPostedFileBase File { get; set; }
        
    }

   

    public class ValidateMaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxSize;

        public ValidateMaxFileSizeAttribute()
        {

            //set maxfilesize from webconfig file
            string MaxFileSizeStr = ConfigurationManager.AppSettings["ImportFileMaxSize"];
            if (!string.IsNullOrEmpty(MaxFileSizeStr))
            {
                int MaxFileSize;
                if(int.TryParse(MaxFileSizeStr, out MaxFileSize))
                {
                    _maxSize = MaxFileSize;
                }
            }
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            return _maxSize > (value as HttpPostedFileWrapper).ContentLength;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("The file size should not exceed {0}", _maxSize);
        }
    }

    public class FileTypesAttribute : ValidationAttribute
    {
        private readonly List<string> _types;

        public FileTypesAttribute(string types)
        {
            _types = types.Split(',').ToList();
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            var fileExt = System.IO.Path.GetExtension((value as System.Web.HttpPostedFileBase).FileName).Substring(1);
            return _types.Contains(fileExt);
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("Invalid file type. Only the following types {0} are supported.", String.Join(", ", _types));
        }
    }

    //public class ValidateMaxFileSizeAttribute : ValidationAttribute
    //{
    //    private readonly List<string> _types;

    //    public ValidateMaxFileSizeAttribute(string types)
    //    {
    //        _types = types.Split(',').ToList();
    //    }

    //    public override bool IsValid(object value)
    //    {
    //        if (value == null) return true;

    //        var fileExt = System.IO.Path.GetExtension((value as HttpPostedFile).FileName).Substring(1);
    //        return _types.Contains(fileExt);
    //    }

    //    public override string FormatErrorMessage(string name)
    //    {
    //        return string.Format("Invalid file type. Only the following types {0} are supported.", String.Join(", ", _types));
    //    }
    //}
}
