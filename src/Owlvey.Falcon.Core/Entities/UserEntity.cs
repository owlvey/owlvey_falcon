﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Owlvey.Falcon.Core.Entities
{
    public class UserEntity: BaseEntity
    {
        public string Email { get; set; }        
        

        internal void Update(string email) {
            this.Email = email;
        }
        
    }
}
