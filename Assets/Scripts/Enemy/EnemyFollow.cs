using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public GameObject Player;
    [SerializeField] private float speedEnemy;
    public float distanceSet;
    private float distance;
    private void Update()
    {
        distance = Vector2.Distance(transform.position, Player.transform.position);
        Vector2 direction = Player.transform.position - transform.position;
        direction.Normalize();
        float ange = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if(distance < distanceSet)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, Player.transform.position, speedEnemy*Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.forward * ange);
        }
    }
}
