using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ViteDotnetCore5.Models.EFCore
{
    [Table("repairSettingGroup")]
    public partial class RepairSettingGroup
    {
        public RepairSettingGroup()
        {
            RepairSettings = new HashSet<RepairSetting>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("description")]
        [StringLength(20)]
        public string Description { get; set; }

        [InverseProperty(nameof(RepairSetting.RepairSettingGroup))]
        public virtual ICollection<RepairSetting> RepairSettings { get; set; }
    }
}
