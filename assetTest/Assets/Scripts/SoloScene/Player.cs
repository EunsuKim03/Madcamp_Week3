using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float mouseX = 0f;
    float mouseSpeed = StartSceneManager.mouseSpeed;

    public Animator playerAnimator;

    public GameObject bulletFactory; // 총알이 생성되는 장소 (총알)
    public Transform firePosition; // 총알을 생성하여 위치시키는 장소 (총구)

    public GameObject gunFire; // 총구 화염    

    private int shotCounter = 0;
    public int shotDelay = 20;

    float reloadTime; // 장전하느라 소비 중인 시간

    public GameObject meteorSpawn;
    
    // Sound
    public AudioSource shotSound;


    // Start is called before the first frame update
    void Start() {
        // psBullet = bulletEffect.GetComponent<ParticleSystem>();
        // asBullet = bulletEffect.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() 
    { 
        mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
        transform.eulerAngles = new Vector3(0, mouseX, 0); 

        // 마우스 왼쪽 버튼 클릭 시 총알이 날라간다.
        if (!GameManager.isReloading && shotCounter == 0 && Input.GetMouseButtonDown(0)) {
            playerAnimator.SetBool("fire", true);
            shotBullet();
            GameManager.bulletCount--;
            shotCounter = 1;
        } else if (!GameManager.isReloading && Input.GetMouseButton(0) && shotCounter == shotDelay) {
            shotBullet();
            GameManager.bulletCount--;
            shotCounter = 1;
        }

        if (!GameManager.isReloading && Input.GetMouseButtonUp(0)) {
            playerAnimator.SetBool("fire", false);
        }

        if (shotCounter == 60) shotCounter = 0;
        if (shotCounter > 0) shotCounter++;

        // 마우스 오른쪽 버튼 클릭 시 히트스캔 방식이 적용된다.
        if (!GameManager.isReloading && Input.GetButtonDown("Fire2")) {
            shotHitScan();
        }

        // R 버튼을 누르거나 장탄 수가 0이 되면 재장전이 된다.
        if (!GameManager.isReloading && 
            GameManager.bulletCount != GameManager.bulletCountLimit &&
            Input.GetKeyDown(KeyCode.R) || GameManager.bulletCount == 0) 
        {
            GameManager.isReloading = true;
        } 
    }

    // 투사체 방식으로 날라가는 총알
    public void shotBullet() {
        gunFire.SetActive(true);
        shotSound.Play();
        Invoke("DisableGunFire", 0.5f);
        
        // bulletFactory에서 bullet을 하나 생성한다.
        GameObject bullet = Instantiate(bulletFactory, firePosition.position, Quaternion.identity);

        // 생성된 bullet의 위치를 총구 위치로 지정한다.
        // 총알이 날라가는 방향을 총구의 방향과 일치시킨다.
        // bullet.transform.position = firePosition.position;
        bullet.transform.forward = firePosition.forward;
    }

    // 히트스캔 방식으로 처리됨
    public void shotHitScan() {
        // Ray: 카메라의 시선, 바라보는 방향
        // 레이의 위치와 방향을 카메라의 위치와 forward 방향으로 설정한다. 
        // (즉 현재의 화면 정중앙 및 내가 바라보는 방향)
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        
        // RaycastHit: Ray가 닿았다면 닿은 위치에 대한 충돌 정보를 저장한다.
        RaycastHit hitInfo = new RaycastHit();

        // ray가 날라가는 궤적을 원점으로부터 거리 20만큼 1초 동안 파란색으로 나타낸다.
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 20, Color.blue, 1f);

        // 만약 ray를 쏴서 어떤 대상에 부딪혔다면, hitInfo에 충돌 정보를 저장한다.
        if (Physics.Raycast(ray, out hitInfo)) {
            // ray에 맞은 대상의 정보를 불러와서 값을 바꿀 수 있다.
            // hitInfo.transform.GetComponent<MeshRenderer>().material.color = Color.red;
            
            // 이펙트가 발생할 지점을 타격 지점으로 설정한다.
            // bulletEffect.transform.position = hitInfo.point;
            // bulletEffect.transform.forward = hitInfo.normal; // 부딪힌 표면의 법선 방향으로 설정

            // 타격 이펙트 및 사운드 재생
            // psBullet.Play();
            // asBullet.Play();
        }
    }


    private void DisableGunFire()
    {
        // 이펙트를 포함한 게임 오브젝트 비활성화
        gunFire.SetActive(false);
    }

}