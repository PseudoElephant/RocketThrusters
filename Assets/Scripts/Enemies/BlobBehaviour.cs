using System.Collections;

using UnityEngine;
using Utility;

public class BlobBehaviour : MonoBehaviour
{
    public int attackRadius;
    public GameObject target;
    public float speed;
    public LayerMask groundMask;
    public int timeBetweenAttacks = 2;
    public int stickDisabledFrames = 5;
    
    private JelloBody _body;
    private bool _grounded = false;
    private bool _canAttack = true;
    private Rigidbody2D _rigidbody2D;
    private Sticky _sticky;
    
    void Start()
    {
        SectionManager.instance.SetTarget(ref target);
        
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _sticky = GetComponent<Sticky>();
        _body = GetComponent<JelloBody>();
        _body.JelloCollisionEvent += ProcessCollisionEvent;
        
    }

    
    
    IEnumerator Attack()
    {
        _canAttack = false;
        yield return new WaitForSeconds(timeBetweenAttacks);
        UpdateGroundState();
        
        // Direction
        if (_grounded && PlayerIsInRange())
        {

          StartCoroutine(RemoveJointsForFrames(stickDisabledFrames));
          yield return new WaitForFixedUpdate();
          //  RemoveAllJoints();
            Vector2 direction = (target.transform.position - transform.position).normalized;
            _body.AddForce(direction * speed);
            
          
        }
        _canAttack = true;

      
        
    
    }

    private void UpdateGroundState()
    {
        foreach (var collision in _body.previousCollisions)
        {
            foreach (var contact in collision.contacts)
            {
                if (IsInGroundLogic(contact)) return;
            }
        }

        _grounded = false;
    }

    private void RemoveAllJoints()
    {
        for (int i = 0; i < _sticky.joints.Count; i++)
        {
            _sticky.joints[i].Destroy();
        }
    }

    IEnumerator RemoveJointsForFrames(int frames)
    {
        for (int i = 0; i < frames; i++)
        {
            yield return new WaitForFixedUpdate();
            RemoveAllJoints();
            
        }
    }
    private bool PlayerIsInRange()
    {
        Vector2 dir = (target.transform.position - transform.position);
        return dir.magnitude < attackRadius;
    }
    private void ProcessCollisionEvent(JelloCollision jelloCollision)
    {
        
        //grounded logic
        if(!_grounded || _canAttack) {
            //loop through each contact in the collision.
            for (int i = 0; i < jelloCollision.contacts.Length; i++)
            {
                JelloContact contact = jelloCollision.contacts[i];

               

                bool isMask = MathUtility.BinHasInPos( groundMask.value,contact.colliderB.gameObject.layer) || 
                              MathUtility.BinHasInPos( groundMask.value,contact.colliderA.gameObject.layer);
                
                if (isMask)
                {
                    _grounded = true;
                    // Get out of loop
                    StartCoroutine(Attack());
                    return;
                }
                _grounded = false;
                return;
            }

        }
    }


    private bool IsInGroundLogic(JelloContact contact)
    {
        bool isMask = MathUtility.BinHasInPos( groundMask.value,contact.colliderB.gameObject.layer) || 
                      MathUtility.BinHasInPos( groundMask.value,contact.colliderA.gameObject.layer);
                
        if (isMask)
        {
            _grounded = true;
            // Get out of loop
            return true;
        }
        _grounded = false;
        return false;
    }

}
