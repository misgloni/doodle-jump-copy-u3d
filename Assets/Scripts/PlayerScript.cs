using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerScript : MonoBehaviour
{
    public float currentJumpSpeed = 5f;

    public float jumpSpeed = 10f;
    public float springJumpSpeed = 20f;
    public float springBedJumpSpeed = 30f;
    public float gASpeed = -13f; //重力加速度
    public float leftRightSpeed = 3f;
    public Transform 瞬移线左;
    public Transform 瞬移线右;
    public float 最大攻击角度 = 30f;
    public float 最高速度 = 20f;
    private float moveDirection = 0f;
    private bool wasFly = false;
    private float _acceleration; //当前加速度

    public float 飞行器加速度 = 13f;
    public float 飞行器时间 = 5f;

    public float 火箭加速度 = 13f;
    public float 火箭时间 = 5f;

    public GameObject baseGameObject;
    public GameObject jumpGameObject;
    public GameObject baseAttackGameObject;
    public GameObject jumpAttackGameObject;
    public GameObject 精灵;
    public GameObject mouth;
    public GameObject 飞行器;
    public GameObject 火箭;
    public GameObject 子弹;
    public GameObject[] objs = new GameObject[4];
    public float 攻击间隔 = 1f;
    private float attackCd = 0;
    public Transform 生成物容器;


    private bool wasBase = true; //true是伸脚状态, false缩脚
    private bool wasAttack = false;
    private float _attackAngle;
    private bool _canAttack = true;
    private float defaultScaleX;

    // Start is called before the first frame update
    void Start()
    {
        // _animator = GetComponent<Animator>();
        objs[0] = baseGameObject;
        objs[1] = jumpGameObject;
        objs[2] = baseAttackGameObject;
        objs[3] = jumpAttackGameObject;
        _acceleration = gASpeed;
        defaultScaleX = transform.localScale.x;
    }

    void ChangeAnimation(short num)
    {
        for (short i = 0; i < objs.Length; i++)
        {
            if (i == num)
            {
                objs[i].SetActive(true);
            }
            else
            {
                objs[i].SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump("springBed");
        }

        if (Input.GetMouseButton(0))
        {
            Attack(Vector2.SignedAngle(new Vector2(0, 1),
                Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position));
        }
        else
        {
            wasAttack = false;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Fly();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Jet();
        }

        moveDirection = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        //移动
        transform.position += Time.deltaTime * currentJumpSpeed * Vector3.up;
        // GetComponent<Rigidbody2D>().velocity = Vector2.up * currentJumpSpeed;
        
        //加速度修正
        float tempSpeed = currentJumpSpeed + _acceleration * Time.deltaTime;
        if (tempSpeed > 最高速度)
        {
            currentJumpSpeed = 最高速度;
        }
        else
        {
            currentJumpSpeed = tempSpeed;
        }

        //左右移动
        transform.position += Vector3.right * moveDirection * leftRightSpeed * Time.deltaTime;

        //动画变形
        if (moveDirection > 0)
        {
            // transform.rotation = new Quaternion(0, -180, 0, 0);
            transform.localScale = new Vector3(-defaultScaleX, transform.localScale.y, transform.localScale.z);
        }
        else if (moveDirection < 0)
        {
            // transform.rotation = new Quaternion(0, 0, 0, 0);
            transform.localScale = new Vector3(defaultScaleX, transform.localScale.y, transform.localScale.z);
        }

        if (transform.position.x > 瞬移线右.position.x)
        {
            transform.position = new Vector3(瞬移线左.position.x, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < 瞬移线左.position.x)
        {
            transform.position = new Vector3(瞬移线右.position.x, transform.position.y, transform.position.z);
        }

        //动画脚更新
        // _animator.SetFloat("CurrentJumpSpeed",currentJumpSpeed);
        wasBase = !(currentJumpSpeed > 0);


        //动画配置
        if (wasBase)
        {
            if (wasAttack)
            {
                ChangeAnimation(2);
            }
            else
            {
                ChangeAnimation(0);
            }
        }
        else
        {
            if (wasAttack)
            {
                ChangeAnimation(3);
            }
            else
            {
                ChangeAnimation(1);
            }
        }

        if (wasAttack)
        {
            mouth.SetActive(true);
            mouth.transform.rotation = Quaternion.Euler(0, 0, _attackAngle);
        }
        else
        {
            mouth.SetActive(false);
        }

        //攻击时间积累
        attackCd += Time.deltaTime;
    }


    void Jump(string type)
    {
        if (type.Equals("base"))
        {
            currentJumpSpeed = jumpSpeed;
        }
        else if (type.Equals("spring"))
        {
            currentJumpSpeed = springJumpSpeed;
        }
        else if (type.Equals("springBed"))
        {
            currentJumpSpeed = springBedJumpSpeed;
        }
    }

    void Attack(float angle)
    {
        if (!_canAttack)
        {
            return;
        }

        Debug.Log("attack");
        wasAttack = true;
        if (angle < -最大攻击角度)
        {
            angle = -最大攻击角度;
        }
        else if (angle > 最大攻击角度)
        {
            angle = 最大攻击角度;
        }

        Debug.Log("angle" + angle);
        _attackAngle = angle;
        //创建子弹

        if (attackCd > 攻击间隔)
        {
            attackCd = 0;
            GameObject tempObj = Instantiate(子弹, 子弹.transform.position, 子弹.transform.rotation, 生成物容器);
            tempObj.SetActive(true);
            tempObj.GetComponent<BulletScript>().Launch();
        }
    }

    void Fly()
    {
        wasFly = true;
        飞行器.SetActive(true);
        currentJumpSpeed = 0;
        飞行器.GetComponent<Animator>().SetTrigger("Fly");
        _acceleration = 飞行器加速度;
        _canAttack = false;
        StartCoroutine(FlyTimer());
    }

    void Jet()
    {
        wasFly = true;
        火箭.SetActive(true);
        currentJumpSpeed = 0;
        火箭.GetComponent<Animator>().SetTrigger("Fly");
        _acceleration = 火箭加速度;
        _canAttack = false;
        StartCoroutine(JetTimer());
    }

    IEnumerator FlyTimer()
    {
        Debug.Log("飞行用Timer开始计时");
        yield return new WaitForSeconds(飞行器时间);

        //结束
        wasFly = false;
        _acceleration = gASpeed;
        飞行器.GetComponent<Animator>().SetTrigger("Stop");
        GameObject temp = Instantiate(飞行器, 飞行器.transform.position, 飞行器.transform.rotation, 生成物容器);
        飞行器.SetActive(false);
        temp.GetComponent<DropItemScript>().Drop();
        _canAttack = true;
        Debug.Log("飞行时间结束");
    }

    IEnumerator JetTimer()
    {
        Debug.Log("火箭用Timer开始计时");
        yield return new WaitForSeconds(火箭时间);

        //结束
        wasFly = false;
        _acceleration = gASpeed;
        火箭.GetComponent<Animator>().SetTrigger("Stop");
        yield return new WaitForSeconds(0.5f);

        GameObject temp = Instantiate(火箭, 火箭.transform.position, 火箭.transform.rotation, 生成物容器);
        火箭.SetActive(false);
        temp.GetComponent<DropItemScript>().Drop();
        _canAttack = true;
        Debug.Log("火箭时间结束");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (currentJumpSpeed < 0)
        {
            //小于0才会触发
            if (other.tag.Equals("Tile"))
            {
                Jump("base");
            }
            else if (other.tag.Equals("Spring"))
            {
                Jump("spring");
                other.GetComponent<Animator>().SetTrigger("Play");
            }
            else if (other.tag.Equals("SpringBed"))
            {
                Jump("springBed");
                other.GetComponent<Animator>().SetTrigger("Play");
                Rotating();
            }
        }
    }

    private void Rotating()
    {
        //跳跃到停下需要的时间
        float t = 1;

        float rotate = 360f;
        float angularV = rotate / t;
        // 精灵.GetComponent<Rigidbody2D>().angularVelocity = angularV;
        StartCoroutine(RotateTimer(t,angularV));
    }

    private float ro_time = 0;
    IEnumerator RotateTimer(float time, float angularV)
    {
        ro_time = 0;
        _canAttack = false;
        for (;;)
        {
            ro_time += Time.deltaTime;
            if (ro_time > time)
            {
                _canAttack = true;
                yield break;
            }
            else
            {
                Vector3 vector3 = 精灵.transform.rotation.eulerAngles;
                精灵.transform.rotation = Quaternion.Euler(vector3.x,vector3.y,vector3.z+angularV*Time.deltaTime);
                yield return 0;
            }
        }
    }
}