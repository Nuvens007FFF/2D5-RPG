using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject player;
    float distance;
    //public GameObject Xoaywater;
    public SkillSO s;
    void Update()
    {
        CheckPos();
    }

    void CheckPos()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        if(distance <= 1.5)
        {
            GameObject SkillNienThu = Instantiate(s.image, this.transform.position, Quaternion.identity);
            Destroy(SkillNienThu, 1.5f);
        }
    }
}
