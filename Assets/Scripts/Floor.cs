using UnityEngine;
public class Floor : MonoBehaviour
{
   public GameObject block;
   [SerializeField]private int width = 10;
   [SerializeField]private int length = 4;
  
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
