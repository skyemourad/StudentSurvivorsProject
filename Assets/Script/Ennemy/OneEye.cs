using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneEye : Ennemy
{
    enum OneEyeState
    {
        Idling,
        Walking,
        Attacking,
        Attacking2
    }
    OneEyeState currentState = OneEyeState.Walking;
    Animator animator;
    float waitTime = 2f;
    [SerializeField] GameObject handObject;
    [SerializeField] GameObject fireballObject;
    int randomNumber = 1;
    [SerializeField] BoxCollider2D boxCollider;
    public Material material;

    protected override void Start()
    {
        base.Start();
        material = spriteRenderer.material;
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        switch (currentState)
        {
            case OneEyeState.Idling:
                waitTime -= Time.deltaTime;
                if (waitTime <= 0)
                {
                    currentState = OneEyeState.Walking;
                    randomNumber = Random.Range(1, 3);
                }
                break;
            case OneEyeState.Walking:
                base.Update();
                float distance = Vector3.Distance(transform.position, player.transform.position);
                animator.SetBool("isWalking", true);
                if(randomNumber == 1 && distance < 1f)
                {
                    currentState = OneEyeState.Attacking;
                }
                if(randomNumber == 2 && distance < 3f)
                {
                    currentState = OneEyeState.Attacking2;
                }
                break;
            case OneEyeState.Attacking:
                animator.SetBool("isWalking", false);
                animator.SetTrigger("Attack");
                StartCoroutine(ShaderCoroutine2());
                waitTime = 5f;
                currentState = OneEyeState.Idling;
                break;
            case OneEyeState.Attacking2:
                animator.SetBool("isWalking", false);
                animator.SetTrigger("Attack2");
                StartCoroutine(ShaderCoroutine());
                waitTime = 6f;
                currentState = OneEyeState.Idling;
                break;
        }  
    }

    IEnumerator ShaderCoroutine()
    {
        for (float i = 0.8f; i > 0; i -= 0.01f)
        {
            material.SetFloat("_Flash", material.GetFloat("_Flash") + 0.01f);
            yield return new WaitForSeconds(0.01f);
        }
        for (float i = 0.8f; i > 0; i -= 0.01f)
        {
            material.SetFloat("_Flash", material.GetFloat("_Flash") - 0.01f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator ShaderCoroutine2()
    {
        for (float i = 0.8f; i > 0; i -= 0.01f)
        {
            material.SetFloat("_Flash2", material.GetFloat("_Flash2") + 0.01f);
            yield return new WaitForSeconds(0.01f);
        }
        for (float i = 0.8f; i > 0; i -= 0.01f)
        {
            material.SetFloat("_Flash2", material.GetFloat("_Flash2") - 0.01f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void SpawnHand()
    {
        Instantiate(handObject, new Vector3(player.transform.position.x, player.transform.position.y +1), Quaternion.identity);
    }

    public void TurnBoxColliderOn()
    {
        boxCollider.enabled = true;
    }

    public void TurnBoxColliderOff()
    {
        boxCollider.enabled = false;
    }
}
