﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace _GAME._Scripts._CharacterController._Shooter
{
    [CreateAssetMenu(menuName = "DatHQ/Effects/New  Custom ImpactEffect", fileName = "CustomImpactEffect@")]
    public class CustomImpactEffectSample : ImpactEffectBase
    {
        public enum Align
        {
            Right, Forward, UP, Left, Back, Down
        }
        public Mesh mesh;
        public float size = 0.02f;
        public float margin = 0.01f;
        public float fadeSpeed = 0.1f;
        public Align alignTransform;
        [ColorUsage(true, true)]
        public Color color;
        public Material material;
        public override void DoImpactEffect(Vector3 position, Quaternion rotation, GameObject sender, GameObject receiver)
        {
            var dir = rotation * Vector3.forward;
            GameObject go = new GameObject();
            go.transform.position = position + dir * margin;


            switch (alignTransform)
            {
                case Align.Right:
                    go.transform.right = dir;
                    break;
                case Align.Forward:
                    go.transform.forward = dir;
                    break;
                case Align.UP:
                    go.transform.up = dir;
                    break;
                case Align.Left:
                    go.transform.right = -dir;
                    break;
                case Align.Back:
                    go.transform.forward = -dir;
                    break;
                case Align.Down:
                    go.transform.up = -dir;
                    break;
            }
            var renderer = go.AddComponent<MeshRenderer>();
            renderer.material = material;
            renderer.material.color = color;
            go.AddComponent<MeshFilter>().sharedMesh = mesh;
            go.transform.localScale = Vector3.one * size;
            go.transform.SetParent(ObjectContainer.root, true);
            go.AddComponent<Fade>().InitFade(renderer, fadeSpeed);
        }

        public class Fade : MonoBehaviour
        {
            public void InitFade(Renderer renderer, float fadeSpeed)
            {
                StartCoroutine(FadeColor(renderer, fadeSpeed));
            }
            IEnumerator FadeColor(Renderer renderer, float fadeSpeed)
            {
                float value = 0;

                while (value < 1)
                {
                    renderer.material.color = Color.Lerp(renderer.material.color, Color.clear, value);
                    value += fadeSpeed * Time.deltaTime;
                    value = Mathf.Clamp(value, 0f, 1f);
                    yield return null;
                }
                Destroy(gameObject);
            }
        }
    }

}