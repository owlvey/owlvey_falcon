using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Owlvey.Falcon.Core.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public int? Id { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public DateTime? ModifiedOn { get; set; }
        [Required]
        public string ModifiedBy { get; set;  }

        public virtual void Create(string user, DateTime on)
        {
            this.CreatedBy = user;
            this.CreatedOn = on;
            this.ModifiedBy = user;
            this.ModifiedOn = on;
            this.Validate();
        }

        public virtual void Delete() {

        }
                
        public virtual void Validate() {
            var context = new ValidationContext(this, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(this, context, results);
            if (!isValid)
            {
                var msg = results.Select(c => c.ErrorMessage).ToList();
                throw new ApplicationException(String.Join(", ", msg));
            }
        }

    }    
}
