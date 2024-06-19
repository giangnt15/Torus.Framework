using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torus.Framework.Core.Commands;

namespace Torus.Framework.Saga
{
    public class SagaInstance
    {
        public Guid Id { get; set; }
        public string SagaType { get; set; }
        public string StateName { get; set; }
        public string SerializedData { get; set; }
        public bool Compensating { get; set; }
        public bool Failed { get; set; }
        public bool Completed { get; set; }
        public int State { get; set; }

        protected List<CommandEnvelop> Commands;

        public SagaInstance()
        {
            Commands = new List<CommandEnvelop>();
        }

        public void AddCommand(CommandEnvelop command)
        {
            Commands.Add(command);
        }

        public void ClearCommands()
        {
            Commands.Clear();
        }

        public List<CommandEnvelop> GetCommands()
        {
            return Commands.ToList();
        }

        public void StartCompensation()
        {
            Compensating = true;
        }

        public void TransitionState(int state, string stateName)
        {
            State = state;
            StateName = stateName;
        }

        public void UpdateData(string data)
        {
            SerializedData = data;
        }

        public void TransitionToCompletedFailed()
        {
            Completed = true;
            Failed = true;
        }

        public void TransitionToCompleted()
        {
            Completed = true;
        }
    }
}
