// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Animator")]
	[Tooltip("Sets the value of a float parameter")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1066")]
	public class SetAnimatorFloat : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The target. An Animator component and a PlayMakerAnimatorProxy component are required")]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("The animator parameter")]
		public FsmString parameter;
		
		[Tooltip("The float value to assign to the animator parameter")]
		public FsmFloat Value;
		
		[Tooltip("Optional: The time allowed to parameter to reach the value. Requires everyFrame Checked on")]
		public FsmFloat dampTime;
		
		[Tooltip("Repeat every frame. Useful when changing over time.")]
		public bool everyFrame;
		
		private PlayMakerAnimatorMoveProxy _animatorProxy;
		private Animator _animator;
		private int _paramID;
		
		public override void Reset()
		{
			gameObject = null;
			parameter = null;
			dampTime = null;
			Value = null;
			everyFrame = false;
		}
		
		public override void OnEnter()
		{
			// get the animator component
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			
			if (go==null)
			{
				Finish();
				return;
			}
			
			_animator = go.GetComponent<Animator>();
			
			if (_animator==null)
			{
				Finish();
				return;
			}
			
			_animatorProxy = go.GetComponent<PlayMakerAnimatorMoveProxy>();
			if (_animatorProxy!=null)
			{
				_animatorProxy.OnAnimatorMoveEvent += OnAnimatorMoveEvent;
			}
			
			// get hash from the param for efficiency:
			_paramID = Animator.StringToHash(parameter.Value);
			
			SetParameter();
			
			if (!everyFrame) 
			{
				Finish();
			}
		}
	
		public void OnAnimatorMoveEvent()
		{
			if (_animatorProxy!=null)
			{
				SetParameter();
			}
		}	
		
		public override void OnUpdate() 
		{
			if (_animatorProxy==null)
			{
				SetParameter();
			}
		}
		
		void SetParameter()
		{		
			if (_animator!=null)
			{
				if (dampTime.Value>0f)
				{
					_animator.SetFloat(_paramID,Value.Value,dampTime.Value,Time.deltaTime);
				}else{
					_animator.SetFloat(_paramID,Value.Value) ;
				}
			}
		}
		
		
	}
}