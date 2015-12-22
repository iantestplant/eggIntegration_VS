
using CookComputing.XmlRpc;
using System;

namespace eggIntegration_VS
{
    public interface ISession : IXmlRpcProxy
    {
        [XmlRpcMethod("StartSession")]
        string StartSession(string suite);

        [XmlRpcMethod("EndSession")]
        string EndSession();

        [XmlRpcMethod("Execute")]
        XmlRpcStruct Execute(string command);

        [XmlRpcMethod("ExecuteWithTimeout")]
        XmlRpcStruct ExecuteWithTimeout(string command);

        [XmlRpcMethod("Update")]
        XmlRpcStruct Update(string command);
    }

    class XMLRPC_Run
    {
        readonly ISession _proxy = XmlRpcProxyGen.Create<ISession>();
        public string FaultString;
        public XmlRpcStruct ExecuteResult;
        public XmlRpcStruct ScriptResult;
        readonly private string _suite;
        readonly private string _script;

        public XMLRPC_Run(string url, string suite, string script)
        {
            _proxy.Url = url.StartsWith("http://") ? url : "http://" + url;
            _suite = suite;
            _script = script;
        }

        public bool RunWithTimeout()
        {
            bool connected = false;
            try
            {
                try
                {
                    _proxy.EndSession();
                }
                catch (Exception) { }

                Utils.Trace(_proxy.StartSession(_suite));
                _proxy.Timeout = -1;
                connected = true;
                ScriptResult = _proxy.ExecuteWithTimeout("RunWithNewResults \"" + _script + "\"");
                //ScriptResult = _proxy.ExecuteWithTimeout("Run \"" + _script + "\"");

                // If the execution has timed out, then repeatedly call Update until the action has completed
                while ((bool)ResultValue("TimedOut"))
                {
                    ScriptResult = _proxy.Update("2");
                }

                //ScriptResult = _proxy.Execute("return EggPlantVersion()");
                ScriptResult = _proxy.Execute("return ScriptResults(" + _script + ")");

                foreach (var key in ScriptResult.Keys) // Duration, Output, Result, ReturnValue (always empty?)
                {
                    Utils.Trace(String.Format("{0} = {1}", key, ScriptResult[key]));
                    if (key.ToString().Contains("Result") && (string)ScriptResult[key] != "")
                        ScriptResult = (XmlRpcStruct)ScriptResult[key];
                }
            }
            catch (XmlRpcFaultException e)
            {
                // Only get the first line as includes the whole script after the fault line
                FaultString = e.Message.Split(new char[] { '\n' }, 2)[0];
                return false;
            }
            catch (System.Net.WebException e)
            {
                FaultString = e.Message;
                return false;
            }
            catch (Exception e)
            {
                FaultString = e.InnerException.ToString();
                return false;
            }
            finally
            {
                if (connected)
                    Utils.Trace(_proxy.EndSession());
            }
            return true;
        }

        public bool Run()
        {
            bool connected = false;
            try
            {
                try
                {
                    _proxy.EndSession();
                }
                catch (Exception) { }

                Utils.Trace(_proxy.StartSession(_suite));
                _proxy.Timeout = -1; // Need to allow for arbitrarily log tests.
                connected = true;
                ExecuteResult = _proxy.Execute("RunWithNewResults \"" + _script + "\"");
                foreach (var key in ExecuteResult.Keys) // Duration, Output, Result, ReturnValue (always empty?)
                {
                    Utils.Trace(String.Format("{0} = {1}", key, ExecuteResult[key]));
                    if (key.ToString().Contains("Result"))
                        ScriptResult = (XmlRpcStruct)ExecuteResult[key];
                }
                ExecuteResult = _proxy.Execute("return ScriptResults(" + _script + ")");
            }
            catch (XmlRpcFaultException e)
            {
                // Only get the first line as includes the whole script after the fault line
                FaultString = e.Message.Split(new char[] {'\n'}, 2)[0];
                return false;
            }
            catch (System.Net.WebException e)
            {
                FaultString = e.Message;
                return false;
            }
            catch (Exception e)
            {
                FaultString = e.InnerException.ToString();
                return false;
            }
            finally
            {
                if (connected)
                    Utils.Trace(_proxy.EndSession());
            }
            return true;
        }

       public bool parseEggplantReturnData(XmlRpcStruct ExecuteResult)
       {
            foreach (var key in ExecuteResult.Keys) // Duration, Output, Result, ReturnValue (always empty?)
            {
                //Utils.Trace(String.Format("{0} = {1}", key, ExecuteResult[key]));
                //if (key.ToString().Contains("Result"))
                //    ScriptResult = (XmlRpcStruct)ExecuteResult[key];
            }
            return false;
       }
        public object ResultValue(object key)
        {
            if (ScriptResult.ContainsKey(key))
                return ScriptResult[key];
            return null;
        }

    }
}
