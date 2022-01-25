using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTemplate : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] public float WalkForce = 5f;
    [SerializeField] public float SprintForce = 8f;
    [SerializeField] public float JumpForce = 10f;
    [SerializeField] public float LadderVertical = 5f;
    [SerializeField] public float LadderHorizontal = 3f;
    [Header("Other")]
    [SerializeField] public float DoubleTapTime = 0.2f;
    [SerializeField] public float TimeInRoll = 0.5f;
    [SerializeField] public float ThrowJump = 5f;
    [Header("Gravity settings")]
    [SerializeField] public float ThrowJumpGravity = 2f;
    [SerializeField] public float NormalGravity = 2.5f;
    [SerializeField] public float FallGravity = 4.5f;
    [SerializeField] public float fallSpeed = 15f;
    [SerializeField] public float fallAcceleration = 20f;
    [SerializeField] public float fallDmg = 2f;
    [Header("Sounds")]
    [SerializeField] public AudioClip Jump;
    [SerializeField] public AudioClip Walk;
    [SerializeField] public AudioClip Run;
    [SerializeField] public AudioClip Reload;
    [SerializeField] public AudioClip emptySound;
    [SerializeField] public AudioClip getHit;
    [SerializeField] public AudioClip deathSound;
}
