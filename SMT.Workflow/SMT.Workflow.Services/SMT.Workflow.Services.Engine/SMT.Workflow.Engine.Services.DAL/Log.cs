﻿/*---------------------------------------------------------------------  
	 * 版　权：Copyright ©   2011    
	 * 文件名：Log.cs  
	 * 创建者： 提莫科技
	 * 创建日期：2011/12/19 16:54:30   
	 * CLR版本： 4.0.30319.239  
	 * 命名空间：SMT.Workflow.Engine.Services.DAL 
	 * 模块名称：
	 * 描　　述： 	 
* ---------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using SMT.Foundation.Log;

namespace SMT.Workflow.Engine.Services.DAL
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
                Tracer.Debug(content);       
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="obj">当前发生的对象（如：类对象this）</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="content">内容</param>
        /// <param name="e">Exception</param>
        /// <returns></returns>
        public static void WriteLog(object obj, string methodName, string content, Exception e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("================================================================================");
            sb.AppendLine("时间:" + DateTime.Now.ToString());
            sb.AppendLine("DLL:" + obj.GetType().Module.Name);
            sb.AppendLine("类名:" + obj.GetType().FullName);
            sb.AppendLine("方法:" + methodName);
            sb.AppendLine("内容:" + content);
            if (e != null)
            {
                sb.AppendLine("异常:" + (e.InnerException != null ? e.InnerException.Message : e.Message));
            }
            Tracer.Debug(sb.ToString());
        }
    }
}
