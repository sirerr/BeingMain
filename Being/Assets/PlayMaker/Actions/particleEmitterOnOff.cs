using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Turns a Particle Emitter on and off with optional delay.")]
	public class particleEmitterOnOff : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The particle system component owner.")]
		public FsmOwnerDefault gameObject;
		[Tooltip("Set to True to turn it on and False to turn it off.")]
		public FsmBool emitOnOff;
		[Tooltip("If 0 it just acts like a switch. Values cause it to Toggle value after delay time (sec).")]
		public FsmFloat delay;
		[Tooltip("The event is fired after the delay has been reached. NOTE: only works when delay > 0")]
		public FsmEvent finishEvent;
		public bool realTime;
		
		private float startTime;
		private float timer;
		private ParticleSystem ps;
		private GameObject go;
		
		public override void Reset()
		{
			gameObject = null;
			emitOnOff = false;
			delay = 0f;
			finishEvent = null;
			realTime = false;
			ps = null;
		}
	
		public override void OnEnter()
		{
			ps = Fsm.GetOwnerDefaultTarget(gameObject).GetComponent<ParticleSystem>();
			
			if (emitOnOff.Value) ps.Play(); else ps.Stop();
			
			if (delay.Value <= 0)
			{
				Finish();
				return;
			}
			
			startTime = Time.realtimeSinceStartup;
			timer = 0f;
		}
		
		public override void OnUpdate()
		{
			// update time
			
			if (realTime)
			{
				timer = Time.realtimeSinceStartup - startTime;
			}
			else
			{
				timer += Time.deltaTime;
			}
			
			if (timer > delay.Value)
			{
				if (!emitOnOff.Value) ps.Play(); else ps.Stop();
				
				Finish();
				if(finishEvent != null) Fsm.Event(finishEvent);
			}
		}
		
	}
}