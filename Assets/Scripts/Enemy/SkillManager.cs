using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public SkillSO s;
    [SerializeField]private GameObject skillpos;
    [SerializeField] private GameObject skill;
    public GameObject player;
    public float knockRange;
    //[SerializeField] private Collider PlayerCollider;
    private void Start()
    {
        skillpos = gameObject;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            /*RaycastHit hit = new RaycastHit();
            if(PlayerCollider.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 9999f))
            {
                GameObject skill = SpanwnerSkill();
                skill.transform.position = hit.point + skill.transform.position;
            }*/
            
            skill = SpanwnerSkill();
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            skill.transform.position = new Vector3(mousePosition.x, mousePosition.y, skill.transform.position.z);

            if (skill == null) return;
            else
            {
                if (Vector2.Distance(skill.transform.position, player.transform.position) < knockRange)
                {
                    KnockToCharacter();
                    Debug.Log("-100HP");
                }
            }
        }

        
    }

    private GameObject SpanwnerSkill()
    {
        GameObject cloneskill = (GameObject)Instantiate(s.image);
        cloneskill.transform.position = new Vector3(0, cloneskill.transform.position.y, 0);
    #if UNITY_3_5
			particles.SetActiveRecursively(true);
    #else
        cloneskill.SetActive(true);
    #endif

        ParticleSystem ps = cloneskill.GetComponent<ParticleSystem>();

    #if UNITY_5_5_OR_NEWER
        if (ps != null)
        {
            var main = ps.main;
            if (main.loop)
            {
                ps.gameObject.AddComponent<CFX_AutoStopLoopedEffect>();
                ps.gameObject.AddComponent<CFX_AutoDestructShuriken>();
            }
        }
    #else
		if(ps != null && ps.loop)
		{
			ps.gameObject.AddComponent<CFX_AutoStopLoopedEffect>();
			ps.gameObject.AddComponent<CFX_AutoDestructShuriken>();
		}
    #endif
        return cloneskill;
    }

    /*
     using UnityEngine;

    public class CharacterController : MonoBehaviour
    {
        public float knockbackForce = 10f;

        public void ApplyKnockback(Vector3 knockbackDirection)
        {
            Rigidbody characterRigidbody = GetComponent<Rigidbody>();
            characterRigidbody.velocity = Vector3.zero; // Đặt vận tốc của nhân vật về 0 trước khi áp dụng knockback.
            characterRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        }
    }
 
     */

    public void KnockToCharacter()
    {
        Vector3 knockDirection = (player.transform.position - skillpos.transform.position).normalized;
        player.GetComponent<HatTung>().Knock(knockDirection);
    }

}
