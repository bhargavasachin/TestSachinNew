using System.Linq;
using System.Threading.Tasks;
using IntelSecurity.MessagingAPI.Core.Model;
using IntelSecurity.MessagingAPI.Infrastructure.Extensions;

namespace IntelSecurity.MessagingAPI.Infrastructure.Filters
{
    public class EventMessageFilter : MessageFilter
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public override async Task<bool> Filter(MessageFilterContextObject contextObject)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (!contextObject.Messages.IsNullOrEmpty())
            {
                contextObject.Messages = (from m in contextObject.Messages
                    where m.Rule.RuleDefinition.Contains("EventCode")
                    select m).ToList();
            }
            return true;
        }
    }
}