using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ViteDotnetCore5.Models.EFCore
{
    [Table("UserToken")]
    public partial class UserToken
    {
        public int SeqNo { get; set; }
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [StringLength(400)]
        public string AccessToken { get; set; }
        public DateTime? ExpireAt { get; set; }
        [Column("IPAddress")]
        [StringLength(30)]
        public string Ipaddress { get; set; }
        public Guid? RefreshToken { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("UserTokens")]
        public virtual User User { get; set; }
    }
}
