using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsBase
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // this is to move in the x direction
        desiredx = 0;
        if (Input.GetAxis("Horizontal") > 0) desiredx = 3;
        if (Input.GetAxis("Horizontal") < 0) desiredx = -3;
    
        // we could do the same with desiredy or we can be lazy.
        if(Input.GetButton("Jump") && grounded) velocity.y = 6.5f; // so now if you jump, we put the velocity in the positive y direction to jump
    }

    public void Collide(Collider2D other) 
    {
        // other is a game object, and we want to know if it is colliding with an emnemy or not
        if (other.gameObject.CompareTag("Lethal"))
        {
            Debug.Log("Dead");
        }
    }

    public override void CollideHorizontal(Collider2D other)
    {
       Collide(other);
    }

    public override void CollideVertical(Collider2D other)
    {
       Collide(other);
    }
}
