using UnityEngine;
using System.Collections;

[RequireComponent (typeof (PlayerController))]
public class Player : LivingEntity {

	public float moveSpeed = 10f;

	PlayerController controller;
    GunController gunController;

    Camera viewCamera;

	// Use this for initialization
    protected override void Start () 
    {   
        base.Start();
        viewCamera = Camera.main;
		controller = GetComponent<PlayerController> ();
        gunController = GetComponent<GunController>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        // Movement Input
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        controller.Move(moveVelocity);

        // Look Input
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            //Debug.DrawRay(ray.origin, point, Color.red);
            controller.LookAt(point);
        }

        // Weapon Input
        if (Input.GetButton("Fire1"))
        {
            gunController.Shoot();
        }

        // Change Weapon
        if (Input.GetKeyDown(KeyCode.Alpha1))
            gunController.EquipGun(1);
        else  if (Input.GetKeyDown(KeyCode.Alpha2))
            gunController.EquipGun(2);

	}
}
