using Apocalibs.StateMachine;
using Apocalibs.StateMachine.Exceptions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Statemachine.Tests
{
    public class StateMachineTests
    {
        private enum State
        {
            Scheduled,
            Running,
            Stopping,
            Stopped,
            Pauzed,
        }

        private enum Trigger
        {
            Start,
            Stop,
            Pauze,
        }

        [Test]
        public void StateMachine_SetupNew_NoStates_ThrowsStateNotFoundException()
        {
            Assert.Throws<StateNotFoundException<State>>(() =>
            {
                var statemachine = StateMachine.SetupNew<State, Trigger>()
                    .FinishSetup(State.Scheduled);
            });
        }

        [Test]
        public void StateMachine_SetupNew_InitialStateExists_InitialStateCorrect()
        {
            var statemachine = StateMachine.SetupNew<State, Trigger>()
                .AddState(State.Scheduled, (b) => b.Allow(t => t.TransitionTo(State.Running).On(Trigger.Start)))
                .FinishSetup(State.Scheduled);

            Assert.AreEqual(State.Scheduled, statemachine.CurrentState);
        }

        [Test]
        public void StateMachine_SendTriggerAsync_TransitionToNonExistingState_ThrowsStateNotFoundException()
        {
            var statemachine = StateMachine.SetupNew<State, Trigger>()
                .AddState(State.Scheduled, (b) => b.Allow(t => t.TransitionTo(State.Running).On(Trigger.Start)))
                .FinishSetup(State.Scheduled);
            Assert.ThrowsAsync<StateNotFoundException<State>>(() =>
            {
                return statemachine.SendTriggerAsync(Trigger.Start);
            });
        }

        [Test]
        public async Task StateMachine_SendTriggerAsync_TransitionToExistingState_StateChanged()
        {
            var statemachine = StateMachine.SetupNew<State, Trigger>()
                .AddState(State.Scheduled, (b) => b.Allow(t => t.TransitionTo(State.Running).On(Trigger.Start)))
                .AddState(State.Running, (b) => b.Allow(t => t.TransitionTo(State.Stopping).On(Trigger.Stop)))
                .FinishSetup(State.Scheduled);
            await statemachine.SendTriggerAsync(Trigger.Start);

            Assert.AreEqual(State.Running, statemachine.CurrentState);
        }

        [Test]
        public async Task StateMachine_SendTriggerAsync_TransitionsWithConstraint_StateChangedOnceConstraintIsMet()
        {
            bool isStopped = false;
            bool stateChanged = false;

            var statemachine = StateMachine.SetupNew<State, Trigger>()
                .AddState(State.Scheduled, (b) => b.Allow(t => t.TransitionTo(State.Running).On(Trigger.Start)))
                .AddState(State.Running, (b) => b.Allow(t => t.TransitionTo(State.Stopping).On(Trigger.Stop)))
                .AddState(State.Stopping, (b) => b.Allow(t => t.TransitionTo(State.Stopped).On(Trigger.Stop).When(() => isStopped)))
                .AddState(State.Stopped, (b) => b.Allow(t => t.TransitionTo(State.Running).On(Trigger.Start)))
                .FinishSetup(State.Scheduled);

            stateChanged = await statemachine.SendTriggerAsync(Trigger.Start);
            Assert.IsTrue(stateChanged);
            Assert.AreEqual(State.Running, statemachine.CurrentState);

            stateChanged = await statemachine.SendTriggerAsync(Trigger.Stop);
            Assert.IsTrue(stateChanged);
            Assert.AreEqual(State.Stopping, statemachine.CurrentState);

            stateChanged = await statemachine.SendTriggerAsync(Trigger.Stop);
            Assert.IsFalse(stateChanged);
            Assert.AreEqual(State.Stopping, statemachine.CurrentState);

            isStopped = true;
            stateChanged = await statemachine.SendTriggerAsync(Trigger.Stop);
            Assert.IsTrue(stateChanged);
            Assert.AreEqual(State.Stopped, statemachine.CurrentState);
        }

        [Test]
        public void StateMachine_SendTriggerAsync_NotAllowedTrigger_ThrowsInvalidTriggerException()
        {
            var statemachine = StateMachine.SetupNew<State, Trigger>()
                .AddState(State.Scheduled, (b) => b.Allow(t => t.TransitionTo(State.Running).On(Trigger.Start)))
                .FinishSetup(State.Scheduled);

            Assert.ThrowsAsync<InvalidTriggerException<State, Trigger>>(() =>
            {
                return statemachine.SendTriggerAsync(Trigger.Stop);
            });
        }

        [Test]
        public async Task StateMachine_SendTriggerAsync_IgnoredTrigger_NoException()
        {
            var statemachine = StateMachine.SetupNew<State, Trigger>()
                .AddState(State.Scheduled, (b) => b.Allow(t => t.TransitionTo(State.Running).On(Trigger.Start))
                                                   .Ignore(Trigger.Stop))
                .FinishSetup(State.Scheduled);

            var stateChanged = await statemachine.SendTriggerAsync(Trigger.Stop);
            Assert.IsFalse(stateChanged);
            Assert.AreEqual(State.Scheduled, statemachine.CurrentState);
        }

        [Test]
        public async Task StateMachine_SendTriggerAsync_MultipleTriggerPaths_CorrectPathTaken()
        {
            bool isStopped = false;

            var statemachine = StateMachine.SetupNew<State, Trigger>()
                .AddState(State.Scheduled, (b) => b.Allow(t => t.TransitionTo(State.Running).On(Trigger.Start)))
                .AddState(State.Running, (b) => b.Allow(t => t.TransitionTo(State.Stopping).On(Trigger.Stop))
                                                 .Allow(t => t.TransitionTo(State.Pauzed).On(Trigger.Pauze)))
                .AddState(State.Stopping, (b) => b.Allow(t => t.TransitionTo(State.Stopped).On(Trigger.Stop).When(() => isStopped)))
                .AddState(State.Stopped, (b) => b.Allow(t => t.TransitionTo(State.Running).On(Trigger.Start)))
                .AddState(State.Pauzed, (b) => b.Allow(t => t.TransitionTo(State.Running).On(Trigger.Start))
                                                .Allow(t => t.TransitionTo(State.Stopping).On(Trigger.Stop)))
                .FinishSetup(State.Scheduled);

            await statemachine.SendTriggerAsync(Trigger.Start);
            Assert.AreEqual(State.Running, statemachine.CurrentState);

            await statemachine.SendTriggerAsync(Trigger.Pauze);
            Assert.AreEqual(State.Pauzed, statemachine.CurrentState);

            await statemachine.SendTriggerAsync(Trigger.Start);
            Assert.AreEqual(State.Running, statemachine.CurrentState);

            await statemachine.SendTriggerAsync(Trigger.Pauze);
            Assert.AreEqual(State.Pauzed, statemachine.CurrentState);

            await statemachine.SendTriggerAsync(Trigger.Stop);
            Assert.AreEqual(State.Stopping, statemachine.CurrentState);
        }

        [Test]
        public async Task StateMachine_SendTriggerAsync_ReentryWithDuplicateTrigger_TakeFirstAllowedTrigger()
        {
            int reentries = 0;

            var statemachine = StateMachine.SetupNew<State, Trigger>()
                .AddState(State.Scheduled, (b) => b.Allow(t => t.TransitionTo(State.Running).On(Trigger.Start)))
                .AddState(State.Running, (b) => b.Allow(t => t.TransitionTo(State.Stopping).On(Trigger.Stop))
                                                 .Allow(t => t.TransitionTo(State.Pauzed).On(Trigger.Pauze)))
                .AddState(State.Stopping, (b) => b.Allow(t => t.TransitionTo(State.Stopped).On(Trigger.Stop).When(() => reentries > 2))
                                                  .Allow(t => t.TransitionTo(State.Stopping).On(Trigger.Stop))
                                                  .OnEnter(() => reentries++))
                .AddState(State.Stopped, (b) => b.Allow(t => t.TransitionTo(State.Running).On(Trigger.Start)))
                .AddState(State.Pauzed, (b) => b.Allow(t => t.TransitionTo(State.Running).On(Trigger.Start))
                                                .Allow(t => t.TransitionTo(State.Stopping).On(Trigger.Stop)))
                .FinishSetup(State.Scheduled);

            await statemachine.SendTriggerAsync(Trigger.Start);
            Assert.AreEqual(State.Running, statemachine.CurrentState);

            await statemachine.SendTriggerAsync(Trigger.Stop);
            Assert.AreEqual(State.Stopping, statemachine.CurrentState);
            Assert.AreEqual(1, reentries);

            await statemachine.SendTriggerAsync(Trigger.Stop);
            Assert.AreEqual(State.Stopping, statemachine.CurrentState);
            Assert.AreEqual(2, reentries);

            await statemachine.SendTriggerAsync(Trigger.Stop);
            Assert.AreEqual(State.Stopping, statemachine.CurrentState);
            Assert.AreEqual(3, reentries);
            
            await statemachine.SendTriggerAsync(Trigger.Stop);
            Assert.AreEqual(State.Stopped, statemachine.CurrentState);
            Assert.AreEqual(3, reentries);
        }

    }
}
