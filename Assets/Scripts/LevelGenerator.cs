using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    int sectionsAmount;
    int curSectionsLeft;
    int maxSpace = 18; //maximum space between bridgeSections;
    int conesAmount = 3;

    public GameObject bridgeSection;
    float distBetweenSections = 29.62f;

    public GameObject lastBridgeSection;
    float dist = 22.19f;

    public GameObject scoreBridgeSection;

    public List<GameObject> obstacle = new List<GameObject>();
    public List<float> obstacleHieght = new List<float>();
    public GameObject expansion;

    public Transform player;

    int numExpandors;

    public void GenerateLevel(int sectionsAmt, int mSpace, int conesAmt)
    {
        sectionsAmount = sectionsAmt;
        maxSpace = mSpace;
        conesAmount = conesAmt;

        StartCoroutine(BuildNextSection());
    }

    IEnumerator BuildNextSection()
    {
        curSectionsLeft = sectionsAmount;
        float xVal = 0;
        float lastSpawnedX = 0;

        for (int i = 0; i < 1; i++)
        {
            Vector3 spawnPoint = new Vector3(xVal, -10, 0);

            GameObject curSection = Instantiate(bridgeSection, spawnPoint, Quaternion.identity);

            xVal += distBetweenSections;
        }

        xVal += Random.Range(6, 12);

        while (true)
        {
            if (player != null)
            {
                if (Mathf.Abs(player.position.x - xVal) < 200 && curSectionsLeft > 0)
                {
                    numExpandors = 0;

                    for (int i = 0; i < (int)Random.Range(1, 3); i++)
                    {
                        Vector3 spawnPoint = new Vector3(xVal, -10, 0);

                        GameObject curSection = Instantiate(bridgeSection, spawnPoint, Quaternion.identity);
                        StartCoroutine(SpawnObstacles(curSection));
                        lastSpawnedX = xVal;

                        xVal += distBetweenSections;
                    }

                    if (sectionsAmount / 2 > curSectionsLeft) maxSpace += maxSpace / 5;

                    xVal += Random.Range(8, maxSpace);                                 //18 is max distance possible at walkspeed 5
                    curSectionsLeft--;
                }
                else if (curSectionsLeft == 0)
                {
                    Vector3 spawnPoint = new Vector3(lastSpawnedX + dist, -10, 0);
                    GameObject curSection = Instantiate(lastBridgeSection, spawnPoint, Quaternion.identity);

                    spawnPoint += new Vector3(distBetweenSections + 10, -20, 0);

                    curSection = Instantiate(scoreBridgeSection, spawnPoint, Quaternion.identity);
                    break;
                }
            }
            
            yield return null;
        }
    }

    IEnumerator SpawnObstacles(GameObject curSection)
    {
        float xVal = curSection.transform.position.x - distBetweenSections/2;
        float yVal = curSection.transform.position.y;

        int betweenCones = 0;

        List<float> xValues = new List<float>();

        for (int i = 0; i < 13; i++)
        {
            xValues.Add(xVal);
            xVal += 2;
        }

        List<float> conePositionX = new List<float>();

        if (conesAmount >= 9) conesAmount = 9;
        int spacer = 2;
        for (int i = 0; i < conesAmount; i++)
        {
            int randNumber = Random.Range(spacer, spacer + 3);

            if (randNumber < xValues.Count)
            {
                conePositionX.Add(xValues[randNumber]);

                xValues.RemoveAt(randNumber);
            }

            spacer += randNumber;
        }

        List<float> expansionPositionX = new List<float>();
        int expansionAmount = 5;

        for (int i = 0; i < expansionAmount; i++)
        {
            int randNumber = Random.Range(0, xValues.Count);
            expansionPositionX.Add(xValues[randNumber]);

            xValues.RemoveAt(randNumber);
        }

        for (int i = 0; i < conePositionX.Count; i++)
        {
            bool spawnSingle = ((int)Random.Range(0, 2) == 0) ? false : true;

            int randObstacle = ((int)Random.Range(0, 3) == 0) ? 0 : 1;

            if (spawnSingle)
            {
                SpawnSingle(conePositionX[i], yVal + obstacleHieght[randObstacle], obstacle[randObstacle], curSection);
            }
            else
            {
                SpawnDouble(conePositionX[i], yVal + obstacleHieght[randObstacle], obstacle[randObstacle], curSection);
            }
        }

        for (int i = 0; i < expansionPositionX.Count; i++)
        {
            bool spawnSingle = ((int)Random.Range(0, 10) == 0) ? false : true;

            if (spawnSingle)
            {
                SpawnSingle(expansionPositionX[i], yVal + 11.6f, expansion, curSection);
            }
            else
            {
                SpawnDouble(expansionPositionX[i], yVal + 11.6f, expansion, curSection);
            }
        }
        yield return null;
    }

    void SpawnSingle(float xVal, float yVal, GameObject spawnObject, GameObject section)
    {
        int zPos = (int)Random.Range(-1, 2);
        GameObject curObject = Instantiate(spawnObject, new Vector3(xVal, yVal, zPos * 2.5f), Quaternion.identity);
        curObject.transform.parent = section.transform;

        zPos++;
        if (zPos > 1) zPos = -1;

        int toSpawnExpansion = Random.Range(0, 3);

        if (spawnObject != expansion && toSpawnExpansion == 0)
        {
            GameObject curExpansion = Instantiate(spawnObject, new Vector3(xVal, yVal, zPos * 2.5f), Quaternion.identity);
            curExpansion.transform.parent = section.transform;

            numExpandors++;
        }

        if (spawnObject == expansion) numExpandors++;
    }

    void SpawnDouble(float xVal, float yVal, GameObject spawnObject, GameObject section)
    {
        int zPos = (int)Random.Range(-1, 2);
        int zPos1 = zPos;

        for (int i = 0; i < 25; i++)
        {
            zPos1 = (int)Random.Range(-1, 2);

            if (zPos1 != zPos) break;
        }

        if (zPos != zPos1)
        {
            GameObject curObject = Instantiate(spawnObject, new Vector3(xVal, yVal, zPos * 2.5f), Quaternion.identity);
            curObject.transform.parent = section.transform;
            curObject = Instantiate(spawnObject, new Vector3(xVal, yVal, zPos1 * 2.5f), Quaternion.identity);
            curObject.transform.parent = section.transform;

            if (spawnObject == expansion) numExpandors += 2;
        }
    }
}
