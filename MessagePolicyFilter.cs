using System.Collections.Generic;
using System.Threading.Tasks;
using IntelSecurity.Common;
using IntelSecurity.Common.Interfaces;
using IntelSecurity.MessagingAPI.Core;
using IntelSecurity.MessagingAPI.Core.Interfaces.Services;
using IntelSecurity.MessagingAPI.Core.Metadata;
using IntelSecurity.MessagingAPI.Core.Model;
using IntelSecurity.MessagingAPI.Infrastructure.Extensions;

namespace IntelSecurity.MessagingAPI.Infrastructure.Filters
{
    public class MessagePolicyFilter : MessageFilter
    {
        private readonly IMessagePolicyService m_MessagePolicyService;
        private readonly IMessageHistoryService m_MessageHistoryService;
        private readonly ILogger m_Logger;
        private const string c_componentName = "MessagePolicyFilter";

        public MessagePolicyFilter(IMessageHistoryService messageHistoryService, IMessagePolicyService messagePolicyService,
            ILogger logger)
        {
            Guard.ThrowIfNull(messageHistoryService, nameof(messageHistoryService));
            Guard.ThrowIfNull(messagePolicyService, nameof(messagePolicyService));
            Guard.ThrowIfNull(logger, nameof(logger));
            m_MessageHistoryService = messageHistoryService;
            m_MessagePolicyService = messagePolicyService;
            m_Logger = logger;
        }

        public override async Task<bool> Filter(MessageFilterContextObject contextObject)
        {
            string l_methodName = "Filter";
            if (!contextObject.Messages.IsNullOrEmpty())
            {
                IList<MessageHistory> l_messageHistories = await m_MessageHistoryService.GetMessageHistoriesAsync(contextObject.Messages, contextObject.ClientContext);
                contextObject.Messages = m_MessagePolicyService.GetMessagesFilteredByPolicy(contextObject.Messages, l_messageHistories, contextObject.ClientContext);
                if (contextObject.Messages.IsNullOrEmpty())
                {
                    m_Logger.LogDebug(LogMessages.NoMatchingMessagesAfterCheckingMessageHistory, c_componentName, l_methodName, contextObject.ClientContext.ToDictionary());
                }
            }
            return true;
        }
    }
}