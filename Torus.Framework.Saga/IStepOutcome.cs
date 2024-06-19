using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torus.Framework.Core.Commands;

namespace Torus.Framework.Saga
{
    public interface IStepOutcome
    {
        bool IsReplyExpected();
        bool IsLocalFailure();
        bool IsSagaCompleted();
        Exception GetLocalException();
        CommandEnvelop GetCommandToSend();
    }

    public class StepOutcome : IStepOutcome
    {
        private readonly CommandEnvelop _commandToSend;
        private readonly Exception _localException;
        private bool _isSagaCompleted;

        private StepOutcome(bool sagaCompleted)
        {
            _isSagaCompleted = sagaCompleted;
        }

        private StepOutcome(CommandEnvelop commandToSend)
        {
            _commandToSend = commandToSend;
        }

        private StepOutcome(Exception localException)
        {
            _localException = localException;
        }

        public bool IsReplyExpected()
        {
            return _commandToSend is not null;
        }

        public bool IsLocalFailure()
        {
            return _localException is not null;
        }
        public Exception GetLocalException()
        {
            return _localException;
        }
        public CommandEnvelop GetCommandToSend()
        {
            return _commandToSend;
        }

        public bool IsSagaCompleted()
        {
            return _isSagaCompleted;
        }

        public static IStepOutcome SagaCompletedOutcome => new StepOutcome(true);
        public static IStepOutcome Create(Exception exception)
        {
            return new StepOutcome(exception);
        }

        public static IStepOutcome Create(CommandEnvelop commandToSend)
        {
            return new StepOutcome(commandToSend);
        }

        public static IStepOutcome SuccessLocal() => new StepOutcome(false);
    }
}
