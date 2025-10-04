using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gym_app.Domain.Model.AppSetting
{
    public enum EnumAppSettingEntityType
    {
        None = 0,
        User = 1,
        Role = 2,
    }
    public enum EnumAppSettingValueType
    {
        String = 1,
        Integer = 2,
        Float = 3,
        XML = 4,
        DateTime = 5,
        Bit = 6,
        Int = 7,
        BigInt = 8,
        None = 9,
    }
}
