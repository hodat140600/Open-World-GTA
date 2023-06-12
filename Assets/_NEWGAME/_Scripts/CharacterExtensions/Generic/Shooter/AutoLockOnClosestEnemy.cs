using _GAME._Scripts._Camera;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace _GAME._Scripts._CharacterController._Shooter
{
	[ClassHeader("Shooter Auto-Aim")]
	[RequireComponent(typeof(ExtendLockOn))]
	public class AutoLockOnClosestEnemy : MonoBehaviour
	{
		#region Variables
		public LayerMask EnemyLayers;
		[Range(1, 50)]
		public float AutoLockRadius = 5f;
		public float TimeBetweenTargetChange = 1f;
		protected float _currentChangeTime;
		public bool DebugShowRadius = false;

		private ThirdPersonCamera Camera;
		private ExtendLockOn Lock;

		protected Transform _currentEnemy;
		private Transform currentEnemy
		{
			set
			{
				// unlock old target
				if (_currentEnemy != null)
					Lock.onUnLockOnTarget.Invoke(_currentEnemy);

				// lock new target
				_currentEnemy = value;
				if (_currentEnemy != null)
				{
					// lock target
					Camera.SetLockTarget(_currentEnemy, 0f);
					Lock.onLockOnTarget.Invoke(_currentEnemy);
				}
				else
				{
					// no target locked
					Camera.RemoveLockTarget();
				}
			}
			get { return _currentEnemy; }
		}
		#endregion


		#region Main Methods
		void Awake()
		{
			Camera = ThirdPersonCamera.instance;
			Lock = this.GetComponent<ExtendLockOn>();
		}


		void Update()
		{
			_currentChangeTime += Time.deltaTime;
		}


		void FixedUpdate()
		{
			// get all enemies within our specified distance
			Collider[] enemies = Physics.OverlapSphere(transform.position + Vector3.up, AutoLockRadius, EnemyLayers);
			if (enemies.Length > 0)
			{
				// get closest enemy (order all enemies by distance and pick the first one)
				Collider closestEnemy = enemies.OrderBy(e => (e.transform.position - this.transform.position).sqrMagnitude).FirstOrDefault();
				if (closestEnemy != null)
				{
					// only assign, if it's not the same
					if (closestEnemy.transform != currentEnemy)
					{
						// can we already switch to a new target?
						if (_currentChangeTime >= TimeBetweenTargetChange)
						{
							// reset timer
							_currentChangeTime = 0f;

							// set new target
							currentEnemy = closestEnemy.transform;
						}
					}
				}
				else
				{
					currentEnemy = null;
				}
			}
			else
			{
				// no enemy found...reset
				if (currentEnemy != null)
					currentEnemy = null;
			}
		}
		#endregion


		#region Utility Methods
		void OnDrawGizmos()
		{
			if (!DebugShowRadius)
				return;

			// draw debug sphere
			Gizmos.color = Color.magenta;
			Gizmos.DrawWireSphere(transform.position + Vector3.up, AutoLockRadius);
		}
		#endregion
	}
}