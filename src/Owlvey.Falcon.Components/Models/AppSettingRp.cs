using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Models
{
    public class AppSettingBaseRp
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class AppSettingGetRp : AppSettingBaseRp {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class AppSettingGetListRp : AppSettingBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class AppSettingPostRp {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class AppSettingPutRp
    {
        public string Value { get; set; }
    }
}
