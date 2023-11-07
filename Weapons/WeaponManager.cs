using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Fire Rate")]
    [SerializeField] float fireRate;
    [SerializeField] bool semiAuto;
    float fireRateTimer;

    [Header("Bullet Properties")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform barrelPos;
    [SerializeField] float bulletVelocity;
    [SerializeField] int bulletsPerShot;
    public float damage = 20;
    AimStateManager aim;

    [SerializeField] AudioClip gunShot;
    [HideInInspector] public AudioSource audioSource;
    [HideInInspector] public WeaponAmmo ammo; //reference to the weapon_ammo class
    WeaponBloom bloom;
    ActionStateManager actions; //reference for actionstatemanager to fix the shooting sound plays when reloading 
    WeaponRecoil recoil;

    Light muzzleFlashLight;
    ParticleSystem muzzleFlashParticles;
    float lightIntensity;
    [SerializeField] float lightReturnSpeed=20;

    public float enemyKickbackForce = 10;

    public Transform leftHandTarget, leftHandHint;
    WeaponClassManager weaponClass; 

    // Start is called before the first frame update
    void Start()
    {
        
        aim = GetComponentInParent<AimStateManager>();
        
        bloom = GetComponent<WeaponBloom>();
        actions = GetComponentInParent<ActionStateManager>();
        muzzleFlashLight = GetComponentInChildren<Light>();
        lightIntensity = muzzleFlashLight.intensity;
        muzzleFlashLight.intensity = 0;
        muzzleFlashParticles = GetComponentInChildren<ParticleSystem>();
        fireRateTimer = fireRate;
    }

    private void OnEnable()
    {
        if(weaponClass == null)
        {
            weaponClass = GetComponentInParent<WeaponClassManager>();
            ammo = GetComponent<WeaponAmmo>();
            audioSource = GetComponent<AudioSource>();
            recoil = GetComponent<WeaponRecoil>();
            recoil.recoilFollowPos = weaponClass.recoilFollowPos;
        }
        weaponClass.SetCurrentWeapon(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (ShouldFire())
        {
            Fire();
        }

        //Debug.Log(ammo.currentAmmo);//prints current ammo everytime we shoot as a console message

        muzzleFlashLight.intensity = Mathf.Lerp(muzzleFlashLight.intensity, 0, lightReturnSpeed * Time.deltaTime);
    }

    bool ShouldFire()
    {
        fireRateTimer += Time.deltaTime;

        //Incrementing the fire rate timer
        //In the shouldFire method, fireRateTimer is incremented by the time that has passed since the last frame, using Time.deltaTime.
        //This means that with every frame update, fireRateTimer increases.
        //It represents the time that has passed since the last shot was fired.

        if (fireRateTimer < fireRate)
        {
            return false;
        }

        if (ammo.currentAmmo == 0) //when the current clip is over the shooting stops
        {
            return false;
        }

        if (actions.currentState == actions.Reload) //fire is prohibited when reloading
        {
            return false;
        }

        if (actions.currentState == actions.Swap)
        {
            return false;
        }

        if (semiAuto && Input.GetKeyDown(KeyCode.Mouse0)) //Weapon is fully automatic
        {
            return true;
        }

        if (!semiAuto && Input.GetKey(KeyCode.Mouse0))
        {
            return true;
        }

        return false;
    }

    void Fire()
    {
        fireRateTimer = 0;

        //Debug.Log("Fire");
        //The Debug.Log("Fire") statement is used to output a message to the Unity console when the fire function is called.
        //This is often used for debugging purposes to confirm that the fire function has been triggered

        ammo.currentAmmo--; //decrement the ammo everytime we shoot

        barrelPos.LookAt(aim.aimPos);
        barrelPos.localEulerAngles = bloom.BloomAngle(barrelPos);

        audioSource.PlayOneShot(gunShot);
        TriggerMuzzleFlash();
        recoil.TriggerRecoil();
        
        for(int i =0; i < bulletsPerShot; i++)  //creating bullet objects for a shot
        {
            GameObject currentBullet = Instantiate(bullet, barrelPos.position, barrelPos.rotation);

            Bullet bulletScript = currentBullet.GetComponent<Bullet>();
            bulletScript.weapon = this;

            bulletScript.dir = barrelPos.transform.forward;

            Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
            rb.AddForce(barrelPos.forward * bulletVelocity, ForceMode.Impulse); //applies impulse force to each of the bullets to make them move towards the forward direction
        }
    }

    void TriggerMuzzleFlash()
    {
        muzzleFlashParticles.Play();
        muzzleFlashLight.intensity = lightIntensity;
    }
}
