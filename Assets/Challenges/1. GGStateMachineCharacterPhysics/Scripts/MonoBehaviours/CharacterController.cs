using System;
using Challenges._1._GGStateMachineCharacterPhysics.Scripts.States;
using Cysharp.Threading.Tasks;
using GGPlugins.GGStateMachine.Scripts.Abstract;
using GGPlugins.GGStateMachine.Scripts.Data;
using GGPlugins.GGStateMachine.Scripts.Installers;
using UnityEditor;
using UnityEngine;
using Zenject;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Challenges._1._GGStateMachineCharacterPhysics.Scripts.MonoBehaviours
{
    [Serializable]
    public class CharacterMovementConfig
    {
        [SerializeField,Range(0,3)]
        private float characterRadius;
        [SerializeField,Range(0,8)]
        private float characterHeight;
        // u/s^2 = units per seconds squared
        [SerializeField,Range(0,20)][Tooltip("How quickly the character speeds up: u/s^2")]
        private float accelerationByTime;
        [SerializeField,Range(0,10)][Tooltip("Maximum speed: u/s")]
        private float maxSpeed;
        [SerializeField,Range(0.9f,1)][Tooltip("Multiplied with speed every frame ")]
        private float generalVelocityDamping;
        [SerializeField,Range(0.9f,1)][Tooltip("Multiplied with speed every frame only when there's input")]
        private float withInputVelocityDamping;
        [SerializeField,Range(0.9f,1)][Tooltip("Multiplied with speed every frame only when there's no input")]
        private float noInputVelocityDamping;
        [SerializeField,Range(0.9f,1)][Tooltip("Multiplied with speed every frame only when the character is above ground")]
        private float midAirXZVelocityDamping;
        [SerializeField,Min(0)][Tooltip("u/s^2")]
        private float gravity;

        
        
        public float Gravity => gravity;

        public float MidAirXZVelocityDamping => midAirXZVelocityDamping;

        public float NoInputVelocityDamping => noInputVelocityDamping;

        public float WithInputVelocityDamping => withInputVelocityDamping;

        public float GeneralVelocityDamping => generalVelocityDamping;

        public float MAXSpeed => maxSpeed;

        public float AccelerationByTime => accelerationByTime;

        public float CharacterHeight => characterHeight;

        public float CharacterRadius => characterRadius;
    }
    [ExecuteAlways]
    public class CharacterController : MonoBehaviour, IInputListener
    {
        [SerializeField]
        private CharacterMovementConfig characterMovementConfig;
        [SerializeField]
        private Transform headTransform;
        
        private GGStateMachineFactory _ggStateMachineFactory;
        private IGGStateMachine _stateMachine;

        [Inject]
        public void Inject(GGStateMachineFactory ggStateMachineFactory)
        {
            _ggStateMachineFactory = ggStateMachineFactory;
        }
        void Start()
        {
            if (!Application.isPlaying) return;
            CreateStateMachine();
            SetupStateMachineStates();
            foreach (var stateMachineUser in transform.GetComponentsInChildren<IStateMachineUser>())
            {
                stateMachineUser.SetStateMachine(_stateMachine);
            }
            _stateMachine.StartStateMachine<IdleState>();
        }

        private void OnDestroy()
        {
            if(Application.isPlaying)
                _stateMachine.RequestExit();
        }

        private void CreateStateMachine()
        {
            _stateMachine = _ggStateMachineFactory.Create();
            //We don't want the machine to leave a state and re-enter it.
            _stateMachine.SetSettings(new StateMachineSettings(true));
            _stateMachine.RegisterUniqueState(new FlowerEarnedState(transform, headTransform));
            _stateMachine.RegisterUniqueState(new IdleState(transform));
            _stateMachine.RegisterUniqueState(new MovState(transform));
        }

        #region EDIT
        // You should only need to edit in this region, you can add any variables you wish.
        
        private bool _isGrounded;
        private Vector3 _velocity;
        private float gravity;

        [SerializeField]
        private LayerMask _rayLayer;
        private Vector3 _rayPoint;
        
        //Add your states under this function
        private void SetupStateMachineStates()
        {
            _stateMachine.RegisterUniqueState(new ExampleState()).RegisterUniqueState(new ExampleParametrizedState(5f));
        }
        
        //Feel free to remove this
        private void ExampleStateSwitching()
        {
            _stateMachine.EnqueueState<ExampleParametrizedState,float>(1f);
            _stateMachine.EnqueueState<ExampleState>();
            // EnqueueState will queue up the states
            
            
            _stateMachine.SwitchToState<ExampleParametrizedState,float>(1f);
            _stateMachine.SwitchToState<ExampleState>();
            // SwitchToState will clear the current queue and add the input as next state (after the currently active one ends)
        }
        
        // CharacterInput.cs will call this function every frame in Update. xzPlaneMovementVector specifies the current input.
        // Ex:
        // (W is pressed) -> (0,1) ;
        // (W and D) -> (1,1) ;
        // (W and S) -> (0,0) ;
        // (A and S) -> (-1,-1) ;
        // (A) -> (-1,0)

         public void SetCurrentMovement(Vector2 xzPlaneMovementVector)
        {
            xzPlaneMovementVector *= characterMovementConfig.MAXSpeed;
            _velocity = new Vector3(xzPlaneMovementVector.x, 0, xzPlaneMovementVector.y) ;
            transform.position += _velocity * Time.deltaTime;

           
            Grounded();
            Grav();
            ControlStates();
        }
       
         private async UniTaskVoid Grounded()
         {
             Ray ray = new Ray(transform.TransformPoint(_rayPoint), Vector3.down);
             RaycastHit hit;

             if (Physics.SphereCast(ray, characterMovementConfig.CharacterRadius * 0.5f, out hit, 5f, _rayLayer))
             {
                 _isGrounded = true;

                 Vector3 nextPosition = new Vector3(transform.position.x, hit.point.y, transform.position.z);
                 transform.position = Vector3.Lerp(transform.position, nextPosition, characterMovementConfig.Gravity * Time.deltaTime);

                 return;
             }

             _isGrounded = false;
         }

         private async UniTaskVoid Grav()
         {
             if (_isGrounded)
                 gravity = 0;
             else
                 gravity = characterMovementConfig.Gravity;
         }
        private async UniTaskVoid ControlStates()
        {
            if (_velocity == Vector3.zero)
            {
                if (_stateMachine.CheckCurrentState<IdleState>()) return;
                _stateMachine.SwitchToState<IdleState>();
            }
            else
            {
                if (_stateMachine.CheckCurrentState<MovState>()) return;
                _stateMachine.SwitchToState<MovState>();
            }
        }
        
        #endregion
        
        
        private void OnDrawGizmosSelected()
        {
            Handles.color = Color.green;
            Handles.DrawWireDisc(transform.position,transform.up,characterMovementConfig.CharacterRadius);
            Handles.DrawWireDisc(transform.position+(transform.up*characterMovementConfig.CharacterHeight),transform.up,characterMovementConfig.CharacterRadius);
            for (int i = 0; i < 10; i++)
            {
                var angle = Mathf.PI*2 * ((i + 0f) / 10f);
                var x = Mathf.Cos(angle);
                var y = Mathf.Sin(angle);
                var localPos = new Vector3(x, 0, y)*characterMovementConfig.CharacterRadius;
                var localTargetPos = localPos + Vector3.up * characterMovementConfig.CharacterHeight;
                Handles.DrawLine(transform.TransformPoint(localPos),transform.TransformPoint(localTargetPos));
            }
          
        }
    }
}