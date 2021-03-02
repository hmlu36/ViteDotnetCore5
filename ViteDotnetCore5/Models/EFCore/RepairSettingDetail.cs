using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ViteDotnetCore5.Models.EFCore
{
    [Table("repairSettingDetail")]
    public partial class RepairSettingDetail
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("createdAt")]
        public DateTime CreatedAt { get; set; }
        [Column("createdUser")]
        public Guid? CreatedUser { get; set; }
        [Column("updatedAt")]
        public DateTime UpdatedAt { get; set; }
        [Column("updatedUser")]
        public Guid? UpdatedUser { get; set; }
        [Column("repairSettingId")]
        public Guid? RepairSettingId { get; set; }
        [Column("changeType")]
        [StringLength(2)]
        public string ChangeType { get; set; }
        [Column("changeUnit", TypeName = "numeric(10, 0)")]
        public decimal? ChangeUnit { get; set; }

        [ForeignKey(nameof(RepairSettingId))]
        [InverseProperty("RepairSettingDetails")]
        public virtual RepairSetting RepairSetting { get; set; }
    }
}
