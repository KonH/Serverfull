using UnityEngine;
using UnityEngine.AI;
using Serverfull.Views;
using Serverfull.Models;

namespace Serverfull.Game {
	[RequireComponent(typeof(NavMeshAgent))]
	public class EngineerUnit : MonoBehaviour {
		abstract class State {
			protected EngineerUnit _owner;
			protected NavMeshAgent _agent;

			public State(EngineerUnit owner) {
				_owner = owner;
				_agent = owner._agent;
			}

			public abstract void Update();

			public bool IsDone() {
				return (_agent.remainingDistance < 0.1f);
			}
		}

		class IdleState : State {
			public IdleState(EngineerUnit owner) : base(owner) {
				GoToRandomPoint();
			}

			void GoToRandomPoint() {
				var start = _owner.transform.position;
				var randomPoint = start + Random.insideUnitSphere * _owner.WalkRange;
				NavMeshHit hit;
				if ( NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas) ) {
					_agent.SetDestination(hit.position);
					return;
				}
				_agent.SetDestination(start);
			}

			public override void Update() {
				if ( IsDone() ) {
					GoToRandomPoint();
				}
			}
		}

		class MoveToServerState : State {
			ServerView _target;

			public MoveToServerState(EngineerUnit owner, ServerView target) : base(owner) {
				_target = target;
				GoToTarget();
			}

			void GoToTarget() {
				Debug.Assert(_agent);
				Debug.Assert(_target);
				Debug.Assert(_target.WorkPoint);
				_agent.SetDestination(_target.WorkPoint.position);
			}

			public override void Update() {
				if ( IsDone() ) {
					_owner.StartFixServer(_target);
				}
			}
		}

		class FixState : State {
			ServerView _target;
			float      _time;
			float      _timer;

			public FixState(EngineerUnit owner, ServerView target, float time) : base(owner) {
				_target = target;
				_timer  = 0.0f;
				_time   = time;
			} 

			public override void Update() {
				_timer += Time.deltaTime;
				if ( _timer > _time ) {
					_owner.DoneFixServer(_target);
				}
			}
		}

		public float WalkRange;

		public EngineerId Id     { get; private set; }
		public bool       IsBusy { get; private set; }

		EngineerManager _manager;

		NavMeshAgent _agent;
		State        _state;

		public void Init(EngineerManager manager, EngineerId id) {
			_manager = manager;
			Id = id;
		}

		void Start() {
			TryInit();
		}

		void TryInit() {
			if ( !_agent ) {
				_agent = GetComponent<NavMeshAgent>();
			}
			if ( _state == null ) {
				_state = new IdleState(this);
			}
		}

		void Update() {
			_state.Update();
		}

		public void GoToFixServer(ServerView server) {
			TryInit();
			IsBusy = true;
			_state = new MoveToServerState(this, server);
		}

		void StartFixServer(ServerView server) {
			_state = new FixState(this, server, _manager.GetFixTime(Id));
		}

		void DoneFixServer(ServerView server) {
			_state = new IdleState(this);
			_manager.DoneFixServer(Id, server);
			IsBusy = false;
		}
	}
}
