using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class IncidentEntity : BaseEntity
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Url { get; set; }        
        public string Tags { get; set; }        
        [Required]
        public int TTD { get; set; }
        [Required]
        public int TTE { get; set; }
        [Required]
        public int TTF { get; set; }


        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        [Required]
        public int Affected { get; set; }

        public virtual ICollection<IncidentMapEntity> FeatureMaps { get; set; }

        [NotMapped]
        public int TTM { get {
                return this.TTD + this.TTE + this.TTF;
            } }

        
        public virtual ProductEntity Product { get; set; }

        public int ProductId { get; set; }


        public void Update(string title,  string description, string modifiedBy, DateTime on,
            DateTime? start = null,
            int? ttd = null, int? tte =null, int? ttf=null, string url = null) {

            this.ModifiedBy = modifiedBy;
            this.ModifiedOn = on;
            this.Title = title ?? this.Title;
            this.Description = description ?? this.Description;
            this.Start = start ?? this.Start;
            this.TTD = ttd?? this.TTD;
            this.TTE = tte ?? this.TTE;
            this.TTF = ttf ?? this.TTF;
            this.End = this.Start.AddMinutes(this.TTM);
            this.Url = url ?? this.Url;
            this.Validate();
        }

    }
}
