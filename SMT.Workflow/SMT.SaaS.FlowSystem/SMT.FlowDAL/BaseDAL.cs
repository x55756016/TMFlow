﻿/*---------------------------------------------------------------------  
	 * 版　权：Copyright ©   2011    
	 * 文件名：BaseDAL.cs  
	 * 创建者： 提莫科技
	 * 创建日期：2011/12/12 9:52:20   
	 * CLR版本： 4.0.30319.239  
	 * 命名空间：SMT.FlowDAL 
	 * 模块名称：
	 * 描　　述： 	 
* ---------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Data.OracleClient;
using SMT.Foundation.Core;

namespace SMT.FlowDAL
{
    public class BaseFlowDAL
    {

        public static IDAO dao = DALFacoty.CreateDao(ConfigurationManager.ConnectionStrings["WorkFlowConnString"].ToString());

        /// <summary>
        /// DataTable 转换为List 集合
        /// </summary>
        /// <typeparam name="TResult">类型</typeparam>
        /// <param name="dt">DataTable</param>
        /// <returns></returns>
        public List<TResult> ToList<TResult>(DataTable dt) where TResult : class, new()
        {
            //创建一个属性的列表
            List<PropertyInfo> prlist = new List<PropertyInfo>();
            //获取TResult的类型实例  反射的入口
            Type t = typeof(TResult);
            //获得TResult 的所有的Public 属性 并找出TResult属性和DataTable的列名称相同的属性(PropertyInfo) 并加入到属性列表 
            Array.ForEach<PropertyInfo>(t.GetProperties(), p => { if (dt.Columns.IndexOf(p.Name) != -1) prlist.Add(p); });
            //创建返回的集合
            List<TResult> oblist = new List<TResult>();

            foreach (DataRow row in dt.Rows)
            {
                //创建TResult的实例
                TResult ob = new TResult();
                //找到对应的数据  并赋值
                prlist.ForEach(p => { if (row[p.Name] != DBNull.Value) p.SetValue(ob, row[p.Name], null); });
                //放入到返回的集合中.
                oblist.Add(ob);
            }
            return oblist;
        }
        #region 提莫科技新增
        /// <summary>
        /// 如果value值为null则返回""字符串,否则返回value值。
        /// </summary>
        /// <param name="value">value值</param>
        /// <returns></returns>
        public static object GetValue(object value)
        {
            return value == null ? DBNull.Value.ToString() : value;
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="conn">//OracleConnection</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="pageparm">数参</param>
        /// <returns></returns>
        public int ExecuteSQL(string sql, Parameter[] pageparm)
        {
            try
            {
                ParameterCollection pras = new ParameterCollection();
                foreach (var item in pageparm)
                {
                    pras.Add(item);
                }
                return dao.ExecuteNonQuery(sql,CommandType.Text, pras);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="conn">//OracleConnection</param>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public int ExecuteSQL(string sql)
        {
            try
            {
                return dao.ExecuteNonQuery(sql);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
        #endregion
    }
}
