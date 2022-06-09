using Mirror;
using UnityEngine;

public class ResourceGenerator : NetworkBehaviour
{
  //  [SerializeField] private Health health = null;
    [SerializeField] private int resourcesPerInterval = 10;
    [SerializeField] private float interval = 2f;

    private float timer;
    private RTSPlayer player;

    public GameObject Node;

    public override void OnStartServer()
    {
        timer = interval;
        player = connectionToClient.identity.GetComponent<RTSPlayer>();

      //  health.ServerOnDie += ServerHandleDie;
        GameOverHandler.ServerOnGameOver += ServerHandleGameOver;
    }

    public override void OnStopServer()
    {
       // health.ServerOnDie -= ServerHandleDie;
        GameOverHandler.ServerOnGameOver -= ServerHandleGameOver;
    }

    [ServerCallback]
    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            player.SetPlayerResources(player.GetPlayerResources() + resourcesPerInterval);

            timer += interval;
        }       
        
            GameObject[] gos;
            gos = GameObject.FindGameObjectsWithTag("SyntheticNode");
            GameObject Node = null;
            float distance = Mathf.Infinity;
            Vector3 position = transform.position;
            foreach (GameObject go in gos)
            {
                Vector3 diff = go.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    Node = go;
                    distance = curDistance;
                }
            }
        Node = GameObject.FindGameObjectWithTag("SyntheticNode");
    }
    

    private void ServerHandleDie()
    {
        NetworkServer.Destroy(gameObject);
    }

    private void ServerHandleGameOver()
    {
        enabled = false;
    }
}