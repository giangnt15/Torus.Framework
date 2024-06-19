using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torus.Framework.Core.Messaging;
using Torus.Framework.Core.Shared.Commands;
using Torus.Framework.Core.Shared.Common;
using Torus.Framework.Core.Shared.Sagas;

namespace Torus.Framework.Saga
{
    public interface ISagaManager<TData> where TData : SagaData
    {
        Task HandleSagaReply(SagaReplyEnvelop envelop);
        Task<SagaInstance> CreateAsync(TData sagaData);
    }

    public class SagaManager<TData> : ISagaManager<TData> where TData : SagaData
    {
        private readonly ISaga<TData> _saga;
        private readonly ISagaInstanceRepository _sagaInstanceRepository;
        private readonly IMessageConsumer _messageConsumer;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SagaManager(IServiceProvider serviceProvider, IServiceScopeFactory serviceScopeFactory)
        {
            _saga = (ISaga<TData>)serviceProvider.GetService(typeof(ISaga<TData>));
            _sagaInstanceRepository = (ISagaInstanceRepository)serviceProvider.GetService(typeof(ISagaInstanceRepository));
            _messageConsumer = serviceProvider.GetService(typeof(IMessageConsumer)) as IMessageConsumer;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task HandleSagaReply(SagaReplyEnvelop envelop)
        {
            var sagaId = envelop.GetHeader(SagaReplyHeaders.SAGA_ID);
            var sagaType = envelop.GetHeader(SagaReplyHeaders.SAGA_TYPE);
            var sagaInstance = await _sagaInstanceRepository.FindAsync(sagaType, sagaId);
            if (sagaInstance == null)
            {
                return;
            }
            var outcome = await _saga.HandleReply(envelop, sagaInstance);
            await HandleSagaReplyOutcome(outcome, sagaInstance);
        }

        public async Task<SagaInstance> CreateAsync(TData sagaData)
        {
            var sagaInstance = _saga.NewInstance(sagaData);
            await _sagaInstanceRepository.SaveAsync(sagaInstance);
            var outcome = await _saga.StartAsync(sagaInstance);
            await HandleSagaReplyOutcome(outcome, sagaInstance);
            return sagaInstance;
        }

        private async Task HandleSagaReplyOutcome(IStepOutcome stepOutcome, SagaInstance sagaInstance)
        {
            while (true)
            {
                if (stepOutcome.IsLocalFailure())
                {
                    stepOutcome = await _saga.HandleReply(new SagaReplyEnvelop()
                    {
                        Message = "{}",
                        Headers = new Dictionary<string, string>()
                        {
                            { ReplyMessageHeaders.REPLY_TYPE, typeof(PseudoSagaReplyMessage).Name },
                            { SagaReplyHeaders.OUTCOME, CommandReplyOutcome.FAILURE }
                        }
                    }, sagaInstance);
                }
                else
                {
                    await _sagaInstanceRepository.SaveAsync(sagaInstance);
                    if (sagaInstance.Completed)
                    {
                        return;
                    }
                    if (stepOutcome.IsReplyExpected())
                    {
                        return;
                    }
                    else
                    {
                        stepOutcome = await _saga.HandleReply(new SagaReplyEnvelop()
                        {
                            Message = "{}",
                            Headers = new Dictionary<string, string>()
                            {
                                { ReplyMessageHeaders.REPLY_TYPE, typeof(PseudoSagaReplyMessage).Name },
                                { SagaReplyHeaders.OUTCOME, CommandReplyOutcome.SUCCESS }
                            }
                        }, sagaInstance);
                    }
                }
            }
        }

        protected Task SubcribeToReplyChannel()
        {
            _messageConsumer.Subscribe<SagaReplyEnvelop>(_saga.GetSagaType(), _saga.GetReplyChannel(), HandleSagaReply);
            return Task.CompletedTask;
        }
    }
}
