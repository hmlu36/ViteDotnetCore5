using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ViteDotnetCore5.Models.EFCore
{
    [Table("repairSetting")]
    public partial class RepairSetting
    {
        public RepairSetting()
        {
            RepairSettingDetails = new HashSet<RepairSettingDetail>();
        }

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
        [Column("repairSettingGroupId")]
        public Guid? RepairSettingGroupId { get; set; }
        [Column("code")]
        [StringLength(5)]
        public string Code { get; set; }
        [Column("description")]
        [StringLength(30)]
        public string Description { get; set; }
        [Column("content")]
        [StringLength(50)]
        public string Content { get; set; }
        [Column("periodType")]
        [StringLength(2)]
        public string PeriodType { get; set; }
        [Column("periodUnit", TypeName = "numeric(10, 0)")]
        public decimal? PeriodUnit { get; set; }

        [ForeignKey(nameof(RepairSettingGroupId))]
        [InverseProperty("RepairSettings")]
        public virtual RepairSettingGroup RepairSettingGroup { get; set; }
        [InverseProperty(nameof(RepairSettingDetail.RepairSetting))]
        public virtual ICollection<RepairSettingDetail> RepairSettingDetails { get; set; }
    }
}
