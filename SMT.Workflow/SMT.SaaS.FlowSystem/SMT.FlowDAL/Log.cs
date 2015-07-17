/*---------------------------------------------------------------------  
	 * 版　权：Copyright ©   2011    
	 * 文件名：Log.cs  
	 * 创建者： 提莫科技
	 * 创建日期：2011/12/12 9:54:36   
	 * CLR版本： 4.0.30319.239  
	 * 命名空间：SMT.FlowDAL 
	 * 模块名称：
	 * 描　　述： 	 
* ---------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SMT.Foundation.Log;

namespace SMT.FlowDAL
{
    public class Log
    {
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public static void WriteLog(string content)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("================================================================================");
            sb.AppendLine("发生时间：" + DateTime.Now.ToString());
            sb.AppendLine("日志内容:" + content);
            Tracer.Debug(sb.ToString());
        }
    }
}
