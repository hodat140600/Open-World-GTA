using UnityEngine;

public class ParticleSystemPlayOnStart : MonoBehaviour
{
    private ParticleSystem _ps;
    
    private void Awake()
    {
        _ps = GetComponent<ParticleSystem>();
    }

    void Start()
    {
        _ps.Play();
    }
}
