using UnityEngine;
using HutongGames.PlayMaker;

namespace DigitalBilby.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Transform)]
	[Tooltip("Get's the Direction (Forward, Up, Right) Vector of the Transform")]
	
	public class GetDirectionVector : FsmStateAction
	{
		
		public enum Direction
		{
			Forward, Up, Right
		}
		
		public Direction direction;
		
		
		[RequiredField]
		public FsmOwnerDefault gameObject;
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector;
		[UIHint(UIHint.Variable)]
		public FsmFloat x;
		[UIHint(UIHint.Variable)]
		public FsmFloat y;
		[UIHint(UIHint.Variable)]
		public FsmFloat z;
		public bool everyFrame;
		
		public override void Reset()
		{
			gameObject = null;
			vector = null;
			x = null;
			y = null;
			z = null;
			direction = GetDirectionVector.Direction.Forward;
			everyFrame = false;
			
		}
		
		public override void OnEnter()
		{
			DoGetForward();
			
			if (!everyFrame) Finish();
		}
		
		public override void OnUpdate()
		{
			DoGetForward();	
		}
		
		void DoGetForward()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			
			if (go == null) return;
			
			Vector3 directionVector = Vector3.zero;
		
			
			
			switch (direction) {
			case Direction.Forward:
				directionVector = go.transform.forward;
				break;
			case Direction.Up:
				directionVector = go.transform.up;
				break;
			case Direction.Right:
				directionVector = go.transform.right;
				break;
			}
			
			if (directionVector == Vector3.zero) return;
			
			vector.Value = directionVector;
			x.Value = directionVector.x;
			y.Value = directionVector.y;
			z.Value = directionVector.z;
		}
	}
	
}