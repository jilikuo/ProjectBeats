using UnityEngine;

namespace Jili.StatSystem.EntityTree.ConsumableSystem
{
    public class CollectionAura : MonoBehaviour
    {
        private readonly string playerTag = "Player";

        private GameObject player;
        private Vector3 offset = new Vector3(0, -0.2f, 0);

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag(playerTag);
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            collision.gameObject.GetComponent<ConsumableUse>().StartMoveToPlayer(); 
        }
    }
}