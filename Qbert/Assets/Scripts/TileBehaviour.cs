using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileBehaviour : MonoBehaviour {

    private List<Material> tileMaterials = new List<Material>();

    public Material mat1;
    public Material mat2;
    public Material mat3;
    public Material mat4;

    private Renderer rend;
    private int materialIndex = 0;
    private int maxMaterial = 0;

    GameController controller;

    // Use this for initialization
    void Start () {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        //add all the tile materials to the List<Material>
        tileMaterials.Add(mat1);
        tileMaterials.Add(mat2);
        tileMaterials.Add(mat3);
        tileMaterials.Add(mat4);

        rend = GetComponent<Renderer>();
        rend.material = tileMaterials[materialIndex]; //set tile to initial material
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    //When a player lands on a tile
    void OnTriggerEnter(Collider other)
    {
        if (materialIndex < controller.getLevel()) //If the tile isnt at its max material for the level
        {
            materialIndex++; //increment the index of material the tile is on
            rend.material = tileMaterials[materialIndex]; //apply new material to tile
            controller.AddScore(materialIndex); //increment the score based on the material index
            controller.IncrementTileChange(); //Tell the controller that a tile has been changed
        }
    }
}
