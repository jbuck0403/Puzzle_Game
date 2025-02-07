using UnityEngine;

public class WeaponCollectible : Collectible
{
    GameObject player;
    PlayerInput playerInput;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected override bool OnCollect(Transform interactor)
    {
        playerInput = player.GetComponent<PlayerInput>();

        playerInput.EquipWeapon(this.transform);

        return true;
    }
}
