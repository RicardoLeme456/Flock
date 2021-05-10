using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockManager myManager; //Pegando a classe Flock Manager
    public float speed; //Velocidade do objeto
    bool turning = false; //A condição de que realmente se o peixe esta girando é verdadeiro ou falso

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed); //Posições aleatórias a qual ficara posicionado os peixes
    }

    // Update is called once per frame
    void Update()
    {
        //O limite ao qual o peixe ira percorrer pela área fazendo ele andar até um certo ponto e depois retorna ao ponto central
        Bounds b = new Bounds(myManager.transform.position, myManager.swinLimits * 2);

        RaycastHit hit = new RaycastHit(); //O raio a ser atingido
        Vector3 direction = myManager.transform.position - transform.position;

        if (!b.Contains(transform.position))
        {
            turning = true;
            direction = myManager.transform.position - transform.position;
        }
        else if(Physics.Raycast(transform.position, this.transform.forward * 50, out hit))
        {
            turning = true;
            direction = Vector3.Reflect(this.transform.forward, hit.normal); //Método usado para evitar do cardume de atravessar um objeto no cenário fazendo ele se desviar
        }
        else
            turning = false;
        

        if (turning)
        {
           transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
        }
        else
        {
            if (Random.Range(0, 100) < 10)
                speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
            if(Random.Range(0,100) < 20)
                ApplyRules();
        }
        transform.Translate(0, 0, Time.deltaTime * speed); //Mudança da posição de um lugar para outro
    }

    //Regras aplicadas ao qual o peixe irá atuar
    void ApplyRules()
    {
        GameObject[] gos;
        gos = myManager.allFish;

        Vector3 vcenter = Vector3.zero; //Posição central do eixo
        Vector3 vavoid = Vector3.zero; //Anulação da posição
        float gSpeed = 0.01f; //velocidade do grupo
        float nDistance; //Distancia percorrida
        int groupSize = 0; //Quantidade de grupos

        //O loop que fará o peixe se limitar através do ponto zero
        foreach(GameObject go in gos)
        {
            if(go != this.gameObject) //Se o game object go for diferente deste game object
            {
                nDistance = Vector3.Distance(go.transform.position, this.transform.position); //Inserindo o método Distance para ocorrer a mudança de posição do ponto A até o ponto B
                if(nDistance <= myManager.neighbourDistance) //Se o variável local nDistance for menor que o meu gerenciamento e a distancia do meu vizinho
                {
                    vcenter += go.transform.position; //Ocorre a mudança de posição do vcenter que é o ponto zero, pois ele está zerado na posição
                    groupSize++; //Acréscimo da quantidade de grupos

                    if (nDistance < 1.0) //Se nDistance for menor que 1
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position); //Ele evita do cardume se colidir com outro cardume se for menor que 1 de distância
                    }

                    Flock anotherFlock = go.GetComponent<Flock>(); //Pega o componente do Flock
                    gSpeed = gSpeed + anotherFlock.speed; //Calculo da velocidade do grupo com o speed da calsse Flock
                }    
            }
        }

        if(groupSize > 0) //Se a Qauntidade do grupo for maior que zero
        {
            vcenter = vcenter / groupSize + (myManager.goalPos - this.transform.position); //O grupo irá percorrer um tal limite de meta fazendo não se afastar do ponto central
            speed = gSpeed / groupSize; //O calculo da velocidade que meu grupo irá percorrer

            Vector3 direction = (vcenter + vavoid) - transform.position; //A posição que o meu peixe irá evitar em relação ponto central
            if (direction != Vector3.zero) //Se minha direção for diferente de zero do meu vetor
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime); //Ocorrerá a rotação suave dos cardumes através do método Slerp
        }
    }
}
