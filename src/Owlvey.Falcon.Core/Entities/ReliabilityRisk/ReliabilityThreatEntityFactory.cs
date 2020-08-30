using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class ReliabilityThreatEntity
    {
        public void Update(DateTime on, string ModifiedBy, string name, 
            string avatar,
            string reference, string description, string tags,
            decimal? ettd, decimal? ette, decimal? ettf,
            decimal? userImpact,
            decimal? ettfail) {
            this.ModifiedBy = ModifiedBy;
            this.Avatar = avatar;
            this.ModifiedOn = on;
            this.Name =  string.IsNullOrWhiteSpace(name) ? this.Name: name;
            this.Reference = string.IsNullOrWhiteSpace(reference) ? this.Reference: reference;
            this.Description = string.IsNullOrWhiteSpace(description) ? this.Description: description;
            this.Tags = tags;
            this.ETTD = ettd.HasValue ? ettd.Value: this.ETTD;
            this.ETTE = ette.HasValue ? ette.Value: this.ETTE;
            this.ETTF = ettf.HasValue ? ettf.Value: this.ETTF;
            this.UserImpact = userImpact.HasValue ? userImpact.Value: this.UserImpact;
            this.ETTFail = ettfail.HasValue ? ettfail.Value: this.ETTFail;
        }
        public static class Factory {
            public static ReliabilityThreatEntity Create(                
                DateTime on, string ModifiedBy, string name)
            {
                var entity = new ReliabilityThreatEntity
                {
                    CreatedBy = ModifiedBy,
                    CreatedOn = on,
                    ModifiedBy = ModifiedBy,
                    ModifiedOn = on ,
                    Name = name,
                    Description = "Insert description",
                    Reference = "https://en.wikipedia.org/wiki/Threat_(computer)",
                    Avatar = "data:image/jpeg;base64,/9j/4AAQSkZJRgABAQAAAQABAAD/2wCEAAkGBxATEBATEw8VExUXFRUXFRUXEA8PEBYSFRUXFhUYGBUYHTQgGBslGxUVITEhJS0rLi4uFx83ODMtNygtLisBCgoKBQUFDgUFDisZExkrKysrKysrKysrKysrKysrKysrKysrKysrKysrKysrKysrKysrKysrKysrKysrKysrK//AABEIAOEA4QMBIgACEQEDEQH/xAAcAAEBAAIDAQEAAAAAAAAAAAAAAQcIAgUGAwT/xABKEAABAwIEBAMFBQYBBg8AAAABAAIDETEEIWFxBQcSQQZRsRMiMoHxFCNCkaEIUmJygpJDMzSTosHRFRYkJTVUVWOUo7LC0+Hw/8QAFAEBAAAAAAAAAAAAAAAAAAAAAP/EABQRAQAAAAAAAAAAAAAAAAAAAAD/2gAMAwEAAhEDEQA/AM3pXyQ+Smg+iCk9ghPbupbIJbUoKTTdCaKW1KWzN0FrS6V7lTUpqUFB7lAVL7Jfb1QUGuyVrspfZNAgtfJCewU0H0S2QQUnsEJ/NS26W1KCk0StLqWzN18cbimQxSTSu6WRtc97syGsaKuNBmcgg+9e5QHuV8sNOyRjZGPa9jgHMc1wcwtOYIIyNV9L5myCgoDXZS+3ql9kFBrslfJS+QTQIKT2CE9gpbIJbdBSfzVquNtSqBS90FVUVQcSewUtkFSfK6lt0C26W1KW1KWzN0C2Zump+ian6JqUDUpfZL7Jfb1QL7eqX2S+yaBA0CaD6JoPolsggWyCW3S26W1KBbUpbM3S2ZumpQNSug8fn/mriRP/AFTEU+cTguy4xxSHDQSYid/RGwVJ6XPIGzRUla88xObuIxzX4fDNOHwzhR1ekzytNw4jJg/hb+Zsg6vl3zHxPDXiM1mwpNXwk5tqc3RE/Ce9LHQ5j2HHufcpq3CYJrB2fM4yH/RsoB/cVhdfv4C/DtxWHdiQXQiVhlAAcTGHAuFDeoQbbeDXYt+Bw78W/qmez2knuMjDev3mxhrRl0tLW+eRXdXyC+ODxcc0bJInh8b2hzXtNWlptRfbQIGg+iWyCWyCW3QLbpbUpbUpbM3QLZm6oHcqalUDuUFVUqqg4k03UtqVSaKWzN0C2Zump+ian6JqUAeZS+yX2S+3qgX29Uvsl9k0CBoE0H0TQfRLZBAtkEtult0tqUC2pS2ZulszdNSgalNSmpS+ZsgDPZeS8Tcu+F4wPfJhGsfQn2kX3EhdS56cnf1Ar1t9vVcZR1NcB5Eb5INIl+nhuE9rNHH7RkfW4ND5C5sTS40BcQDQV70yWw/gflDhYMLI3GMbNPMwtkN2wtP4Yj2eDT39Msr4L8Z+GpeH4yXDS59Ocb7CSJ1eh+lqEdiCOyDO3K3wvxrhrjBM+CXCuqaNmkL4Xn8TAWCoJu2o8xnUHJtsgsfclPFZxvDxG91Z8PSN5Jq50dPunnzNAW7sJ7rINt0C26W1KW1KWzN0C2ZumpTUpqUDUqjPNS+ZsqM9vVByqiIg4nLNTUqnzKmpQNSl9kvsl9vVAvt6pfZL7JoEDQJoPomg+iWyCBbIJbdLbpbUoFtSlszdLZm6alA1KalNSl8zZAvmbJfb1X4uM8Vhw0EmInkEcUYq5x79gAO5JoAO5IXjPAfNTC8RldA5v2aXqd7JjnAiWOp6Q13Z4F2/lXOgZAvsl8gsWcyOb8eCkfhcJG2aduUj3V9hG7u2gNXuHcZAWqSCBiLH8zuMykl2PkYK5CMRwAaDoANN6oNsNAsIftInDEYGkjTiGmQFgIL/AGLgDVwuB1DKvm7VYnxHiriMleviGJcDcHEzEH5dVF+ThvDMTipeiGGSZ5NmNdI7M3J7DUoMj/s64p44nOwfC/DOLh2qySPpJ/uI+a2LtqVjrlBy/fw2KSbEUOJmABYCHNijGfR1C7iaFxGXuilqnItszdAtmbpqU1KalA1KXzNkvmbJfb1QL7eqta7KX29Va+SDkilFUHEjuVL7KkKX29UC+3ql9kvsmgQNAmg+iaD6ITTIf/tSgWyC4zStY0uc4NAzLnENaB5kmwWLvHfOTDYUvhwYbiZhUOkJP2ZjtxnIdG0GuVFg3xH4qx2Of1YnEvkFahlemFv8sY90XvfVBsdxnmxwfDdQ+1e3ePwwMM1dn5M/1l5DGc/oAT7Hh8j9ZJmRfo1rvVYGRBmoftASV/6Mb/4p3/xr9uC5/wARI9tw57R5xztk/RzR6rBCINouC83uD4ggOndh3GzZ4/Zj5vaSwfMr2rcfC6IzCZhhDS4yB7TH0gVLuoGlAFpQv0QY6ZjJI2SvayQASMa97WPANR1tBo7MC6D2/NjmE7iU/s4iW4SIn2bc2mR1vauH/pBsD5krwUby0hwJBBBBBIIIzBBFiuKIPvhoXzSsb1DrkeG9T3UHU91KucbCpzJWxnAeSnC4o2jEB+KloC4mSSKMHv0tjINNyStbFmnkf49xjpmcPkY/ERkExyfE+BrR+NxvEMgK5ioArkEGUcNy+4PHQN4bhz/PGJvz66r0GFwscTQ2ONjB2axjWN/ICi+ttSlszdAtmbpqU1KalA1KXzNkvmbJfb1QL7eqX29Uvt6pfIIF8grXsFNAroEFoqoqg4kV2Uvsqc9lNAgaBNB9E0H0S2QQcZpWsa4lwaGguc5xDWtaBUucTYUWuvNLmm/Fl+FwbizC5tfJm2TEdjq2P+Hv38l2HPHx8ZJH8Ow0n3bDTEvB+OQH/JV/dab+Zy7Z4cQEREBERAREQEREBERB+nh2AlxEscMMZkke4NYxtyT6DuScgBUrarlz4Ji4XhQwUfPJQzSAfE7s1v8AA2uXnme9Fgzkt4mw+C4h9/G3pmaImzEe9C4nLPs1xoHHY2Brs/bM3QLZm6alNSmpQNSl8zZL5myX29UC+3ql9vVL7eqXyCBfIJoE0CaBA0Coyy7qWyF1Rlug5Ioqg4nyU0H0VJ7BS2QQLZBeR5peKv8Ag7h0kjD99IfZQ+ftHA1f/S0E+VQB3XrrbrWzn5x0z8T+zg+5hmBuhlkAfIfy6G/0lBjRziSSSSTmSTUkm5JUREBERAREQEREBERAREQFs7yU8WfbcAI5X9U+GpG8k1c6P/CfqaAtJ7lte61iXt+TnHvsnFsPV1I5j7CTy+8/yZ+UgZn5VQbUalL5myXzKX29UC+3ql9vVL7eqXyCBfIJoE0CaBA0CWyF0tkLpbUoFtSqBS91LZm6oHcoKqoqg4k9gpbdUn81LalBHvDQSfIk7DNaW8Zx5xGJxE5vLLJIf63F3+1be+L5zFw7Hyd24adw3ETiFpsgIiICIiAiIgIiICIiAiIgLnFIWua5po4EEEXBBqCPmuCIN0+C44YjDYecfDLFHIP62B3+1fsvt6rynKmcycG4efKLp/0b3M/9q9XfIIF8gmgTQJoEDQJbIXS2QultSgW1KWzN0tmbpqUDUqgdypqVRnmgtVVKqoOJNFLZm6pyzU1KDpPHMZdwviI7nCYgAamJ2S07W7OMw4kjkY6z2Ob8nAg+q0qxELmPexwo5ri1w8nNNCPzCD5oiICIiAiIgIiICIiD13gDwDiuJyO9n91AwgSzuBLQb9LR+N9M6dsqkVFfe/8AEfwqJPsp4q8T1p1+3jp11p0l3s/Zg1y6a1X08S4qTCeEuHjCksbN7NszmV6qSNkfJVwtV4Ddsu6wgg9fzC8BYjhcreo+1gefupw2gJv0PH4X0z8iMxYgeQWauV/HouJ4KXguOfUlh+zvJrJ0tFQ1pP446dQ82giwWKfEvA5sFipsNM2j2OoD+F7fwvb/AAkUP/2g2b5QxEcE4eLe48/J0r3D9CvYaBdT4RwJgwGCgPxR4eJrv5wwdX61XZzzMY0lzmtAu5zg1o3JQc9AlshdfHDYuJ4rHIyTVr2vHzIK81xDmRweCX2UmPZ11oelskrGurSjnsaWtob1OXdB6u2pS2ZuvlhMTHJG2SORsjXirXtcHscDbpIyIX11KBqU1KalL5lAvmVRnspfb1VrXb1QckREHE+ZU1KpHcqXzNkAZ59lqrzh4L9l4viqCjJj7dm0tS//AMwPHyC2qvt6rF/PvwycTgW4qNtZMLUu83QO+P8AtIDtupBrgiIgIiICIiAiIgIiIMx8oeMwY3Bz8Exh917XHDuqK5nrc1tfxNd943+ryzxj4n4DNgcVLhph7zDk6hDXsObXt0I/3dl+DCYl8UjJI3Fj2ODmOGRa5pqCPms38ew0fiLhDMXCwDH4UdMjBkXUFXsA7h3xsvnVvcoMI4LFyQyMljeWSMcHMcMi1wNQVsJhGcL49hcLjsQRDJhP86oWsAa0Fxa8n/CJ98HsOoXqtdF94MXI1krGyOayQNEjQ4hrw1we0OHejgCEGxk3PDhI9sGNncWg9BMIayVwBoGnqq2p7uAWBfFHifF4+Z0uIlLsz0sqRFGD+FjbDe57krpkQc4pXNJLXFuVDQkZeWS4IiD3PK/mBLw3ENa9zn4R5pLHm7oJ/wARg7OHcD4hrQjaHCYhkjGSseHse0OY4GrSxwqCD3qCtJVnP9nvxYXe04dK6vSDJh6mwr95H+Z6h/WgzZfMpfb1S+3ql9vVAvt6q18lL5CytewQcqIpRVBxIUvt6qkV2UvsgX2UkYHAtIBaQQ4EVBByIorfIJoEGqnNLwU7huMcGgnDSEugdmaDvGT+82vzFD5rxa3J8U+HcPjsM/DTtq12YcMnxvHwvaezh+uYORK1W8a+EcTw3EmGdtWmpilAPs5WDuPIjKrbjYgkPPoiICIiAiIgIiIC9Ty68XScNxrJszE+jJ2A/FETcD95tx8x3K8su38L+G8Vj8Q2DDx9TjTqdaONlc3vd2aPzNhU5IPd86PCEcb2cSwlH4bE0c/ozYyR4qHClmvv/NXzAWLVt3wfwbh4eGDhr+qaMsc15cTVxeS5zm/uAONQBag75rWHxr4Zl4fjJcNJmB70b+0kJJ6HDXKhHYghB0SIiAiIgLvfAvEjhuJYGatOmdgd5dDz0P8A9Vzl0SIN377eqXyFl0vgvjP2zh+ExAIJfE3rp2lb7sg/uDl3WgQNAroFNArbJBVVFUHEiuyl8gqfJTQIGg+iWyCWyCW3QLbrruP8Dw2LgdBiYhKx3Y5ODuzmuu0jzC7G2pS2Zug1p8ecocZgy6TDB2Kw9/dFcQwfxsHxD+JvzAWNlu/qV5PxRy54Zjup82HEch/xoqRS183UFHn+YFBqYizLxrkJiBU4TGRvbX4ZmuieB/M0EOPyavI4vlNxuMn/AJF1j95k0Dwdh1V/RB4hF6gcu+Mf9mz/ANg/3r9mD5U8bkP+Yubq+WCMD5F1f0QeLRZf4NyFxbyDicXFEMqiMPnfTuM+kA/msmeGeV/C8EWubB7eUUPtJqSkEZgtbTpaa2IFdUGEPA3KzHY8se9pw+HNCZXtIe5v/dsu7c0GpstgOG8N4dwbBO6S2CJoBkleave61Xuu5xOQaPOgHZejtl3Wu/7Q3FpnY+LDFxEUcTXhtmukf1VeR3yAaPKh8yg7LxVz1kJczAQBrbe3mHU86tjBo3TqrsFizxB4kxmOe1+KxDpi2vTUNa1oNK0a0ACtBYdl1KICIiAiIgIiIM1/s7eJiHT8Pe7J1ZoK/vAUlaNx0uA/hee6zpoFpdwPikmFxMOJjNHxPa9vkaHNp0IqDoStxeDcTjxGHhniNWSsa9vnRwrQ6ix1CD9lshdUZbqW1Koy3QVVRVBxJ7BS2QVJ7BS26BbdLalLalLZm6BbM3TUpqU1KBqUvmbJfM2S+3qgX29Uvt6pfb1S+QQL5BNAmgTQIGgS2QulshdLalAtqV4Dmny3bxNscscoixMbS1pcCY5GVJDHEZtoSSHCtzka5e/tmbpqUGtA5J8Y6qdMAH7xn939BX9F3uG5A4gtrJxCJh7hsMkg/MuHos9alL5lBr5xHkLjmtrDi4ZdHCSB3yuPzIWOOP8Ah3GYJ4ZicO+Jxr0kgFjgL9Lx7ru1j3W5V9vVfh41wiDFwvgniEkThmD59i03aR2IzQaXIsteMuSWJh6pMC84mMZ+zd0txLdiPdk72ofIFYpxED2OcyRjmPaaOa5pa8HyIOYQfNEWS+XPKfEY7onxPVBhjm0UpPKO3SD8LD+8b9ga1AeS8IeEsZxGb2WHjqB8cjqiGMebnefkBmfJbSeCfDTeHYKPCtldL0lxLnAD3nGrulo+FtamlTc5ldjwfhWHwkLIMPE2ONtmtHfuSbk+ZOZX7LboFt1QO5UtmVQO5QVVEQcSfzUtqVyKgFM+6CWzN01KoHcoB3KCalL5mytK3SldvVBL7eqX29VTnsh8kEvkE0Cp8gmgQTQJbIXVta6UpqUEtqUtmbqgUz7oB3KCalNSqB3KUrdBL5lL7eqtK7Ic9vVBL7eqXyFlT5IfIIJoF57xb4KwHEGdOIgBeBRszPcnZs/uP4XVGi9FoEtZBjHwhyaweEndNNJ9rcHVha5gZGwdi9tT1uH5d6VpTJtt1aU1KAU3QS26WzKoHcoB3KCan6KjPMpSuZS+yC1VREERVEEQqogFERAUCqIIEVRAUVRBEVRBChVRAREQAoFUQRFUQRFUQRVEQQqoiCIiIP/Z",
                    Tags = "#reliability #threat",
                    ETTD = 10, 
                    ETTE = 10,
                    ETTF= 20,
                    ETTFail = 100,
                    UserImpact = 0.1m
                };                
                return entity;
            }
        }
    }
}
