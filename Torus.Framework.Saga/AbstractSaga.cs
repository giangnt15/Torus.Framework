using Torus.Framework.Saga.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torus.Framework.Core.Shared.Common;
using Torus.Framework.Core.Shared.Sagas;

namespace Torus.Framework.Saga
{
    public abstract class AbstractSaga<TData> : ISaga<TData> where TData : SagaData
    {
        protected readonly List<ISagaStep<TData>> _steps;
        protected string SagaType { get; }

        protected SagaStepBuilder<TData> _stepBuilder;

        public AbstractSaga()
        {
            SagaType = GetType().AssemblyQualifiedName;
            _steps = new List<ISagaStep<TData>>();
            _stepBuilder = new SagaStepBuilder<TData>(this);
        }

        public async Task<IStepOutcome> StartAsync(SagaInstance sagaInstance)
        {
            if (sagaInstance.State != -1)
            {
                throw new SagaUnprocessableException("Saga instance is already started");
            }
            var data = DeserializeSagaData(sagaInstance.SerializedData);
            return await ProcessStepAction(sagaInstance, data);
        }

        public async Task<IStepOutcome> HandleReply(SagaReplyEnvelop sagaReplyEnvelop, SagaInstance sagaInstance)
        {
            var replyType = sagaReplyEnvelop.GetHeader(ReplyMessageHeaders.REPLY_TYPE);
            var currentStep = _steps[sagaInstance.State];
            var data = DeserializeSagaData(sagaInstance.SerializedData);
            if (!currentStep.IsLocal())
            {
                currentStep.ExcuteReplyHandler(replyType, sagaInstance.State, sagaInstance.StateName, sagaReplyEnvelop.Message, data);
            }
            else if (replyType != typeof(PseudoSagaReplyMessage).Name)
            {
                throw new SagaUnprocessableException("Saga reply type is not correct");
            }
            var commandOutcome = sagaReplyEnvelop.GetHeader(SagaReplyHeaders.OUTCOME);
            if (currentStep.IsSuccessfullReply(commandOutcome))
            {
                return await ProcessStepAction(sagaInstance, data);
            }
            else if (sagaInstance.Compensating)
            {
                sagaInstance.TransitionToCompletedFailed();
                return StepOutcome.SagaCompletedOutcome;
            }
            else
            {
                sagaInstance.StartCompensation();
                return await ProcessStepAction(sagaInstance, data);
            }
        }

        private async Task<IStepOutcome> ProcessStepAction(SagaInstance sagaInstance, TData data)
        {
            var (index, nextStep) = NextStepToExecute(sagaInstance.Compensating, sagaInstance.State);
            //Nếu chưa phải bước cuối cùng thì thực hiện bước tiếp theo
            if (nextStep != null)
            {
                IStepOutcome stepOutcome;
                if (sagaInstance.Compensating)
                {
                    stepOutcome = await nextStep.CompensateAsync(data);
                }
                else
                {
                    stepOutcome = await nextStep.ExecuteAsync(data);
                }
                sagaInstance.TransitionState(index, nextStep.GetStateName());
                sagaInstance.UpdateData(SerializeSagaData(data));
                if (stepOutcome.GetCommandToSend() != null)
                {
                    sagaInstance.AddCommand(stepOutcome.GetCommandToSend());
                }
                return stepOutcome;
            }
            else
            {
                sagaInstance.TransitionToCompleted();
                return StepOutcome.SagaCompletedOutcome;
            }
        }

        private (int, ISagaStep<TData>) NextStepToExecute(bool compensating, int state)
        {
            var direction = compensating ? -1 : 1;
            for (int i = state + direction; i >= 0 && i < _steps.Count; i += direction)
            {
                if (compensating)
                {
                    if (_steps[i].HasCompensation())
                    {
                        return (i, _steps[i]);
                    }
                }
                else
                {
                    if (_steps[i].HasAction())
                    {
                        return (i, _steps[i]);
                    }
                }
            }
            return (_steps.Count - 1, null);
        }

        public string SerializeSagaData(TData data)
        {
            return System.Text.Json.JsonSerializer.Serialize(data);
        }

        public TData DeserializeSagaData(string data)
        {
            return System.Text.Json.JsonSerializer.Deserialize<TData>(data);
        }

        public virtual void AddStep(ISagaStep<TData> step)
        {
            _steps.Add(step);
        }

        public SagaInstance NewInstance(TData data) 
        {
            return new SagaInstance()
            {
                Id = Guid.NewGuid(),
                State = -1,
                SerializedData = SerializeSagaData(data),
                SagaType = SagaType,
            };
        }

        public string GetSagaType()
        {
            return SagaType;
        }

        public string GetReplyChannel()
        {
            return SagaType + "-reply";
        }
    }
}
