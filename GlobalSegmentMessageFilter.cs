using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IntelSecurity.Common;
using IntelSecurity.Common.Interfaces;
using IntelSecurity.ExpressionParser.RuleEngine.Interfaces;
using IntelSecurity.MessagingAPI.Core.Model;
using IntelSecurity.MessagingAPI.Core.Types;
using IntelSecurity.MessagingAPI.Infrastructure.Extensions;

namespace IntelSecurity.MessagingAPI.Infrastructure.Filters
{
    public class GlobalSegmentMessageFilter : MessageFilter
    {
        private readonly IRuleEngine m_RuleEngine;
        private readonly ILogger m_Logger;

        public GlobalSegmentMessageFilter(IRuleEngine ruleEngine, ILogger logger)
        {
            Guard.ThrowIfNull(ruleEngine, nameof(ruleEngine));
            Guard.ThrowIfNull(logger, nameof(logger));
            m_Logger = logger;
            m_RuleEngine = ruleEngine;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public override async Task<bool> Filter(MessageFilterContextObject contextObject)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            ClientContext l_ClientContext = contextObject.ClientContext;
            if (!l_ClientContext.IsCohortAssigned && !l_ClientContext.IsCustomSegmentAssigned)
            {
                IList<Segment> l_segments = contextObject.Catalog.GetMatchingSegments(l_ClientContext, m_RuleEngine, m_Logger);
                contextObject.Messages = contextObject.Catalog.GetGlobalSegmentMessages(l_ClientContext, l_segments, m_RuleEngine, m_Logger);
            }
            return true;
        }
    }
}