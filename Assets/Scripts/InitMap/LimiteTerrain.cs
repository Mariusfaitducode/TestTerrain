using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LimiteTerrain : MonoBehaviour
{
    //private Personnage perso;
    //public GameObject player;

    public GameObject limiteCube;  //Prefab
    public Personnage personnage;
    public PlateauJeu plateau;
    public List <LimiteBlock> listCube = new List<LimiteBlock>();
    public struct LimiteBlock
    {
        public GameObject cube;
        public Vector2 tabRef;
        public bool active;
    }

    public int distance;
    // Start is called before the first frame update
    
    void Update()
    {
        //ActualizeBlock();
    }
    
    public void InitLimite()
    {
        //personnage = player.GetComponent<Personnage>();

        //int i = (int)personnage.ReturnCaseRef().x;

        int x = (int) personnage.ReturnCaseRef(0).x;
        int y = (int) personnage.ReturnCaseRef(0).y;
        
        int l_Xmax = x + distance;
        int l_Xmin = x - distance;
        int l_Ymax = y + distance;
        int l_Ymin = y - distance;

        for (int i = l_Xmin + 1; i <= l_Xmax - 1; i++)
        {
            AddBlock(i, l_Ymax);
            AddBlock(i, l_Ymin);
        }

        for (int j = l_Ymin + 1; j <= l_Ymax - 1; j++)
        {
            AddBlock(l_Xmax, j);
            AddBlock(l_Xmin, j);
        }
    }
    public void AddBlock(int x, int z) 
    {
        Vector3 position;
        bool canAdd = true;

        int taille = 100 - (int)plateau.grid[x,z].surfaceHeight;
            
        GameObject obj = Instantiate(limiteCube);

        position.x = (x * Constants.CaseSize) - Constants.MapSize_1 * Constants.CaseSize / 2 + Constants.CaseSize / 2;
        
        position.y = plateau.grid[x,z].surfaceHeight + taille / 2;
        
        position.z = (z * Constants.CaseSize) - Constants.MapSize_1 * Constants.CaseSize / 2 + Constants.CaseSize / 2;
            
        obj.transform.position = position;
            
        obj.transform.localScale = new Vector3(Constants.CaseSize, taille, Constants.CaseSize);

        LimiteBlock newBlock;
        newBlock.cube = obj;
        newBlock.tabRef = new Vector2(x, z);
        newBlock.active = true;

        foreach (LimiteBlock block in listCube)
        {
            if (block.tabRef.x == newBlock.tabRef.x && block.tabRef.y == newBlock.tabRef.y)
            {
                canAdd = false;
            }
        }

        if (canAdd)
        {
            listCube.Add(newBlock);
        }
    }

    public void ActualizeBlock()
    {
        int vision = 50;
        
        
        List <LimiteBlock> listRemove = new List<LimiteBlock>();

        //foreach (LimiteBlock block in listCube)
            
        for (int a = 0; a<listCube.Count; a++)
        {
            LimiteBlock block = listCube[a];
            Vector2 ecart;

            ecart.x = block.cube.transform.position.x - personnage.ReturnCase(0).caseCube.transform.position.x;
            ecart.y = block.cube.transform.position.z - personnage.ReturnCase(0).caseCube.transform.position.z;
            
            //print(ecart.magnitude);
            int refX = (int)block.tabRef.x;
            int refY = (int)block.tabRef.y;

            if (ecart.magnitude < vision)
            {
                print("taille list = " + listCube.Count);
                listRemove.Add(block);

                //var block = enumerator.Current;
                //item = Value;
                
                //Destroy(block.cube);
                block.cube.SetActive(false);
                block.cube = default;
                block.active = false;
                
                for (int i = refX-1; i < refX + 2; i++)
                {
                    for (int j = refY-1; j < refY + 2; j++)
                    {
                        ecart.x = plateau.grid[i, j].caseCube.transform.position.x -
                                  personnage.ReturnCase(0).caseCube.transform.position.x;
                        
                        ecart.y = plateau.grid[i, j].caseCube.transform.position.z -
                                  personnage.ReturnCase(0).caseCube.transform.position.y;
                        
                        print(i + "//"+j);

                        if (ecart.magnitude > vision)
                        {
                            AddBlock(i, j);
                        }
                    }
                }
            }
        }
    }
}
