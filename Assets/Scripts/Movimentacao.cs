using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimentacao : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body; //pr usar o rigidbody temos q fazer ref a ele
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCD;
    private float horizontalInput;

    //Awake e chamado cada vez que o script e carregado
    private void Awake(){
        body = GetComponent<Rigidbody2D>();      
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
       // playerCtrl = GetComponent<PlayerCtrl>();
    }

    private void Update(){
        horizontalInput = Input.GetAxis("Horizontal");

        //virar personagem
        if(horizontalInput > 0.01f){
            transform.localScale = new Vector3(4, 4, 4);
            }else if(horizontalInput < -0.01f){
                transform.localScale = new Vector3(-4, 4, 4);
        }
        
        //parametros da animacao
        anim.SetBool("Run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        if(wallJumpCD > 0.2f){
        //pulo            
            body.velocity = new Vector2(horizontalInput*speed, body.velocity.y);

            if(onWall() && !isGrounded()){
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }else{
                body.gravityScale = 7;

            if(Input.GetKey(KeyCode.Space) && isGrounded())
                Jump();  
            }
        }else{
            wallJumpCD += Time.deltaTime;
        }
    }

    private void Jump(){
        if(isGrounded()){
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("jump");
        }else if(onWall() && !isGrounded()){
            if(horizontalInput == 0){
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x)* 20, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }else{
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x)* 20, 12);
            }
            wallJumpCD = 0;
            
        }
        
    }
    //detecta colisao
    private void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.CompareTag("Move")){
            this.transform.parent = col.transform;
        } 
    }

    private void OnCollisionExit2D(Collision2D col){
        if(col.gameObject.CompareTag("Move")){
            this.transform.parent = null;
        }
    }

    private bool isGrounded(){
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall(){
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
}
