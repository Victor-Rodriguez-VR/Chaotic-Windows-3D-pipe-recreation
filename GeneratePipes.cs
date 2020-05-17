﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePipes : MonoBehaviour{

    public GameObject pipePrefab = null; // Our prefab in the Unity Editor's assets. Resembles a cylinder.
    public GameObject spherePrefab = null; // Our prefab in the Unity Editor's assets. Resembles a sphere.
    string[] variables = {"x","-x","z","-z", "y","-y",};
    private static Quaternion[] variableRotations={
        Quaternion.Euler(0f,0f,-90f),
        Quaternion.Euler(0f,0f,90f),
        Quaternion.Euler(90f,0f,0f),
        Quaternion.Euler(-90f,0f,0f),
		Quaternion.Euler(0f,180f,0f), 
        Quaternion.Euler(180f,0f,0f)
    };



    // Start is called before the first frame update
    void Start(){
        continuePipe(spawnAPrefabSomewhere(), 5);      
    }


    // Update is called once per frame
    void Update(){

    }

    public GameObject spawnAPrefabSomewhere(){
        Vector3 spawnLocation = new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-4.5f, 5.0f) , 0);
        GameObject objec = Instantiate(pipePrefab, spawnLocation, Quaternion.Euler(0f,180f,0f));
        objec.GetComponent<Renderer>().material.color = new Color(Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f));
        return objec;
    }

    public void continuePipe(GameObject previousPipe, int previousDirection){ 
        if(outOfBounds(previousPipe.transform)){
            return;
        }
        int direction = randomTransform(previousDirection);
        Color parentColor = previousPipe.GetComponent<Renderer>().material.color;
        if(changesDirection() && previousDirection != direction){ 
                GameObject spherey = Instantiate(spherePrefab, previousPipe.transform.position + previousPipe.transform.up, Quaternion.Euler(0f, 0f, 0f));
                spherey.transform.SetParent(previousPipe.transform, true);
                spherey.GetComponent<Renderer>().material.color = parentColor;
                GameObject piper = Instantiate(pipePrefab, spherey.transform.position + newDirection(spherey, direction), rotation(direction));
                piper.transform.SetParent(spherey.transform, true);
                piper.GetComponent<Renderer>().material.color = parentColor;
                continuePipe(piper, direction);

        }
        else{
            GameObject piper = Instantiate(pipePrefab,previousPipe.transform.position + previousPipe.transform.up, Quaternion.Euler(previousPipe.transform.eulerAngles.x, previousPipe.transform.eulerAngles.y, previousPipe.transform.eulerAngles.z));
            piper.transform.SetParent(previousPipe.transform, true);
            piper.GetComponent<Renderer>().material.color = parentColor;
            continuePipe(piper, previousDirection);
        }

    }
    /*
        Returns a string of a direction (x or y or z)

        @param previousDirection - 
    */
    public int randomTransform(int lastIndex){
        int idk = Random.Range(0,6);
        int modMe = 1;
        if(lastIndex %2 == 1){
            modMe = -1;
        }
        if( (lastIndex+modMe) != idk){
            return idk;
        }
        return randomTransform(lastIndex);
    }

    /*
        Determines a new direction.
        @param previousObject - the previous object we'd like to get a new direction from.
        @param randomTransform - a string (x or y or z) that tell us which direction our object will go.

        @return a Vector3 that determines in which direction the pipe will go.
    */
    public Vector3 newDirection(GameObject previousObject, int randomTransform){
        int rotation180 = 1;
        if(randomTransform % 2 == 1 ){
            rotation180 = -1;
        }
        if(randomTransform < 2){
            return previousObject.transform.right * rotation180;
        }
        else if (randomTransform < 4 ){
            return previousObject.transform.forward* rotation180; 
        }
        else{
            return previousObject.transform.up * rotation180;
        }

    }
   
    /*
        Returns eulerAngles (x,y,z) in Quaternion form based on axis of rotation.

        @param rotationAxis - the desired axis of rotation (ex: from our randomTransform we got x so we are going to rotate around x).
    */
   public Quaternion rotation(int variable){
       return variableRotations[variable];
   }

   public bool changesDirection(){
       int randomNumber = Random.Range(0,1000);
       if(randomNumber > 400){
           return true;
       }
       return false;
   }

   public bool outOfBounds(Transform pipe){
       if(pipe.position.x <-10 || pipe.position.x > 10 || pipe.position.y <-10 || pipe.position.y > 10 || pipe.position.z <-10 || pipe.position.z > 10){
           Debug.Log("I stopped");
           return true;
       }
       return false;
   }

   public bool alreadyFilled(GameObject pipe, Vector3 direction ){
       if(Physics.CheckSphere(pipe.transform.position + direction, 0.5f )){
           Debug.Log("IT WAS FILLED AT " + pipe.transform.position + direction );
           return true;
       }
       return false;
   }
}

