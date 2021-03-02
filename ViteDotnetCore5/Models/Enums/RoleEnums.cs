using System.ComponentModel;

namespace ViteDotnetCore5.Models.Enums {

    public enum RoleEnums {

        [Description("管理者")]
        Admin,
       
        [Description("指揮官")]
        Commandar,

        [Description("操作手")]
        Operator,
        
        [Description("檢視人員")]
        Viewer,

        [Description("一般使用者")]
        User,
    }
}
