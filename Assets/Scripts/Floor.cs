using UnityEngine;
public class Floor : MonoBehaviour
{
   public GameObject block;
   public int width = 10;
   public int length = 4;
  
   void Start()
   {
       for (int z=0; z < length; ++z)
       {
           for (int x=0; x < width; ++x)
           {
               Instantiate(block, new Vector3(x,0,z), Quaternion.identity);
           }
       }       
   }
}
