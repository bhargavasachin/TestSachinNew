using System.Collections.Generic;
using System.Threading.Tasks;
using IntelSecurity.Common;
using IntelSecurity.Common.Interfaces;
using IntelSecurity.ExpressionParser.RuleEngine.Interfaces;
using IntelSecurity.MessagingAPI.Core.Interfaces.Services;
using IntelSecurity.MessagingAPI.Core.Model;
using IntelSecurity.MessagingAPI.Infrastructure.Extensions;

namespace IntelSecurity.MessagingAPI.Infrastructure.Filters
{
    public class LegacyCohortMessageFilter : MessageFilter
    {
        private readonly IRuleEngine m_RuleEngine;
        private readonly ILogger m_Logger;
        private readonly ILegacyCohortService m_LegacyCohortService;

        public LegacyCohortMessageFilter(IRuleEngine ruleEngine, ILogger logger, ILegacyCohortService legacyCohortService)
        {
            Guard.ThrowIfNull(ruleEngine, nameof(ruleEngine));
            Guard.ThrowIfNull(logger, nameof(logger));
            Guard.ThrowIfNull(legacyCohortService, nameof(legacyCohortService));
            m_RuleEngine = ruleEngine;
            m_Logger = logger;
            m_LegacyCohortService = legacyCohortService;
        }


        public override async Task<bool> Filter(MessageFilterContextObject contextObject)
        {
            IList<Segment> l_segments = contextObject.Catalog.GetMatchingSegments(contextObject.ClientContext, m_RuleEngine, m_Logger);
            if (l_segments != null && l_segments.Count != 0)
            {
                //Get Cohort messages
                IList<Message> l_matchingMessages = await m_LegacyCohortService.GetCohortMessagesAsync(contextObject.ClientContext, l_segments);
                if (contextObject.ClientContext.IsCohortAssigned)
                {
                    contextObject.Messages = l_matchingMessages;
                    return true;
                }
            }
            return false;
        }
    }
}