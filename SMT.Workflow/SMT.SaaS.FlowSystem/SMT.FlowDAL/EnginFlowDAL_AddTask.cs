using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml.Linq;
using System.IO;
using SMT.Workflow.Common.Model.FlowEngine;
using System.Data.OracleClient;
using SMT.Workflow.SMTCache;
using EngineDataModel;
using SMT.Foundation.Log;
using SMT.Foundation.Core;

namespace SMT.FlowDAL
{
    public partial class EnginFlowDAL
    {
        /// <summary>
        /// 新增待办
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dr1"></param>
        /// <param name="SourceValueDT"></param>
        /// <param name="strAPPFIELDVALUE"></param>
        public void AddDoTask(T_WF_DOTASK entity, DataRow[] drs, DataTable SourceValueDT, string strAPPFIELDVALUE, string submitUserName, string ModeName, string applicationUrl)
        {
            CloseDoTaskStatus(entity.SYSTEMCODE, entity.ORDERID, null);
            try
            {
                string[] strListUser;
                if (entity.RECEIVEUSERID.IndexOf('|') != -1)
                {
                    strListUser = entity.RECEIVEUSERID.ToString().Split('|');
                }
                else
                {
                    strListUser = new string[1];
                    strListUser[0] = entity.RECEIVEUSERID.ToString();
                }
                //
                foreach (string User in strListUser)
                {
                    string insSql = @"INSERT INTO T_WF_DOTASK (DOTASKID,COMPANYID,ORDERID,ORDERUSERID,ORDERUSERNAME,ORDERSTATUS,MESSAGEBODY,
                                     APPLICATIONURL,RECEIVEUSERID,BEFOREPROCESSDATE,DOTASKTYPE,DOTASKSTATUS,MAILSTATUS,
                                     RTXSTATUS,APPFIELDVALUE,FLOWXML,APPXML,SYSTEMCODE,MODELCODE,MODELNAME,REMARK)
                                     VALUES (@DOTASKID,@COMPANYID,@ORDERID,@ORDERUSERID,@ORDERUSERNAME,@ORDERSTATUS,@MESSAGEBODY,@APPLICATIONURL,
                                    @RECEIVEUSERID,@BEFOREPROCESSDATE,@DOTASKTYPE,@DOTASKSTATUS,@MAILSTATUS,@RTXSTATUS,
                                    @APPFIELDVALUE,@FLOWXML,@APPXML,@SYSTEMCODE,@MODELCODE,@MODELNAME,@REMARK)";
                    Parameter[] pageparm =
                        {               
                            new Parameter("@DOTASKID",null), 
                            new Parameter("@COMPANYID",null), 
                            new Parameter("@ORDERID",null), 
                            new Parameter("@ORDERUSERID",null), 
                            new Parameter("@ORDERUSERNAME",null), 
                            new Parameter("@ORDERSTATUS",null), 
                            new Parameter("@MESSAGEBODY",null), 
                            new Parameter("@APPLICATIONURL",null), 
                            new Parameter("@RECEIVEUSERID",null), 
                            new Parameter("@BEFOREPROCESSDATE",null), 
                            new Parameter("@DOTASKTYPE",null),
                            new Parameter("@DOTASKSTATUS",null), 
                            new Parameter("@MAILSTATUS",null), 
                            new Parameter("@RTXSTATUS",null),                  
                            new Parameter("@APPFIELDVALUE",null), 
                            new Parameter("@FLOWXML",null), 
                            new Parameter("@APPXML",null), 
                            new Parameter("@SYSTEMCODE",null), 
                            new Parameter("@MODELCODE",null), 
                            new Parameter("@MODELNAME",null),                  
                            new Parameter("@REMARK",null)
                        };
                    pageparm[0].ParameterValue = GetValue(Guid.NewGuid().ToString());//待办任务ID
                    pageparm[1].ParameterValue = GetValue(entity.COMPANYID);//公司ID
                    pageparm[2].ParameterValue = GetValue(entity.ORDERID);//单据ID
                    pageparm[3].ParameterValue = GetValue(entity.ORDERUSERID);//单据所属人ID
                    pageparm[4].ParameterValue = GetValue(entity.ORDERUSERNAME);//单据所属人名称
                    pageparm[5].ParameterValue = GetValue(entity.ORDERSTATUS);//单据状态

                    if (SourceValueDT != null)
                    {
                        foreach (DataRow dr in SourceValueDT.Rows)
                        {
                            if (!string.IsNullOrEmpty(dr["ColumnValue"].ToString().Trim()))
                            {
                                if (dr["ColumnName"].ToString().ToLower() == "appusername")
                                {
                                    string AppUserName = dr["ColumnValue"].ToString();
                                    pageparm[20].ParameterValue = AppUserName;//接收员工名，使用remark字段
                                }
                            }
                        }
                    }
                    #region 消息体
                    string XmlTemplete = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" + "\r\n" +
                                  "<System>" + "\r\n" +
                                  "{0}" +
                                  "</System>";

                    string strMsgBody = string.Empty;
                    string strMsgUrl = applicationUrl;
                    if (drs.Count() == 0)//如果没有设置消息，则构造默认消息：请审核xxx提交的"xxxx",
                    {
                        strMsgBody = "请审核[" + submitUserName + @"]提交的[" + ModeName + "]";
                        pageparm[6].ParameterValue = strMsgBody;//消息体  
                        pageparm[7].ParameterValue = ReplaceLowerValue(strMsgUrl, SourceValueDT);//应用URL     
                    }
                    else
                    {
                        DataRow dr1 = drs[0];
                        if (dr1["MESSAGEBODY"].ToString() == "")//默认消息为空
                        {
                            if (dr1 != null)
                            {
                                ModelMsgDefine(dr1["SYSTEMCODE"].ToString(), dr1["MODELCODE"].ToString(), entity.COMPANYID, ref strMsgBody, ref strMsgUrl);
                            }
                            if (string.IsNullOrEmpty(strMsgBody))
                            {
                                try
                                {
                                    DataRow[] drvList = SourceValueDT.Select("ColumnName='ModelName'");
                                    if (drvList.Count() == 1)
                                    {
                                        string value = drvList[0]["ColumnValue"].ToString();
                                        if (string.IsNullOrWhiteSpace(value))
                                        {
                                            value = drvList[0]["ColumnText"].ToString();
                                        }
                                        pageparm[6].ParameterValue = GetValue(value + "已审批通过");//消息体                                    
                                    }
                                    else
                                    {
                                        pageparm[6].ParameterValue = GetValue(entity.ORDERID + "已审批通过");//消息体 

                                    }
                                }
                                catch { }

                            }
                            else
                            {
                                pageparm[6].ParameterValue = GetValue(ReplaceMessageBody(strMsgBody, SourceValueDT));//消息体                             
                            }
                            string strUrl = string.Format(XmlTemplete, ReplaceValue(strMsgUrl, SourceValueDT));
                            Tracer.Debug("查询到得消息链接：" + strUrl + "单据ID：" + entity.ORDERID);
                            pageparm[7].ParameterValue = GetValue(strUrl);//应用URL                         
                        }
                        else//在引擎配置界面定义了消息内容
                        {
                            Tracer.Debug("Formid=" + entity.ORDERID + "开始 待办消息体:" + dr1["MESSAGEBODY"].ToString() + "\n\r 开始 打开待办连接的参数:" + dr1["APPLICATIONURL"].ToString());
                            string rowsValues = "Formid=" + entity.ORDERID + "\r\n";//每一行的值
                            for (int j = 0; j < SourceValueDT.Rows.Count; j++)
                            {
                                for (int i = 0; i < SourceValueDT.Columns.Count; i++)
                                {
                                    string columnName = SourceValueDT.Columns[i].ColumnName;
                                    rowsValues += columnName + "=" + SourceValueDT.Rows[j][columnName].ToString() + ";";
                                }
                                rowsValues += "\r\n";
                            }
                            Tracer.Debug("SourceValueDT表数据:" + rowsValues);
                            pageparm[6].ParameterValue = GetValue(ReplaceMessageBody(dr1["MESSAGEBODY"].ToString(), SourceValueDT));//消息体
                            pageparm[7].ParameterValue = GetValue(string.Format(XmlTemplete, ReplaceValue(dr1["APPLICATIONURL"].ToString(), SourceValueDT)));//应用URL   
                            Tracer.Debug("Formid=" + entity.ORDERID + "最后 待办消息体:" + pageparm[6].ParameterValue + "\n\r 最后 打开待办连接的参数:" + pageparm[7].ParameterValue);
                        }
                    }
                    #endregion
                    pageparm[8].ParameterValue = GetValue(User);// GetValue(entity.RECEIVEUSERID);//接收用户ID
                    if (entity.BEFOREPROCESSDATE != null)//流程过期时间属性
                    {
                        //sql += "to_date('" + entity.BEFOREPROCESSDATE + "','YYYY-MM-DD hh24:mi:ss')";
                        pageparm[9].ParameterValue = GetValue(entity.BEFOREPROCESSDATE);//可处理时间（主要针对KPI考核）
                    }
                    else
                    {
                        if (drs.Count() > 0)//如果没有设置消息，则构造默认消息：请审核xxx提交的"xxxx",
                        {
                            DataRow dr1 = drs[0];
                            if (dr1["LASTDAYS"] != null)
                            {
                                if (string.IsNullOrEmpty(dr1["LASTDAYS"].ToString()))
                                {
                                    pageparm[9].ParameterValue = GetValue(DateTime.Now.AddDays(3));//可处理时间（主要针对KPI考核）
                                }
                                else
                                {
                                    pageparm[9].ParameterValue = GetValue(DateTime.Now.AddDays(int.Parse(dr1["LASTDAYS"].ToString())));//可处理时间（主要针对KPI考核）                             
                                }
                            }
                            else
                            {
                                pageparm[9].ParameterValue = GetValue(DateTime.Now.AddDays(3));
                            }
                        }
                    }
                    pageparm[10].ParameterValue = GetValue(0);//待办任务类型(0、待办任务、1、流程咨询、3 ) 
                    pageparm[11].ParameterValue = GetValue(0);//代办任务状态(0、未处理 1、已处理 、2、任务撤销 10、删除)
                    pageparm[12].ParameterValue = GetValue(0);//邮件状态(0、未发送 1、已发送、2、未知 )
                    pageparm[13].ParameterValue = GetValue(0);//RTX状态(0、未发送 1、已发送、2、未知 )
                    pageparm[14].ParameterValue = GetValue(strAPPFIELDVALUE);//应用字段值
                    pageparm[15].ParameterValue = GetValue(entity.FLOWXML);//流程XML
                    pageparm[16].ParameterValue = GetValue(entity.APPXML);//应用XML
                    pageparm[17].ParameterValue = GetValue(entity.SYSTEMCODE);//系统代码                  
                    pageparm[18].ParameterValue = GetValue(entity.MODELCODE);//模块代码
                    pageparm[19].ParameterValue = GetValue(entity.MODELNAME);//模块名称
                    //DataRow[] ModelCodeList = SourceValueDT.Select("ColumnName='ModelCode'");
                    //if (ModelCodeList.Count() == 1)
                    //{
                    //    sql += "'" + ModelCodeList[0]["ColumnValue"].ToString() + "')";
                    //}
                    //else
                    //{
                    //    sql += "'')";
                    //}
                    int result = dao.ExecuteNonQuery(insSql, pageparm);
                    if (result > 0)
                    {
                        Tracer.Debug("A新增待办任务AddDoTask （成功）  FormID=" + entity.ORDERID + " 接收人ID＝" + User);
                    }
                    else
                    {
                        Tracer.Debug("A新增待办任务AddDoTask （失败）  FormID=" + entity.ORDERID + " 接收人ID＝" + User);
                    }
                }


            }
            catch (Exception ex)
            {
                Tracer.Debug("A新增待办任AddDoTask （失败） FormID=" + entity.ORDERID + " 命名空间： SMT.FlowDAL.EnginFlowDAL 类方法：AddDoTask（）" + ex.Message);
                throw new Exception("新增待办任失败 FormID=" + entity.ORDERID);
            }

        }
    }
}
