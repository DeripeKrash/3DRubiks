using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubickscube : MonoBehaviour
{
    [SerializeField] uint size = 3;

    [SerializeField] VisibleCube refCube;

    [SerializeField] List<VisibleCube> visibleCubes;

    // Start is called before the first frame update
    void Start()
    {

        float U = (0.5f * (size - 1)) / size;

        for (uint i = 0; i < size; i++)
        {
            for (uint j = 0; j < size; j++)
            {
                for (uint k = 0; k < size; k++)
                {
                    if (i == 0 || j == 0 || k == 0 || i == size - 1 || j == size - 1 || k == size - 1)
                    {

                        VisibleCube newCube = Instantiate(refCube);

                        newCube.transform.localScale = new Vector3(1.0f / size, 1.0f / size, 1.0f / size);
                        newCube.transform.parent = transform;

                        newCube.transform.position = new Vector3(i * (1.0f / size), j * (1.0f / size), k * (1.0f / size));
                        newCube.transform.position -= new Vector3(U, U, U);

                        newCube.SetPositionOnCube(i, j, k);

                        visibleCubes.Add(newCube);
                    }
                }
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
