using System.Collections;
using Unity.FPS.Game;
using UnityEngine;

public class SimpleBaseEnemy : MonoBehaviour
{
    private Health myHealth;
    private EnemyManager enemyManager;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        SetUp();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected virtual void FixedUpdate()
    {

    }

    protected virtual void SetUp()
    { 
        RegisterWithEnemyManager();
        SetUpHealthSystem();
    }

    protected virtual void RegisterWithEnemyManager()
    {
        if(enemyManager == null)
        {
            enemyManager = FindObjectOfType<EnemyManager>();
        }
        if(enemyManager != null)
        {
            // add ourselves to our enemy manager
            enemyManager.AddEnemy(gameObject);
        }     
    }

    protected virtual void SetUpHealthSystem()
    {
        // search for the health script
        myHealth = GetComponent<Health>();
        if (myHealth != null)
        {
            // subscribe our death function to this event, so when we take damage and reach a threshold our function is called.
            myHealth.OnDie += OnDeath;
        }
    }

    protected virtual void OnHealed()
    {
        Debug.Log("I am healed");
    }

    protected virtual void OnTakeDamage()
    {
        Debug.Log("I've taken damage");
    }

    protected virtual void OnDeath()
    {
        Debug.Log("I am dead");

        if (enemyManager != null)
        {
            // add ourselves to our enemy manager
            enemyManager.RemoveEnemy(gameObject);
        }

        if (myHealth)
        {
            // unsubscribe our death function to this event, so we don't get double ups of function calls.
            myHealth.OnDie -= OnDeath;
        }

        // destroy this object
        Destroy(gameObject);
    }
}
