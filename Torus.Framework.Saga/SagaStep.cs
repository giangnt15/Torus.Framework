using Torus.Framework.Saga.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using Torus.Framework.Core.Shared.Commands;
using Torus.Framework.Core.Messaging;

namespace Torus.Framework.Saga
{
    public abstract class SagaStep<TData> : ISagaStep<TData> where TData : SagaData
    {
        protected string _name;
        protected Dictionary<string, Action<string, int, string, TData>> _replyHandlers;

        public SagaStep(string name) : this()
        {
            _name = name;
        }

        public SagaStep()
        {
            _replyHandlers = new Dictionary<string, Action<string, int, string, TData>>();
        }

        public string GetStateName()
        {
            return _name;
        }

        public void AddReplyHanlder<TReply>(Action<TReply, int, string, TData> action) where TReply : SagaReplyMessage
        {
            _replyHandlers.Add(typeof(TReply).Name, (reply, state, stateName, data)
                => action(Message.FromJson<TReply>(reply), state, stateName, data));
        }

        public bool IsSuccessfullReply(string commandOutcome)
        {
            return commandOutcome.Equals(CommandReplyOutcome.SUCCESS);
        }
        public virtual void ExcuteReplyHandler(string replyTypeName, int state, string stateName, string replyMessage, TData data)
        {
            var replyExpected = _replyHandlers.TryGetValue(replyTypeName, out var handler);
            if (!replyExpected)
            {
                throw new SagaUnprocessableException("Saga reply type is not correct");
            }
            handler.Invoke(replyMessage, state, stateName, data);
        }

        public void SetStateName(string stateName)
        {
            _name = stateName;
        }


        public abstract Task<IStepOutcome> CompensateAsync(TData data);


        public abstract Task<IStepOutcome> ExecuteAsync(TData data);

        public abstract bool HasAction();

        public abstract bool HasCompensation();

        public abstract bool IsLocal();
    }
}
