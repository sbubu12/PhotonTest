using Photon.Pun;
using UnityEngine;

public class AvatarFireBullet : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject bulletPrefab = default;

    private int nextBulletId = 0;

    public void Start()
    {
       
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            // 左クリックでカーソルの方向に弾を発射する
            if (Input.GetMouseButtonDown(0))
            {
                var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var direction = mousePosition - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x);

                photonView.RPC(nameof(FireBullet), RpcTarget.All, nextBulletId++, angle);
            }
        }
    }

    // 弾を発射するメソッド
    [PunRPC]
    private void FireBullet(int id,float angle)
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.GetComponent<Bullet>().Init(id,photonView.OwnerActorNr,transform.position, angle);

    }
}
