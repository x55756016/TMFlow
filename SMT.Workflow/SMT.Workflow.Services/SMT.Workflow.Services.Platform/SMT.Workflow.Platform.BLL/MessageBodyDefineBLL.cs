﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMT.Workflow.Platform.DAL;
using SMT.Workflow.Common.Model.FlowEngine;
using System.Data.OracleClient;
using SMT.Foundation.Core;

namespace SMT.Workflow.Platform.BLL
{
    public class MessageBodyDefineBLL
    {

        MessageBodyDefineDAL dal = new MessageBodyDefineDAL();

        /// <summary>
        /// 获取默认消息的分页列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="strFilter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="strOrderBy"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public List<T_WF_MESSAGEBODYDEFINE> GetDefaultMessgeList(int pageSize, int pageIndex, string strFilter, string strOrderBy, ref int pageCount)
        {
            try
            {
                if (string.IsNullOrEmpty(strOrderBy))
                {
                    strOrderBy = "CREATEDATE DESC";
                }
                if (pageSize < 5)
                {
                    pageSize = 15;
                }
                return dal.GetDefaultMessgeList(pageSize, pageIndex, strFilter, strOrderBy, ref  pageCount);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public bool ExistsDefaultMessage(T_WF_MESSAGEBODYDEFINE entity)
        {
            try
            {
                string sql = "select DEFINEID from T_WF_MESSAGEBODYDEFINE where SYSTEMCODE='" + entity.SYSTEMCODE + "' and MODELCODE='" + entity.MODELCODE + "' and COMPANYID='" + entity.COMPANYID + "' and DEFINEID!='" + entity.DEFINEID + "'";
                if (dal.GetDataTable(sql).Rows.Count > 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 新增默认消息
        /// </summary>
        /// <param name="entity"></param>
        public void AddDefaultMessage(T_WF_MESSAGEBODYDEFINE entity)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(entity.DEFINEID))
                {
                    entity.DEFINEID = Guid.NewGuid().ToString();
                }               
                string insSql = @"INSERT INTO T_WF_MESSAGEBODYDEFINE (DEFINEID,COMPANYID,SYSTEMCODE,MODELCODE,MESSAGEBODY,MESSAGEURL,MESSAGETYPE,
                               RECEIVEPOSTID,RECEIVEPOSTNAME,RECEIVERUSERID,RECEIVERUSERNAME,RECEIVETYPE,CREATEDATE,
                                CREATEUSERNAME,CREATEUSERID) VALUES (:DEFINEID,:COMPANYID,:SYSTEMCODE,:MODELCODE,:MESSAGEBODY,:MESSAGEURL,:MESSAGETYPE,
                                :RECEIVEPOSTID,:RECEIVEPOSTNAME,:RECEIVERUSERID,:RECEIVERUSERNAME,:RECEIVETYPE,:CREATEDATE,:CREATEUSERNAME,:CREATEUSERID)";

                Parameter[] pageparm =
                {  
                    new Parameter(":DEFINEID",dal.GetValue(entity.DEFINEID)), //默认消息ID 
                    new Parameter(":COMPANYID",dal.GetValue(entity.COMPANYID)), //公司ID 
                    new Parameter(":SYSTEMCODE",dal.GetValue(entity.SYSTEMCODE)), //系统代号 
                    new Parameter(":MODELCODE",dal.GetValue(entity.MODELCODE)), //模块代码 
                    new Parameter(":MESSAGEBODY",dal.GetValue(entity.MESSAGEBODY)), //消息体 
                    new Parameter(":MESSAGEURL",dal.GetValue(entity.MESSAGEURL)), //消息链接 
                    new Parameter(":MESSAGETYPE",dal.GetValue(entity.MESSAGETYPE)), //消息类型 
                    new Parameter(":RECEIVEPOSTID",dal.GetValue(entity.RECEIVEPOSTID)), //
                    new Parameter(":RECEIVEPOSTNAME",dal.GetValue(entity.RECEIVEPOSTNAME)), //
                    new Parameter(":RECEIVERUSERID",dal.GetValue(entity.RECEIVERUSERID)) ,//
                    new Parameter(":RECEIVERUSERNAME",dal.GetValue(entity.RECEIVERUSERNAME)), //
                    new Parameter(":RECEIVETYPE",dal.GetValue(entity.RECEIVETYPE)), //
                    new Parameter(":CREATEDATE",dal.GetValue(DateTime.Now)), //创建日期 
                    new Parameter(":CREATEUSERNAME",dal.GetValue(entity.CREATEUSERNAME)), //创建人名称 
                    new Parameter(":CREATEUSERID",dal.GetValue(entity.CREATEUSERID)) //创建人 

                };
                dal.ExecuteSql(insSql, pageparm);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void EditDefaultMessage(T_WF_MESSAGEBODYDEFINE entity)
        {
            try
            {
                string updSql = @"UPDATE T_WF_MESSAGEBODYDEFINE SET SYSTEMCODE=:SYSTEMCODE,MODELCODE=:MODELCODE,MESSAGEBODY=:MESSAGEBODY,
                MESSAGEURL=:MESSAGEURL,RECEIVEPOSTID=:RECEIVEPOSTID,RECEIVEPOSTNAME=:RECEIVEPOSTNAME,RECEIVERUSERID=:RECEIVERUSERID,RECEIVERUSERNAME=:RECEIVERUSERNAME,RECEIVETYPE=:RECEIVETYPE 
                    WHERE   DEFINEID=:DEFINEID";
                Parameter[] pageparm =
                { 
                    new Parameter(":DEFINEID",dal.GetValue(entity.DEFINEID)), //默认消息ID 
                    new Parameter(":SYSTEMCODE",dal.GetValue(entity.SYSTEMCODE)), //系统代号 
                    new Parameter(":MODELCODE",dal.GetValue(entity.MODELCODE)), //模块代码 
                    new Parameter(":MESSAGEBODY",dal.GetValue(entity.MESSAGEBODY)), //消息体 
                    new Parameter(":MESSAGEURL",dal.GetValue(entity.MESSAGEURL)), //消息链接 
                    new Parameter(":RECEIVEPOSTID",dal.GetValue(entity.RECEIVEPOSTID)), //
                    new Parameter(":RECEIVEPOSTNAME",dal.GetValue(entity.RECEIVEPOSTNAME)), //
                    new Parameter(":RECEIVERUSERID",dal.GetValue(entity.RECEIVERUSERID)) ,//
                    new Parameter(":RECEIVERUSERNAME",dal.GetValue(entity.RECEIVERUSERNAME)), //
                    new Parameter(":RECEIVETYPE",dal.GetValue(entity.RECEIVETYPE)) //

                };
                dal.ExecuteSql(updSql, pageparm);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        public void DeleteDefaultMessage(string DeleteID)
        {
            try
            {

                string Sql = "DELETE FROM T_WF_MESSAGEBODYDEFINE WHERE  DEFINEID ='" + DeleteID + "'";
                dal.ExecuteSql(Sql);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
