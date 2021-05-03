using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public GameObject fishPrefab; //Os peixes que serão pré-fabricados
    public int numFish = 20; //O numero de peixes
    public GameObject[] allFish; //A quantidade de peixes
    public Vector3 swinLimits = new Vector3(5, 5, 5); //O limite a qual o peixe irá nadar

    [Header("Configurações do Cardume")]
    [Range(0.0f, 5.0f)] //Alcançe do peixe
    public float minSpeed; //A velocidade mínima do peixe
    [Range(0.0f, 0.5f)] //Alcançe do peixe
    public float maxSpeed; //A velocidade máxima do peixe

    // Start is called before the first frame update
    void Start()
    {
        allFish = new GameObject[numFish]; //A quantidade de peixes que serão mostradas
        for(int i = 0; i < numFish; i++)
        {
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x), //O limite a qual o peixe vai percorrer através dos eixos x, y e z aleatóriamente
                                                                Random.Range(-swinLimits.y, swinLimits.y), 
                                                                Random.Range(-swinLimits.z, swinLimits.z));
            allFish[i] = (GameObject) Instantiate(fishPrefab, pos, Quaternion.identity); //O clone a qual o obejeto sera criado
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
