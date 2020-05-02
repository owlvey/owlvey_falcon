using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Owlvey.Falcon.Core.Entities.Source
{
    public class SourceCollection : Collection<SourceEntity>,        
        IEnumerable<SourceEntity>,
        ICollection<SourceEntity>
    {
        public SourceCollection() { }
        public SourceCollection(IList<SourceEntity> items) : base(items) { 

        }

    }
}
