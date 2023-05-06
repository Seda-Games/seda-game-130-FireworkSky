using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTemplate : MonoBehaviour
{
    private static GameObject ParticleTemplateRoot;
    private static Dictionary<GameObject, ParticleTemplate> PrefabToParticleTempDir = null;
    private ParticleSystem[] tempPars = null;
    private int[] emitCount;
    public static ParticleTemplate Warm(GameObject prefab)
    {
        if (PrefabToParticleTempDir == null)
        {
            PrefabToParticleTempDir = new Dictionary<GameObject, ParticleTemplate>();
        }
        if (ParticleTemplateRoot == null)
        {
            ParticleTemplateRoot = new GameObject("ParticleTemplateRoot");
        }
        if (PrefabToParticleTempDir.TryGetValue(prefab, out var parTemp))
        {
            if (parTemp == null)
            {
                Debug.LogError("null particle template.");
                PrefabToParticleTempDir.Remove(prefab);
            }
        }
        if (parTemp == null)
        {
            var obj = GameObject.Instantiate<GameObject>(prefab, ParticleTemplateRoot.transform);
            if (!obj.TryGetComponent<ParticleTemplate>(out parTemp))
            {
                parTemp = obj.AddComponent<ParticleTemplate>();
            }

            PrefabToParticleTempDir.Add(prefab, parTemp);
        }

        return parTemp;
    }
    public static void Emit(GameObject prefab, Vector3 pos, Quaternion rot, Vector3 scale)
    {
        var temp = Warm(prefab);
        if (temp != null)
        {
            temp.Emit(pos, rot, scale);
        }
    }
    public static void Emit(GameObject prefab, Transform trans)
    {
        Emit(prefab, trans.position, trans.rotation, trans.lossyScale);
    }
    public static void Emit(GameObject prefab, Vector3 pos)
    {
        Emit(prefab, pos, Quaternion.identity, Vector3.one);
    }
    public static void Emit(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        Emit(prefab, pos, rot, Vector3.one);
    }
    public static void Clear()
    {
        PrefabToParticleTempDir.Clear();
    }


    private void OnDestroy()
    {
        foreach (var kv in PrefabToParticleTempDir)
        {
            if (kv.Value == this)
            {
                PrefabToParticleTempDir.Remove(kv.Key);
                return;
            }
        }
    }

    void Init()
    {
        tempPars = transform.GetComponentsInChildren<ParticleSystem>();
        emitCount = new int[tempPars.Length];
        for (int i = 0; i < tempPars.Length; i++)
        {
            var par = tempPars[i];
            int count = 1;
            var emission = par.emission;

            //test code.
            var parMain = par.main;
            parMain.simulationSpace = ParticleSystemSimulationSpace.World;
            parMain.maxParticles = 1000;


            if (emission.burstCount >= 1)
            {
                count = (int)emission.GetBurst(0).count.Evaluate(0);
            }
            emission.enabled = false;
            emitCount[i] = count;
        }
    }

    void Emit(Vector3 pos, Quaternion rot, Vector3 scale)
    {
        if (tempPars == null)
        {
            this.Init();
        }
        transform.position = pos;
        transform.rotation = rot;
        transform.localScale = scale;
        for (int i = 0; i < tempPars.Length; i++)
        {
            tempPars[i].Emit(emitCount[i]);
        }
    }
}