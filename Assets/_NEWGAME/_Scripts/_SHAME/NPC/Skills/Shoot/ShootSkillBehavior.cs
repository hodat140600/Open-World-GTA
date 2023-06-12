using Assets._SDK.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootSkillBehavior : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Animator _animator;

    private float shootingDistance = 100f;
    private float shootingRange = 12f;
    private float timebtwShoot = 1f;


    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    Coroutine shootCoroutine;
    Coroutine distanceDetectCoroutine;

    public void ActiveShoot(Transform target)
    {
        StartCoroutine(CheckDistanceRoutine(target));
    }
    public void StartShoot(Transform target)
    {
        _agent.isStopped = true;
        _animator.SetBool("fire", true);
        transform.LookAt(target);
        transform.eulerAngles = new Vector3(0, transform.transform.eulerAngles.y + 30, 0);

        if (shootCoroutine == null)
        {
            shootCoroutine = StartCoroutine(ShootRoutine(target));
        }
    }
    public void StopShoot()
    {
        _animator.SetBool("fire", false);
        _agent.isStopped = false;
        if (shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
            shootCoroutine = null;
        }        
    }

    IEnumerator CheckDistanceRoutine(Transform target)
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            // Rotate
            transform.LookAt(target);
            
            if (Vector3.Distance(target.position, transform.position) < shootingRange)
            {
               
                StartShoot(target);
                
            }
            else
            {
                StopShoot();
               
            }
        }
    }
    IEnumerator ShootRoutine(Transform target)
    {
        while (true)
        {            
            yield return new WaitForSeconds(timebtwShoot);
            Shoot(target);
        }
    }
    public void Shoot(Transform target)
    {
        _agent.isStopped = true;

        // Set animation
        SetAnimShoot();

        RaycastHit rayCastHit;

        Vector3 direction = (target.transform.position - transform.position).normalized;
        if (Physics.Raycast(transform.position, direction, out rayCastHit, shootingDistance))
        {
            Debug.Log("Shooting : " + rayCastHit.transform.name);
            Debug.DrawRay(transform.position, transform.forward * shootingDistance, Color.yellow);

            // ----- weapon flaze ------
            /*for (int i = 0; i < npcClassRef.aIData.npcWeaponManager.weapon.flazeFX.transform.childCount; i++)
            {
                if (npcClassRef.aIData.npcWeaponManager.weapon.flazeFX.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>() != null)
                {
                    npcClassRef.aIData.npcWeaponManager.weapon.flazeFX.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>().Play();
                }
            }*/

            // CharacterClass player = rayCastHit.transform.gameObject.GetComponent<CharacterClass>();
            // if (player != null)
            // {
            //     player.gameObject.GetComponent<PlayerHealth>().PlayerHitDamage(5f);
            //     //GameObject effect = Instantiate(bloodEffect, rayCastHit.point, Quaternion.LookRotation(rayCastHit.normal));
            //     //Destroy(effect, 1);
            // }

        }
    }

    public void LevelUp(ISkillLevel skill)
    {

    }
    void SetAnimShoot()
    {
        _animator.SetBool("walk", false);
        _animator.SetBool("fire", true);

    }

    #region Methods 
    public bool CanShoot(float distance)
    {
        return distance <= shootingRange;
    }
    #endregion


}
