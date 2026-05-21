using UnityEngine;
using System.Collections.Generic;
using System;

public class Unit : MonoBehaviour {
    
    public Vector2 movement;
    public float distance;
    public event Action<float> OnMove;

    void FixedUpdate() {
        Move(new Vector2(movement.x, 0) * Time.fixedDeltaTime);
        Move(new Vector2(0, movement.y) * Time.fixedDeltaTime);
        distance += movement.magnitude*Time.fixedDeltaTime;
        if (distance > 0.5f)
        {
            OnMove?.Invoke(distance);
            distance = 0;
        }
    }

    public void Move(Vector2 ds) {
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        int n = GetComponent<Rigidbody2D>().Cast(ds, hits, ds.magnitude * 2);
        if (n == 0)
        {
            transform.Translate(ds);
        }
    }
    
    public void SetMovement(Vector2 ds) {
        bool movingPreviously = movement.magnitude >= 0.000001;
        bool newMovement = ds.magnitude >= 0.000001;

        movement = ds;

        if (movingPreviously && !newMovement) {
            EventBus.Instance.DoMovementStopped(this);
        } else if (!movingPreviously && newMovement) {
            EventBus.Instance.DoMovementStarted(this);
        }
    }

}

