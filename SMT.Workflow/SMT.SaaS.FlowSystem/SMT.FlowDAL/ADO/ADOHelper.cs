using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;
using System.Runtime.Caching;
using SMT.Foundation.Log;
using SMT.Foundation.Core;


namespace SMT.FLOWDAL.ADO
{   
   
    public class ADOHelper
    {

        public static void AddParameter(string name, object value, OracleType oracleType, ParameterCollection parameters)
        {
            Parameter p = new Parameter("p" + name, oracleType);
            switch (oracleType)
            {
                case OracleType.DateTime:
                    p.ParameterValue = value == null ? DateTime.Now : value;
                    break;
                default:
                      if (value == null)
                    {
                        p.ParameterValue = string.Empty;
                    }
                    else
                    {
                        if (value.ToString() == "")
                        {
                            p.ParameterValue = string.Empty;
                        }
                        else
                        {
                            p.ParameterValue = value;
                        }
                    }
                    
                    break;
                    
            }
            parameters.Add(p);
            
        }
    }

   
}
