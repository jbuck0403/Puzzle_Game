using UnityEngine;

public class WeaponCollectible : Collectible
{
    GameObject player;
    PlayerInput playerInput;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void OnCollect()
    {
        playerInput = player.GetComponent<PlayerInput>();

        playerInput.EquipWeapon(this.transform);
    }
}
