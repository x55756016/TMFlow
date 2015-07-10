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
        public static Dictionary<string, WorkflowInstance> WorkflowInstanceAll;

        /// <summary>
        /// 创建工作流运行时
        /// </summary>
        /// <param name="IsPer">是否使用持久化</param>
        /// <returns></returns>
        public static WorkflowRuntime CreateWorkFlowRuntime(bool IsPer)
        {
            try
            {
                WorkflowRuntime WfRuntime = new WorkflowRuntime();
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
        /// <param name="FlowDefineXml">模型文件</param>
        /// <param name="Rules">规则文件</param>
        /// <returns></returns>
        public static WorkflowInstance CreateWorkflowInstance(WorkflowRuntime WfRuntime, string FlowDefineXml, string Rules)
        {
            try
            {
                WorkflowInstance instance=new WorkflowInstance();
                instance.WorkFlowDefine=XMLFlowManager.CreateWFInstanceFromXmlDefine(FlowDefineXml);
                instance.InstanceId = Guid.NewGuid().ToString();
                WorkflowInstanceAll.Add(instance.InstanceId, instance);
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
        public static WorkflowInstance CreateFreeWorkflowInstance(WorkflowRuntime WfRuntime, string xmlFileName)
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
                if(WorkflowInstanceAll[INSTANCEID]!=null)
                {
                    return WorkflowInstanceAll[INSTANCEID];
                }
                else
                {
                    return null;
                }
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
            string StateName = CurrentStateName;
            try
            {
                if (instance == null)
                {
                    StateName = "EndFlow";
                    return StateName;
                }
                StateName = XMLFlowManager.GetNextNode(instance.WorkFlowDefine, CurrentStateName, string.Empty);
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
                NextNode = XMLFlowManager.GetNextNode(instance.WorkFlowDefine, CurrentStateName, string.Empty);
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
