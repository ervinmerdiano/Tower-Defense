using UnityEngine;

public class TowerPlacement2D : MonoBehaviour
{
    public GameObject towerPrefab;        // Tower asli
    public GameObject ghostTowerPrefab;   // Preview tower
    private GameObject ghostTower;

    public LayerMask platformLayer;       // Layer Platform
    public LayerMask towerLayer;          // Layer Tower (buat cek overlap)

    private bool canPlace = false;

    void Start()
    {
        ghostTower = Instantiate(ghostTowerPrefab);
        ghostTower.SetActive(false);
    }

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Raycast buat cek platform
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, platformLayer);

        if (hit.collider != null)
        {
            ghostTower.SetActive(true);
            ghostTower.transform.position = hit.point;

            // Cek apakah overlap tower lain
            bool overlappingTower = Physics2D.OverlapCircle(hit.point, 0.3f, towerLayer);
            canPlace = !overlappingTower;

            // Ubah warna ghost sesuai valid / tidak
            SpriteRenderer sr = ghostTower.GetComponent<SpriteRenderer>();
            sr.color = canPlace ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);

            // Klik mouse kiri → place tower
            if (Input.GetMouseButtonDown(0) && canPlace)
            {
                Instantiate(towerPrefab, hit.point, Quaternion.identity);
            }
        }
        else
        {
            ghostTower.SetActive(false);
            canPlace = false;
        }
    }
}
