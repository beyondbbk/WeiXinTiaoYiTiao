using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowAndroidModel
{
    public class Config
    {
        ///<summary> 
        ///更新配置 
        ///</summary> 
        public static void UpdateConnectionStringsConfig(string newRate)
        {
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //写入<add>元素的Value
            config.AppSettings.Settings["rate"].Value = newRate;
            config.Save(ConfigurationSaveMode.Modified);
        }

    }
}
