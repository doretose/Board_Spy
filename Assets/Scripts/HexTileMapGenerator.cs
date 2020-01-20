using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTileMapGenerator : MonoBehaviour
{
    public GameObject hexTilePrefab;
    public Transform holder;
    [SerializeField] int mapWidth = 0;
    [SerializeField] int mapHeight = 0;

    float tileXOffset = 1.8f;
    float tileZOffset = 1.565f;
    // Start is called before the first frame update
    void Start()
    {
        CreateHexTileMap();
    }

    // Update is called once per frame
    void CreateHexTileMap()
    {
        float mapXMin = 0;
        float mapXMax = mapWidth;

        float mapZmin = 0;
        float mapZMax = mapHeight;

        for (float x = mapXMin; x < mapXMax; x++)
        {
            for (float z = mapZmin; z < mapZMax; z++)
            {
                GameObject TempGo = Instantiate(hexTilePrefab);
                Vector3 pos;

                if (z % 2 == 0)
                {
                    pos = new Vector3(x * tileXOffset, 0, z * tileZOffset);
                }
                else
                {
                    pos = new Vector3(x * tileXOffset + tileXOffset /2, 0, z * tileZOffset);
                }
                StartCoroutine(SetTileInfo(TempGo, x, z, pos));
            }
        }
    }

    IEnumerator SetTileInfo(GameObject GO, float x, float z, Vector3 pos)
    {
        yield return new WaitForSeconds(0.00001f);
        GO.transform.parent = holder;
        GO.name = x.ToString() + ", " + z.ToString();
        int hex_x = Mathf.RoundToInt(x);
        int hex_z = Mathf.RoundToInt(z);
        GO.GetComponent<Hex>().x = hex_x;
        GO.GetComponent<Hex>().y = hex_z;
        GO.transform.position = pos;
    }

    private void OnTriggerExit(Collider other)
    {
        Destroy(other.gameObject);
    }
}
