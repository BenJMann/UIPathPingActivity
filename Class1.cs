using System.Activities;
using System.ComponentModel;
using System.Net.NetworkInformation;

namespace PingActivity
{
    public class PingHost : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> HostName { get; set; }

        [Category("Output")]
        public OutArgument<bool> Success { get; set; }

        [Category("Output")]
        public OutArgument<System.Int32> DelayMS { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var hostName = HostName.Get(context);
            bool success;
            Ping ping = null;

            try
            {
                ping = new Ping();
                PingReply reply;

                reply = ping.Send(hostName);
                success = reply.Status == IPStatus.Success;

                if (success)
                {
                    DelayMS.Set(context, (int)reply.RoundtripTime);
                }

                Success.Set(context, success);
            }
            catch (System.Exception)
            {
                Success.Set(context, false);
            }
            finally
            {
                if (ping != null)
                {
                    ping.Dispose();
                }
            }
        }
    }
}
