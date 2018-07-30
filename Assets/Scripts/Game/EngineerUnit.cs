using UnityEngine;
using UnityEngine.AI;
using Serverfull.Views;
using Serverfull.Controllers;
using Zenject;

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
				_owner.TryGetWork();
			}
		}

		class MoveToServerState : State {
			ServerView _target;

			public MoveToServerState(EngineerUnit owner, ServerView target) : base(owner) {
				_target = target;
				GoToTarget();
			}

			void GoToTarget() {
				_agent.SetDestination(_target.transform.position);
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

		BreakController _break;
		ServerManager   _server;

		NavMeshAgent _agent;
		State        _state;

		[Inject]
		public void Init(BreakController breaking, ServerManager server) {
			_break  = breaking;
			_server = server;
		}

		void Start() {
			_agent = GetComponent<NavMeshAgent>();
			_state = new IdleState(this);
		}

		void Update() {
			_state.Update();
		}

		void TryGetWork() {
			foreach ( var server in _break.BreakedServers ) {
				var view = _server.GetView(server);
				if ( view != null ) {
					_state = new MoveToServerState(this, view);
					return;
				}
			}
		}

		void StartFixServer(ServerView server) {
			_state = new FixState(this, server, 1.5f);
		}

		void DoneFixServer(ServerView server) {
			_break.FixServer(server.Id);
			_state = new IdleState(this);
		}
	}
}
