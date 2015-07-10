using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using SMT.WFLib;
using System.Xml;
using System.IO;
using SMT.FlowWFService.PublicClass;
using SMT.Foundation.Log;
using SMT.FlowWFService.XmlFlowManager;

namespace SMT.FlowWFService.NewFlow
{
    /// <summary>
    /// 流程运行操作类
    /// </summary>
    class SMTWorkFlowManage
    {

        /// <summary>
        /// 创建工作流运行时
        /// </summary>
        /// <param name="IsPer">是否使用持久化</param>
        /// <returns></returns>
        public static WorkflowRuntime CreateWorkFlowRuntime(bool IsPer)
        {
            try
            {
               // WorkflowRuntime WfRuntime = new WorkflowRuntime();
                WorkflowRuntime WfRuntime = new WorkflowRuntime();

                //if (IsPer)
                //{
                //    ConnectionStringSettings defaultConnectionString = ConfigurationManager.ConnectionStrings["//OracleConnection"];
                //    WfRuntime.AddService(new AdoPersistenceService(defaultConnectionString, true, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(0)));
                //    WfRuntime.AddService(new AdoTrackingService(defaultConnectionString));
                //    WfRuntime.AddService(new AdoWorkBatchService());
                //}

                //FlowEvent ExternalEvent = new FlowEvent();
                //ExternalDataExchangeService objService = new ExternalDataExchangeService();
                //WfRuntime.AddService(objService);
                //objService.AddService(ExternalEvent);

                //ManualWorkflowSchedulerService scheduleService = new ManualWorkflowSchedulerService();
                //WfRuntime.AddService(scheduleService);

                //TypeProvider typeProvider = new TypeProvider(null);
                //WfRuntime.AddService(typeProvider);
                //WfRuntime.StartRuntime();
                return WfRuntime;
            }
            catch (Exception ex)
            {
                Tracer.Debug("CreateWorkFlowRuntime异常信息 ：" + ex.ToString());
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 释放运行时
        /// </summary>
        /// <param name="WfRuntime"></param>
        public static void ColseWorkFlowRuntime(WorkflowRuntime WfRuntime)
        {
            
        }

        /// <summary>
        /// 根据模型文件创建工作流实例
        /// </summary>
        /// <param name="WfRuntime">运行时</param>
        /// <param name="Xoml">模型文件</param>
        /// <param name="Rules">规则文件</param>
        /// <returns></returns>
        public static WorkflowInstance CreateWorkflowInstance(WorkflowRuntime WfRuntime, string Xoml, string Rules)
        {
            try
            {
                WorkflowInstance instance;
                XmlReader readerxoml, readerule;
                StringReader strXoml = new StringReader(Xoml);
                StringReader strRules = new StringReader(Rules == null ? "" : Rules);

                readerxoml = XmlReader.Create(strXoml);
                readerule = XmlReader.Create(strRules);

                //  WorkflowRuntime workflowRuntime = SMTWorkFlowManage.StarWorkFlowRuntime(true);
                if (Rules == null || Rules=="")
                    instance =new WorkflowInstance();
                else
                    instance = new WorkflowInstance();

                //instance.Start();
                return instance;
            }catch(Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// 建立虚拟流程实例
        /// </summary>
        /// <param name="WfRuntime"></param>
        /// <param name="xmlFileName"></param>
        /// <returns></returns>
        public static WorkflowInstance CreateWorkflowInstance(WorkflowRuntime WfRuntime, string xmlFileName)
        {
            try
            {
                WorkflowInstance instance = new WorkflowInstance();

                return instance;
            }
            catch (Exception ex)
            {
                Tracer.Debug("CreateWorkflowInstance异常信息 ：" + ex.ToString());
                throw new Exception(ex.Message);
            }

        }
        /// <summary>
        /// 从持久化库在恢复实例
        /// </summary>
        /// <param name="WfRuntime"></param>
        /// <param name="INSTANCEID"></param>
        /// <returns></returns>
        public static WorkflowInstance GetWorkflowInstance(WorkflowRuntime WfRuntime, string INSTANCEID)
        {
            try
            {
                WorkflowInstance instance = new WorkflowInstance();
                return instance;
            }
            catch (Exception ex)
            {
                Tracer.Debug("实例ID instanceid=" + INSTANCEID + " ;GetWorkflowInstance异常信息 ：" + ex.ToString());
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 克隆一个实例
        /// </summary>
        /// <param name="WfRuntimeClone"></param>
        /// <param name="instanceClone"></param>
        /// <param name="WfRuntime"></param>
        /// <returns></returns>
        public static WorkflowInstance CloneWorkflowInstance(WorkflowRuntime WfRuntimeClone, WorkflowInstance instanceClone, WorkflowRuntime WfRuntime)
        {
            try
            {

                WorkflowInstance instance = new WorkflowInstance();
             
                return instance;
            }
            catch (Exception ex)
            {
                Tracer.Debug("CloneWorkflowInstance异常信息 ：" + ex.ToString());
                throw new Exception(ex.Message);
            }

        }
        /// <summary>
        /// 获取当前实例的状态代码
        /// </summary>
        /// <param name="WfRuntime"></param>
        /// <param name="instance"></param>
        /// <param name="CurrentStateName"></param>
        /// <returns></returns>
        public static string GetNextState(WorkflowRuntime WfRuntime, WorkflowInstance instance, string CurrentStateName)
        {
            try
            {
                string StateName = CurrentStateName;
                //Tracer.Debug("循环获取当前实例的状态代码  （开始）instance＝" + (instance != null ? instance.InstanceId.ToString() : "null") + " StateName＝" + StateName);
                while (StateName == CurrentStateName)
                {
                    if (instance == null)
                    {
                        StateName = "EndFlow";
                        return StateName;
                    }
                    //StateMachineWorkflowInstance workflowinstance = new StateMachineWorkflowInstance(WfRuntime, instance.InstanceId);
                    StateName ="CurrentStateName";
                }
                return StateName;
            }
            catch (Exception ex)
            {
                Tracer.Debug("GetNextState异常信息 ：" + ex.ToString());
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 激发事件到一下状态，并获取状态代码
        /// </summary>
        /// <param name="WfRuntime"></param>
        /// <param name="instance"></param>
        /// <param name="CurrentStateName"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string GetNextStateByEvent(WorkflowRuntime WfRuntime, WorkflowInstance instance, string CurrentStateName, string xml)
        {
            string NextNode = string.Empty;
            try
            {
                string FlowDefineXml=string.Empty;
                NextNode= FlowManager.GetNextNode(FlowDefineXml, CurrentStateName, xml);
            }
            catch (Exception ex)
            {
                Tracer.Debug("GetNextStateByEvent异常信息 ：" + ex.ToString());
                throw new Exception(ex.Message);
            }
            return NextNode;
        }
    }
}
