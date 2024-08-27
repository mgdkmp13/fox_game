using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedPlatforms : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab;
    const int PLATFORMS_NUM = 100;

    public GameObject[] platforms;
    public Vector3[] positions;
    private float startY; // Pocz�tkowa pozycja Y
                          // public Transform referenceObject; // Referencja do obiektu, wzgl�dem kt�rego b�d� umieszczane platformy

    public bool on = true;
    [SerializeField] private Sprite[] spriteArray;


    private void Awake()
    {
        platforms = new GameObject[PLATFORMS_NUM];
        positions = new Vector3[PLATFORMS_NUM];
        float step = 360f / PLATFORMS_NUM;


        // Pobierz pozycj� samego siebie (obiekt zawieraj�cy ten skrypt)
        Vector3 referencePosition = transform.position;

        for (int i = 0; i < PLATFORMS_NUM; i++)
        {
            float x = i * 0.3f + referencePosition.x; // Po�o�enie X wzd�u� osi poziomej
            float y = Mathf.Sin(Mathf.Deg2Rad * (i * step)) * 1f + referencePosition.y; // Wykorzystanie sinusoidy dla po�o�enia Y

            positions[i] = new Vector3(x, y, 0f);
            platforms[i] = Instantiate(platformPrefab, positions[i], Quaternion.identity);
        }
        startY = positions[0].y;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (on)
        {
            float amplitude = 0.8f; // Wysoko�� ruchu w g�r� i w d�
            float speed = 0.8f; // Szybko�� ruchu

            float waveFrequency = 2f * 3.14f/5f; // Cz�stotliwo�� fali (5 okres�w w zakresie 360 stopni)

            for (int i = 0; i < PLATFORMS_NUM; i++)
            {
                // Oblicz now� pozycj� Y za pomoc� sinusoidy w czasie
                float newY = Mathf.Sin(waveFrequency * (Time.time * speed - i * 0.1f)) * amplitude + startY;

                // Utw�rz now� pozycj� platformy
                Vector3 newPosition = new Vector3(platforms[i].transform.position.x, newY, platforms[i].transform.position.z);

                // Przesu� platform� w kierunku nowej pozycji Y
                platforms[i].transform.position = Vector3.MoveTowards(platforms[i].transform.position, newPosition, speed * Time.deltaTime);
            }
        }
    }

    public void swthc(Collider2D other)
    {
        on = !on;
        other.GetComponent<SpriteRenderer>().sprite = spriteArray[on ? 0 : 1];
    }


}
