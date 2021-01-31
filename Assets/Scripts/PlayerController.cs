using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CreaturePermutation
{
    public List<CreatureController> creatures = new List<CreatureController>();
    public float distance;
}

public class ColliderComparer : IComparer<Collider2D>
{
    int IComparer<Collider2D>.Compare(Collider2D a, Collider2D b)
    {
        return a.transform.position.y.CompareTo(b.transform.position.y);
    }
}

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 10f;
    [SerializeField]
    CreatureData[] startCreatures;
    [SerializeField]
    float creatureDistance = 1f;
    [SerializeField]
    float creatureSeparation = 45f;
    [SerializeField]
    LayerMask enemySelectMask;

    public static PlayerController Instance;
    public static event System.Action<CreatureController> OnCreatureAdd;

    public EnemyController TargetEnemy { get { return selected; } }
    public List<CreatureController> Creatures { get { return creatures; } }

    Rigidbody2D rigidbody;
    InputActionAsset actions;

    Vector2 dir = Vector2.down;

    List<CreatureController> creatures = new List<CreatureController>();
    List<Vector2> creaturePositions = new List<Vector2>();
    Dictionary<CreatureController, int> creatureIndices = new Dictionary<CreatureController, int>();
    List<CreaturePermutation> permutations = new List<CreaturePermutation>();

    Collider2D[] colliders = new Collider2D[10];
    ColliderComparer colliderComparer = new ColliderComparer();
    EnemyController highlighted;
    EnemyController selected;

    void Awake()
    {
        Instance = this;

        rigidbody = GetComponent<Rigidbody2D>();
        actions = GetComponent<PlayerInput>().actions;

        CalcCreaturePositions(startCreatures.Length);
        for (int i = 0; i < startCreatures.Length; i++)
            AddCreature(startCreatures[i], null);
    }

    void CalcCreaturePositions(int count)
    {
        for (int i = creaturePositions.Count; i < count; i++)
            creaturePositions.Add(Vector2.zero);
        creaturePositions.RemoveRange(count, creaturePositions.Count - count);
        for (int i = 0; i < count; i++)
            creaturePositions[i] = transform.position + Quaternion.Euler(0f, 0f, -creatureSeparation * (count - 1) / 2f + creatureSeparation * i) * -dir * creatureDistance;
    }

    public void AddCreature(CreatureData data, CreatureController obj)
    {
        if (creaturePositions.Count < creatures.Count + 1)
            CalcCreaturePositions(creatures.Count + 1);
        if (obj == null)
        {
            obj = Instantiate(data.Prefab, creaturePositions[creatures.Count], Quaternion.identity);
            obj.Data = data;
        }
        creatureIndices[obj] = creatures.Count;
        if (permutations.Count == 0)
        {
            CreaturePermutation perm = new CreaturePermutation();
            perm.creatures.Add(obj);
            permutations.Add(perm);
        }
        else
        {
            int oldCount = permutations.Count;
            for (int i = 0; i < oldCount; i++)
            {
                for (int j = 0; j < permutations[i].creatures.Count; j++)
                {
                    CreaturePermutation perm = new CreaturePermutation();
                    perm.creatures.AddRange(permutations[i].creatures);
                    perm.creatures.Insert(j, obj);
                    permutations.Add(perm);
                }
                permutations[i].creatures.Add(obj);
            }
        }
        creatures.Add(obj);
        if (OnCreatureAdd != null)
            OnCreatureAdd(obj);
    }

    void AssignCreatures()
    {
        for (int i = 0; i < permutations.Count; i++)
        {
            permutations[i].distance = 0f;
            for (int j = 0; j < permutations[i].creatures.Count; j++)
            {
                permutations[i].distance += Vector2.Distance(permutations[i].creatures[j].transform.position, creaturePositions[j]);
            }
        }
        int minIndex = 0;
        float minDist = permutations[0].distance;
        for (int i = 1; i < permutations.Count; i++)
        {
            if (permutations[i].distance < minDist)
            {
                minIndex = i;
                minDist = permutations[i].distance;
            }
        }
        for (int j = 0; j < permutations[minIndex].creatures.Count; j++)
        {
            creatureIndices[permutations[minIndex].creatures[j]] = j;
        }
    }

    void Update()
    {
        rigidbody.velocity = actions.FindAction("Move").ReadValue<Vector2>() * moveSpeed;
        if (rigidbody.velocity.magnitude > 0)
        {
            dir = rigidbody.velocity.normalized;
        }
        int colCount = Physics2D.OverlapPointNonAlloc(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), colliders, enemySelectMask.value);
        System.Array.Sort(colliders, 0, colCount, colliderComparer);
        EnemyController newHighlighted = null;
        for (int i = 0; i < colCount; i++)
        {
            EnemyController enemy = colliders[i].GetComponentInParent<EnemyController>();
            if (enemy != null)
            {
                newHighlighted = enemy;
                break;
            }
        }
        if (newHighlighted != highlighted)
        {
            if (highlighted != null)
                highlighted.SetHighlighted(false);
            highlighted = newHighlighted;
            if (highlighted != null)
                highlighted.SetHighlighted(true);
        }
        if (highlighted != selected && actions.FindAction("Select").triggered)
        {
            if (selected != null)
                selected.SetSelected(false);
            selected = highlighted;
            if (selected != null)
                selected.SetSelected(true);
        }
    }

    private void FixedUpdate()
    {
        CalcCreaturePositions(creatures.Count);
        AssignCreatures();
    }

    public Vector2 GetCreaturePosition(CreatureController creature)
    {
        return creaturePositions[creatureIndices[creature]];
    }
}
