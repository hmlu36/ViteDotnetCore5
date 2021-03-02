using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ViteDotnetCore5.Models.EFCore
{
    [Table("Role")]
    public partial class Role
    {
        public Role()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public int SeqNo { get; set; }
        [Key]
        public Guid Id { get; set; }
        [Column(TypeName = "numeric(1, 0)")]
        public decimal? Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? CreatedUser { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid? UpdatedUser { get; set; }
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        [Required]
        [StringLength(20)]
        public string Code { get; set; }

        [InverseProperty(nameof(UserRole.Role))]
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
