using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;



/**
 * This class demonstrates the CaveGenerator on a Tilemap.
 * 
 * By: Erel Segal-Halevi
 * Since: 2020-12
 */

public class TilemapCaveGenerator: MonoBehaviour {

    [SerializeField] Tilemap tilemap = null;
    [SerializeField] GameObject player = null;
    [SerializeField] GameObject portal = null;
    [SerializeField] GameObject chaser = null;
    private int posxp = 1000;
    private int posyp = 1000;
    private int posxg = -1;
    private int posyg = -1;
    

    [Tooltip("The tile that represents a wall (an impassable block)")]
    [SerializeField] TileBase wallTile1 = null;
    [SerializeField] TileBase wallTile2 = null;

    [Tooltip("The tile that represents a floor (a passable block)")]
    [SerializeField] TileBase floorTile1 = null;
   

    [Tooltip("The percent of walls in the initial random map")]
    [Range(0, 1)]
    [SerializeField] float randomFillPercent = 0.5f;

    [Tooltip("Length and height of the grid")]
    [SerializeField] int gridSize = 100;

    [Tooltip("How many steps do we want to simulate?")]
    [SerializeField] int simulationSteps = 20;

    [Tooltip("For how long will we pause between each simulation step so we can look at the result?")]
    [SerializeField] float pauseTime = 1f;

   

    private CaveGenerator caveGenerator;

    void Start()  {
       

        //To get the same random numbers each time we run the script
        Random.InitState(100);
        
        caveGenerator = new CaveGenerator(randomFillPercent, gridSize);
        caveGenerator.RandomizeMap();
        
        //For testing that init is working
        GenerateAndDisplayTexture(caveGenerator.GetMap());
            
        //Start the simulation
        StartCoroutine(SimulateCavePattern());
       
        
        
        
    }


    //Do the simulation in a coroutine so we can pause and see what's going on
    private IEnumerator SimulateCavePattern()  {
        for (int i = 0; i < simulationSteps; i++)   {
            yield return new WaitForSeconds(pauseTime);

            //Calculate the new values
            caveGenerator.SmoothMap();

            //Generate texture and display it on the plane
            GenerateAndDisplayTexture(caveGenerator.GetMap());
        }
        int[,] map = caveGenerator.GetMap();
        int z = GetPositionp(map);
        int t = GetPositiong(map);
        Vector3Int ploc = new Vector3Int(posxp, posyp, 0);
        Vector3Int gloc = new Vector3Int(posxg, posyg, 0);
        int e = gridSize / 2;
        int c = e;
        player.transform.position = tilemap.GetCellCenterWorld(ploc);
        portal.transform.position = tilemap.GetCellCenterWorld(gloc);
        for (int y = e; y < gridSize; y++)
        {
            if (map[y, e] == 0)
            {
                c = y;
                break;
            }
        }
        Vector3Int cloc = new Vector3Int(e,c,0);
        chaser.transform.position = tilemap.GetCellCenterWorld(cloc);
        Debug.Log("Simulation completed!");


        
    }

    private int GetPositionp(int[,] map)
    {
        for (int y = 0; y < gridSize; y++)
        {
            //print("\n");
            for (int x = 0; x < gridSize; x++)
            {

                //print(map[y, y]);
                if (map[x,y] == 0)
                {
                    posxp = System.Math.Min(posxp,x);
                    posyp = System.Math.Min(posyp, y);
                   
                    return 1;
                }
                
               
            }
        }
        return 1;
    }

    private int GetPositiong(int[,] map)
    {
        for (int y = gridSize-1; y > 0; y--)
        {
            //print("\n");
            for (int x = gridSize-1; x > 0; x--)
            {

                //print(map[y, y]);
                if (map[x, y] == 0)
                {
                    
                    posxg = System.Math.Max(posxg, x);
                    posyg = System.Math.Max(posyg, y);
                    return 1;
                }


            }
        }
        return 1;
    }

    //Generate a black or white texture depending on if the pixel is cave or wall
    //Display the texture on a plane
    private void GenerateAndDisplayTexture(int[,] data) {
        
        for (int y = 0; y < gridSize; y++) {
            for (int x = 0; x < gridSize; x++) {
                int chance = Random.Range(1, 3);
                var position = new Vector3Int(x, y, 0);
                if (x == 0 || x == gridSize - 1 || y == 0 || y == gridSize - 1) chance = 1;
                //var tile = data[x, y] == 1 ? wallTile: floorTile;
                //tilemap.SetTile(position, tile);
                if (data[x, y] == 0) tilemap.SetTile(position, floorTile1);
                else
                {
                    if (chance == 1) tilemap.SetTile(position, wallTile1);
                    if (chance == 2) tilemap.SetTile(position, wallTile2);
                    
                }


            }
        }
    }
}
