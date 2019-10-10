using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Owlvey.Falcon.Core.Exceptions;

namespace Owlvey.Falcon.Core.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public int? Id { get; set; }

        [Required]
        public DateTime? CreatedOn { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public DateTime? ModifiedOn { get; set; }
        [Required]
        public string ModifiedBy { get; set;  }
        [Required]
        public bool Deleted { get; set; }
        public virtual void Delete(DateTime on, string modifiedBy) {
            this.Deleted = true;
        }        
       
        public virtual void Validate() {
            var context = new ValidationContext(this, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(this, context, results);
            if (!isValid)
            {
                var msg = results.Select(c => c.ErrorMessage).ToList();
                throw new InvalidStateException(String.Join(", ", msg));
            }
        }

        

    }    
}
