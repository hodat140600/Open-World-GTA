using _GAME._Scripts.Npc.SkillSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour, IObserverObject
{
    [SerializeField]
    private float _minDistance = 5.0f;
    [SerializeField]
    private float _maxDistance = 10.0f;
    [SerializeField]
    const float _timeBetweenSpawn = 0.2f;

    private ObservableSubject _observableSubject;
    private ObjectPoolController _objectPoolController;

    public ObjectPoolController ObjectPoolController { get => _objectPoolController; private set => _objectPoolController = value; }

    public void Initialized(ObjectPoolController objectPoolController, ObservableSubject observableSubject)
    {
        ObjectPoolController = objectPoolController;
        _observableSubject = observableSubject;
        _observableSubject.AddObserver(this);
    }

    public void OnNotify()
    {
        
    }

    public void OnNotify(IDataObserver enemySpawnModel)
    {
        SpawnEnemies((EnemySpawnerModel)enemySpawnModel);
    }

    private void SpawnEnemies(EnemySpawnerModel enemySpawnModel)
    {
        enemySpawnModel.enemySpawns.ForEach(enemySpawn =>
        {
            StartCoroutine(SpawnLimit(enemySpawn.enemySetting.Enemy.Id, GetPosOnRing(ObjectPoolController.tommy.position, _minDistance, _maxDistance), enemySpawn.LimitInSpawn, enemySpawn.isEndLessSpawn));
        });
    }
    WaitForSeconds WaitForSeconds = new WaitForSeconds(_timeBetweenSpawn);
    IEnumerator SpawnLimit(int id, Vector3 position, int maxNumber, bool isEndlessSpawn)
    {
        // TODO: Condition to stop spawn
        while (isEndlessSpawn)
        {
            ObjectPoolController.SpawnEnemy(id, position);
            yield return WaitForSeconds;
            yield return new WaitUntil(() => ObjectPoolController.Enemies[id].Count < maxNumber);
        }
    }
    public Vector3 ToXZ(Vector2 aVec)
    {
        return new Vector3(aVec.x, 0, aVec.y);
    }

    public Vector3 GetPosOnRing(Vector3 origin, float minRadius, float maxRadius)
    {
        var v = Random.insideUnitCircle;
        return ToXZ(v.normalized * minRadius + v * (maxRadius - minRadius)) + origin;
    }
}
