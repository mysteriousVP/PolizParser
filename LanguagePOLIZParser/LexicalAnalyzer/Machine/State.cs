using System.Collections.Generic;

namespace LexicalAnalyzer
{
    class State
    {
        public int Id { get; }

        private readonly Dictionary<int, State> transitions;
        private State otherwise;

        public State(int id, bool isFinal = false, bool takeCharacter = true)
        {
            this.transitions = new Dictionary<int, State>();

            this.Id = id;
            this.IsFinal = isFinal;
            this.TakeCharacter = takeCharacter;
        }

        public State ConfigureTransition(int charClass, State nextState)
        {
            if (!CanTransit(charClass))
            {
                transitions.Add(charClass, nextState);
            }

            return this;
        }
        public State ConfigureSelfTransition(int charClass)
        {
            if (!CanTransit(charClass))
            {
                transitions.Add(charClass, this);
            }

            return this;
        }

        public State ConfigureOtherwiseSelfTransition()
        {
            otherwise = this;
            return this;
        }

        public State ConfigureOtherwiseTransition(State state)
        {
            otherwise = state;
            return this;
        }

        public State Transit(int charClass)
        {
            return CanTransit(charClass) ? transitions[charClass] : otherwise;
        }

        private bool CanTransit(int charClass)
        {
            return transitions.ContainsKey(charClass);
        }

        public bool IsError => this.GetType() == typeof(CustomErrorHandler);
        public bool IsFinal { get; }
        public bool TakeCharacter { get;}
    }
}