using UnityEngine;

public class PlayerDamageable : Damageable
{
    public override string Name => "player";
    
    [SerializeField] private Healthbar healthbar;
    
    private Rigidbody _rb;
    
    private void Start()
    {
        healthbar.SetMaximumHealth(Health);
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            TakeDamage(10);
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        healthbar.SetHealth(Health);
    }

    protected override void Die()
    {
        Debug.Log("You died!");
        _rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
