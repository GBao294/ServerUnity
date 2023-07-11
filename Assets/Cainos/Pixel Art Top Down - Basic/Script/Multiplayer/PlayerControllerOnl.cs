using Riptide;
using Riptide.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControllerOnl : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Rigidbody2D rb;
    //[SerializeField] private Transform camProxy;
    [SerializeField] private float movementSpeed;




    private Animator animator;


    private Vector2 moveDirection;

    private float moveSpeed;
    private bool[] inputs;
    public Transform Weapon;    
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 20f;


    private void OnValidate()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        if (player == null)
            player = GetComponent<Player>();
        Initialize();
    }
    public void Enabled(bool value)
    {
        enabled = value;
        rb.simulated = value;
    }

    private void Start()
    {
        if(animator==null)
            animator = GetComponent<Animator>();    
        inputs = new bool[5];

        


    }

    private void FixedUpdate()
    {
        //Debug.Log("W co " + inputs[0]);
        Vector2 inputDirection = Vector2.zero;
        if (inputs[0])
        {
            inputDirection.y += 1;
            animator.SetInteger("Direction", 1);
        }
        if (inputs[1])
        {
            inputDirection.y -= 1;
            animator.SetInteger("Direction", 0);
        }
        if (inputs[2])
        {
            inputDirection.x -= 1;
            animator.SetInteger("Direction", 3);
        }
        if (inputs[3])
        {
            inputDirection.x += 1;
            animator.SetInteger("Direction", 2);
        }
        //if (inputs[4])
        //    shoot(); 
        animator.SetBool("IsMoving", inputDirection.magnitude > 0);
        Move(inputDirection);
       

    }
    //void x()
    //{
    //    player.list.Count = 0;
        
    //}
    public void shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        SendShootNotification();
    }
    private void SendShootNotification()
    {
        // Tạo thông điệp phản hồi bắn
        Message message = Message.Create(MessageSendMode.Unreliable, (ushort)ServerToClientId.shootNotification);
        message.AddUShort(player.Id);

        // Gửi thông điệp phản hồi cho client
        NetworkManager.Singleton.Server.SendToAll(message);
    }



    private void Move(Vector2 inputDirection)
    {
        moveDirection = inputDirection.normalized * movementSpeed ; //move
        rb.velocity = moveDirection;
        SendMovement();
    }

   
    private void Initialize()
    {

        moveSpeed = movementSpeed * Time.fixedDeltaTime;

    }

    public void SetInput(bool[] inputs,Quaternion rot)
    {
            this.inputs = inputs;
            Weapon.rotation = rot;
       
    }


    private void SendMovement()
    {
        if (NetworkManager.Singleton.CurrentTick % 2 != 0) return;

        Message message = Message.Create(MessageSendMode.Unreliable, ServerToClientId.playerMovement);
        message.AddUShort(player.Id);
        message.AddUShort(NetworkManager.Singleton.CurrentTick);
        message.AddVector2(transform.position);
        message.AddQuaternion(Weapon.rotation);
      //  Debug.Log("x" + transform.position.x + "y " + transform.position.y);

        NetworkManager.Singleton.Server.SendToAll(message);
    }

  
}