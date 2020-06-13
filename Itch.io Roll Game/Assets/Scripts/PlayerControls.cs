using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public GameObject Planet;
    public GameObject PlayerPlaceholder;
    public Rigidbody RBD;

    public float speed = 4;
    public float JumpHeight = 1.2f;

    float gravity = 100;
    bool OnGround = false;


    float distanceToGround;
    Vector3 Groundnormal;
    Vector3 move;



    

    // Start is called before the first frame update
    void Start()
    {
        RBD = GetComponent<Rigidbody>();
        RBD.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {

        //MOVEMENT

        float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        Vector3 move = new Vector3(x, z, 0);

        RBD.AddForce(move * 100000 * Time.deltaTime);

        //Local Rotation

        if (Input.GetKey(KeyCode.E))
        {

            transform.Rotate(0, 150 * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.Q))
        {

            transform.Rotate(0, -150 * Time.deltaTime, 0);
        }

        //Jump

        if (Input.GetKeyDown(KeyCode.Space))
        {
            RBD.AddForce(transform.up * 40000 * JumpHeight * Time.deltaTime);

        }



        //GroundControl

        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -transform.up, out hit, 10))
        {

            distanceToGround = hit.distance;
            Groundnormal = hit.normal;

            if (distanceToGround <= 0.2f)
            {
                OnGround = true;
            }
            else
            {
                OnGround = false;
            }


        }


        //GRAVITY and ROTATION

        Vector3 gravDirection = (transform.position - Planet.transform.position).normalized;

        if (OnGround == false)
        {
            RBD.AddForce(gravDirection * -gravity);

        }

        //

        Quaternion toRotation = Quaternion.FromToRotation(transform.up, Groundnormal) * transform.rotation;
        transform.rotation = toRotation;



    }


    //CHANGE PLANET

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform != Planet.transform)
        {

            Planet = collision.transform.gameObject;

            Vector3 gravDirection = (transform.position - Planet.transform.position).normalized;

            Quaternion toRotation = Quaternion.FromToRotation(transform.up, gravDirection) * transform.rotation;
            transform.rotation = toRotation;

            RBD.velocity = Vector3.zero;
            RBD.AddForce(gravDirection * gravity);


            PlayerPlaceholder.GetComponent<TutorialPlayerPlaceholder>().NewPlanet(Planet);

        }
    }


}

