using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBase : MonoBehaviour
{
    // can use unity vector 2 or 3. at some point you need to convert the vector 2 to vector 3. since unity positions are 3 dimensional.
    // but its easier to use vector 2 internally  
    public Vector2 velocity;
    public float gravityFactor; // turn gravity on or off. make objects stronger or weaker
    public float desiredx;
    public bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        


    }

    // if our movement code detects that we collide with anything, then call these methods. and if any subclass wants to do anything, say the enemy wants to detect when its colliding with a wall to turn around, it just changes direction.
    // of if the player jumps, or collides with an enemy, in this method, it can basically say player is dead if it collides with enemy 
    public virtual void CollideHorizontal(Collider2D other) 
    {

    }

    public virtual void CollideVertical(Collider2D other) 
    {

    }

    // method basically says, if we're colliding with something that has a normal vector value of a point somewhere towards up or down, then there will be a collision
    void Movement(Vector2 move, bool horizontal)
    {
        // need to ask player if it moves, if it collides with anything
        // takes care of movement and checks collision. this is good for any object that needs movement
        // gets vector 2 and tells it how much it should move
        if (move.magnitude < 0.000001f) return; // super slow speed
        grounded = false;
        bool collision = false;

        // check for collision
        RaycastHit2D[] hits = new RaycastHit2D[16];
        int cnt = GetComponent<Rigidbody2D>().Cast(move, hits, move.magnitude + 0.01f);

        // CAST take current position of our game object, and then take its collider, and cast it in a direction and tell me if it would collide with anything. in that direction. 
        // colliding with something shouldnt move. and if it collides with something, we know what it collides with.
        // third parameter = how far it should check if theres object hit. unity default is inifnity checks all directions. to collide with anything in your game. infinity is not useful. so we will use our move magnitude + a little bit of force to make sure it works
        
        // so cast method, takes our circle, looks in the direction that we give it in the move direction for a distance equivalent to the size of the Move plus some slack. and then it tells us how many different things are we hitting in that direction.
        // if (cnt > 0) return; // hitting something
        
        
        for(int i = 0; i < cnt; ++i) // iterate over all of our collisions
        {   // for each of them if the normal vector points in the horinzontal direction then we hit a wall. length of normal vector is always 1.
            if (Mathf.Abs(hits[i].normal.x) > 0.3f && horizontal) {
                // colliding more with a wall than it is a floor
                // 0.3 instead of 0.1 since it could be like something thats a slope. by setting this value to different values, we can determine like how much of a slope is a barrier for us. which is kind of neat.
                // abs value of it since it can be pointing to the left or the right it is still a wall

                // if it collides
                // return;
                collision = true;
                CollideHorizontal(hits[i].collider);
                // call this method and then whoever wants to handle it, override it.
            }

            // opposite for vertical. if we're not moving horizontally
            if (Math.Abs(hits[i].normal.y) > 0.3f && !horizontal) {
                // if what we hit has a normal vector that points upwards, then we will say that we are on the ground.  
                if (hits[i].normal.y > 0.3f) grounded = true;
                collision = true;
                CollideVertical(hits[i].collider);
                // return; // then we're going to not move
            }

        }


        if (collision) return;

        // if we move, change the transform position with the move
        // not hitting anything then move game object
        transform.position += (Vector3)move;
    }

    // Update is called once per frame
    // however, we are using FixedUpdate instead. 
    // 2 reasons
    // 1. we want to use our update for other things than the player
    // 2. the update is for every time a frame is drawn. fast game 60fps. slow game 5 fps. but is more useful if our physics simulation is more consistent. 
    // fixed update runs every 0.02 or 0.04 segment default. but can be changed if needed 
    void FixedUpdate()
    {
        Vector2 acceleration = 9.81f * Vector2.down*gravityFactor;
        
        // velocity is sum of all acceleration
        // using Time.fixedDeltaTime instead of regular Time.deltaTime
        velocity += acceleration * Time.fixedDeltaTime;
        velocity.x = desiredx; // before we actually move our object
        
        // spliting our move to two components. x and y
        Vector2 move = velocity * Time.fixedDeltaTime; // use this for both x and y
        Movement(new Vector2(move.x, 0), true);
        Movement(new Vector2(0, move.y), false);
        // now if you done everything right you will have collision
        // Movement(velocity * Time.fixedDeltaTime);
        // transform.position += (Vector3)velocity * Time.deltaTime;

        // transform position. basically every game object has a transform that contains various stuff. where is it, how big is it, and how is it working/walking? 
    }
}
