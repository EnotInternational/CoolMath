using UnityEngine;

public class Wall : MonoBehaviour
{
     public GameObject block;
   [SerializeField]private int width = 10;
   [SerializeField]private int height = 4;
  
   void Start()
   {
       for (int y=0; y < height; ++y)
       {
           for (int x=0; x < width; ++x)
           {
               Instantiate(block, new Vector3(x,y,0), Quaternion.identity);
           }
       }       
   }
}
