using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerV2.Application
{
    public static class EnumHelper
    {
        public static int GetId<TEnum>(string enumName) where TEnum :struct, Enum
        {
            if(Enum.TryParse<TEnum>(enumName, out var enumValue))
            {
                return Convert.ToInt32(enumValue);
            }
            throw new Exception($"{enumName} is not supported.");
        }
        public static int GetId<TEnum>(TEnum enumValue) where TEnum : struct, Enum 
        {
            return Convert.ToInt32(enumValue);
        }
    }
}
