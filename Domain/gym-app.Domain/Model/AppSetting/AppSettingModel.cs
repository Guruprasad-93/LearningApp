using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gym_app.Domain.Model.AppSetting
{
    public class AppSettingModel
    {
        [Required]
        public long? SettingGroupId { get; set; }
       // [XssValidation]
        public string SettingGroupCode { get; set; }
        // [XssValidation]
        public string SettingGroupName { get; set; }
        // [XssValidation]
        public string AppSettingKey { get; set; }
        // [XssValidation]
        public string AppSettingKeyName { get; set; }

        public long? ParentAppSettingKeyID { get; set; }
        public long? EntityID { get; set; }
        public EnumAppSettingEntityType EntityType { get; set; }
        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public long AppSettingKeyID { get; set; }

        // [XssValidation]
        public string Value { get; set; }

        // [XssValidation]
        public string DefaultValue { get; set; }
        public EnumAppSettingValueType EntityValue { get; set; }

        public long? ProjectID { get; set; }

        public List<AppSettingModel>  children { get; set; }
    }
}
