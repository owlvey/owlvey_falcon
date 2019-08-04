
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    /// <summary>
    /// This class is responsible for storing the core settings.
    /// </summary>
    public partial class AppSettingEntity : BaseEntity
    {

        /// <summary>
        /// Key Setting
        /// </summary>
        [Required]
        public string Key { get; set; }

        /// <summary>
        /// Value Setting
        /// </summary>
        [Required]
        public string Value { get; set; }

        /// <summary>
        /// If it is read-only, it is a system variable
        /// </summary>
        public bool IsReadOnly { get; set; }

        
    }
}
