using UnityEngine;
using System.Collections;

public class Shell : MonoBehaviour {

    public float maxForce;
    public float minForce;

    Rigidbody myRigid;

    float lifeTime = 4;
    float fadeTime = 2;


	// Use this for initialization
	void Start () {
        myRigid = GetComponent<Rigidbody>();

        float force = Random.Range(minForce, maxForce);
        myRigid.AddForce(transform.right * force);
        myRigid.AddTorque(Random.insideUnitSphere * force);

        StartCoroutine(Fade());
	}

    IEnumerator Fade(){
        yield return new WaitForSeconds(lifeTime);

        Material mat = GetComponent<Renderer>().material;
        Color initialColor = mat.color;

        float percent = 0f;
        float fadeSpeed = 1 / fadeTime;

        while (percent <= 1f)
        {
            percent += Time.deltaTime * fadeSpeed;
            mat.color = Color.Lerp(initialColor, Color.clear, percent);

            yield return null;
        }

        Destroy(gameObject);
    }
	
}
