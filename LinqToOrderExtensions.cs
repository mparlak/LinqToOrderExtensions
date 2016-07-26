using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EPlatform.CRM.Business.Utils
{
    public static class LinqToOrderExtensions
    {
        public static List<T> OrderBy<T>(this List<T> data,string sortDirection, string sortExpression)
        {

            List<T> data_sorted = new List<T>();

            if (sortDirection == "asc")
            {
                data_sorted = (from n in data
                               orderby GetDynamicSortProperty(n, sortExpression) ascending
                               select n).ToList();
            }
            else if (sortDirection == "desc")
            {
                data_sorted = (from n in data
                               orderby GetDynamicSortProperty(n, sortExpression) descending
                               select n).ToList();

            }

            return data_sorted;

        }

        public static object GetDynamicSortProperty(object item, string propName)
        {
            var value = new object();
            if (item.GetType().GetProperty(propName) != null)
            {
                value = item.GetType().GetProperty(propName).GetValue(item, null);
            }
            else
            {
                value = GetSubDynamicSortProperty(item, propName);
            }
            return value;
        }

        private static object GetSubDynamicSortProperty(object item, string propName)
        {
            var value = new object();
            foreach (var x in item.GetType().GetProperties())
            {
                if (x.PropertyType.IsClass && x.PropertyType.GetProperty(propName) != null)
                {
                    var info = item.GetType().GetProperty(x.Name);
                    var i = info.GetValue(item, null);
                    var info2 = i.GetType().GetProperty(propName);
                    value = info2.GetValue(i, null);
                }
            }
            return value;
        }
    }
}
