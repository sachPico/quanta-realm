using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sachet.Experimental
{
    public class ConditionalState<T>
    {
        public State<T> state;
        public Predicate<T> check;

        public ConditionalState(State<T> _state, Predicate<T> _check)
        {
            state = _state;
            check = _check;
        }
    }

    public class State<T>
    {
        public string name;
        public T relatedObject
        {
            get;
            set;
        }

        public Action OnEnter;
        public Action OnExit;
        public Action OnFixedUpdate;
        public Action OnUpdate;

        #region CONSTRUCTOR

        public State()
        {

        }

        public State(T id)
        {
            relatedObject = id;
        }

        public State(T _id, string _name) : this(_id)
        {
            name = _name;
        }

        public State(T _id, Action _onEnter, Action _onExit = null, Action _onFixedUpdate = null, Action _onUpdate = null) : this(_id)
        {
            OnEnter = _onEnter;
            OnExit = _onExit;
            OnFixedUpdate = _onFixedUpdate;
            OnUpdate = _onUpdate;
        }

        public State(T _id, string _name, Action _onEnter, Action _onExit = null, Action _onFixedUpdate = null, Action _onUpdate = null) : this(_id, _name)
        {
            OnEnter = _onEnter;
            OnExit = _onExit;
            OnFixedUpdate = _onFixedUpdate;
            OnUpdate = _onUpdate;
        }
        #endregion

        #region METHODS
        #region VIRTUALS
        public virtual void Enter()
        {

        }

        public virtual void Exit()
        {

        }

        public virtual void FixedUpdate()
        {

        }

        public virtual void Update()
        {

        }
        #endregion
        #endregion
    }

    public class GFSM<T>
    {
        protected List<State<T>> states;
        protected Dictionary<int, Dictionary<int, Predicate<T>>> flow = new Dictionary<int, Dictionary<int, Predicate<T>>>();
        protected State<T> activeState;
        protected State<T> defaultStart;

        public Action OnFixedUpdate;
        public Action OnUpdate;

        public T relatedObject
        {
            get;
            set;
        }

        int currentId;
        
        #region CONSTRUCTOR
        public GFSM() {}

        public GFSM(T _id)
        {
            states = new List<State<T>>();

            currentId = 0;
        }

        public GFSM(State<T>[] _ids)
        {
            states = new List<State<T>>();
            foreach(var i in _ids)
            {
                AddState(i);
            }
            states[0].Enter();

            currentId = 0;
        }
        #endregion

        #region METHODS
        public void AddFlow(int aid, int bid, Predicate<T> condition)
        {
            if(aid!=bid)
            {
                if (flow.ContainsKey(aid))
                {
                    if (!flow[aid].ContainsKey(bid))
                    {
                        flow[aid].Add(bid, condition);
                    }
                }
                else
                {
                    flow.Add(aid, new Dictionary<int, Predicate<T>>());
                    flow[aid].Add(bid, condition);
                }
            }
        }

        public void AddState(State<T> _state)
        {
            if(states.Count==0)
            {
                defaultStart = _state;
                activeState = _state;
            }
            states.Add(_state);
        }

        public void RemoveState(State<T> _state)
        {
            states.Remove(_state);
        }

        public void Next()
        {
            State<T> result = null;
            int aid = GetActiveStateId();

            foreach (var p in flow[aid].Keys)
            {
                if(flow[aid][p].Invoke(relatedObject))
                {
                    result = states[p];
                }
            }

            SetActiveState(result);
        }

        public int GetActiveStateId()
        {
            return states.FindIndex(x => x == activeState);
        }

        public State<T> GetActiveState()
        {
            return activeState;
        }

        public State<T> GetState(int _stateIndex)
        {
            if(_stateIndex > 0 && _stateIndex < states.Count)//states.ContainsKey(_stateID))
            {
                return states[_stateIndex];
            }
            return null;
        }

        private void SetActiveState(State<T> _state)
        {
            if(activeState == _state)
            {
                return;
            }

            if(activeState != null)
            {
                activeState.Exit();
            }

            activeState = _state;

            if(activeState != null)
            {
                activeState.Enter();
            }
        }

        public void Restart()
        {
            SetActiveState(defaultStart);
        }

        public void FixedUpdate()
        {
            if(activeState!=null)
            {
                activeState.FixedUpdate();
                OnFixedUpdate?.Invoke();
            }
        }

        public void Update()
        {
            if(activeState!=null)
            {
                activeState.Update();
                OnUpdate?.Invoke();
            }
        }
        #endregion
    }
}