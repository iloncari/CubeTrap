﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMoving : MonoBehaviour
{
    public bool isUsingLevelLoad = false;
    public float moveSpeed;
    public GameObject deathParticles;
    public bool usesManager = true;
    public GameManager manager;

    private Vector3 input;
    private float maxSpeed = 5f;
    private Rigidbody rigidbody;
    private Vector3 spawn;
    private AudioSource audio;
    private string[] clips = { "Sounds/level_win", "Sounds/coin_win", "Sounds/die_sound"};

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        spawn = transform.position;
        if(usesManager)
            manager = manager.GetComponent<GameManager>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        rigidbody = GetComponent<Rigidbody>();
        if(rigidbody.velocity.magnitude < maxSpeed)
        {
            rigidbody.AddForce(input * moveSpeed);
        }
        if(transform.position.y < -1)
        {
            Die();
        }
        if (isUsingLevelLoad)
        {
            if (transform.position.x < -15.92 || transform.position.x > 15.10 || transform.position.z < -2.84 || transform.position.z > 13.70)
                gameObject.GetComponent<Rigidbody>().useGravity = true;
            else
            {
                gameObject.GetComponent<Rigidbody>().position = new Vector3(transform.position.x, 0.35f, transform.position.z);
                gameObject.GetComponent<Rigidbody>().useGravity = false;
            }
        }
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag == "Enemy")
        {
            Die();
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Goal")
        {
            PlaySound(0);
            Time.timeScale = 0f;
            if (usesManager)
                manager.CompleteLevel();      
        }
        if (other.transform.tag == "Enemy")
        {
            Die();
        }
        if (other.transform.tag == "Token")
        {
            PlaySound(1);
            if (usesManager)
                manager.tokenCount +=1;
            Destroy(other.gameObject);
        }
    }
    void PlaySound(int clipIndex)
    {
        audio.clip = Resources.Load<AudioClip>(clips[clipIndex]);
        audio.Play();
    }

    private void Die()
    {
        PlaySound(2);
        PlaySound(2);
        Instantiate(deathParticles, transform.position, Quaternion.Euler(270,0,0));
        transform.position = spawn;
    }
}
