using System.Threading.Tasks;
using IntelSecurity.Common;
using IntelSecurity.MessagingAPI.Core.Interfaces.Filters;
using IntelSecurity.MessagingAPI.Core.Model;

namespace IntelSecurity.MessagingAPI.Infrastructure.Filters
{
    public abstract class MessageFilter : ActionChainNode, IMessageFilter
    {
        public abstract Task<bool> Filter(MessageFilterContextObject contextObject);
    }
}