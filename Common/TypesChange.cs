using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbtocpp.Common
{
    public static class TypesChange
    {
        public static String dbtocpp(String oldType)
        {
            String newType = "";
            switch(oldType)
            {
                case "bigint":
                    newType="long";
                    break;
                case "timestamp":
                case "year":
                case "tinyint":
                case "smallint":
                case "mediumint":
                    newType = "short";
                    break;
                case "varchar":
                case "tinytext":
                case "mediumtext":
                case "longtext":
                case "text":
                case "datetime":
                case "time":
                case "date":
                    newType="std::string";
                    break;
                default:
                    newType = oldType;
                    break;
            }
            return newType;
        }
    }
}
