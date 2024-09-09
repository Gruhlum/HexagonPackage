
//using HexTecGames.Basics;
//using HexTecGames.TweenLib;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//namespace HexagonPackage
//{
//    public class HexagonGridBackdrop : MonoBehaviour
//    {
//        [SerializeField] private HexagonGrid hexGrid = default;
//        [SerializeField] private Spawner<SpriteRenderer> hexagonSpawner = default;
//        //[SerializeField] private TweenPlayer tweenPlayer = default;
//        [SerializeField] private Material transparentMaterial = default;

//        [SerializeField][Range(0f, 1f)] private float finalAlpha = 0.5f;

//        public bool Animate
//        {
//            get
//            {
//                return animate;
//            }
//            set
//            {
//                animate = value;
//            }
//        }
//        [SerializeField] private bool animate = default;

//        public bool AutoLoad
//        {
//            get
//            {
//                return autoLoad;
//            }
//            private set
//            {
//                autoLoad = value;
//            }
//        }
//        [SerializeField] private bool autoLoad = default;
//        public bool AutoRemove
//        {
//            get
//            {
//                return autoRemove;
//            }
//            private set
//            {
//                autoRemove = value;
//            }
//        }
//        [SerializeField] private bool autoRemove = default;

//        private void Awake()
//        {
//            hexGrid.GridLoaded += HexGrid_GridLoaded;
//            hexGrid.GridRemoved += HexGrid_GridRemoved;
//            Color col = transparentMaterial.color;
//            if (Animate)
//            {
//                col.a = 0;
//            }
//            else col.a = finalAlpha;
//            transparentMaterial.color = col;
//        }

        

//        private void OnDestroy()
//        {
//            Color col = transparentMaterial.color;
//            col.a = finalAlpha;
//            transparentMaterial.color = col;

//            hexGrid.GridLoaded -= HexGrid_GridLoaded;
//            hexGrid.GridRemoved -= HexGrid_GridRemoved;
//        }
//        private void HexGrid_GridRemoved()
//        {
//            if (!AutoRemove)
//            {
//                return;
//            }
//            DeactivateAll();
//        }
//        private void HexGrid_GridLoaded()
//        {
//            if (!AutoLoad)
//            {
//                return;
//            }
//            GenerateBackground();
//        }

//        [ContextMenu("Generate")]
//        public void GenerateBackground()
//        {
//            List<GameObject> GOs = new List<GameObject>();
//            if (Application.isPlaying == false)
//            {
//                hexagonSpawner.TryDestroyAll();
//            }
//            foreach (var hex in hexGrid.Hexagons.Values)
//            {
//                SpriteRenderer sr = hexagonSpawner.Spawn();
//                sr.transform.position = hex.transform.position;
//                GOs.Add(sr.gameObject);
//            }
//            if (Animate && Application.isPlaying)
//            {
//                StartCoroutine(AnimateActivation(1f, 1f));
//            }
//        }
//        public void DeactivateAll(bool animate = true)
//        {
//            if (hexagonSpawner.GetActiveBehaviours().Count() <= 0)
//            {
//                return;
//            }
//            if (animate)
//            {
//                StartCoroutine(AnimateDeactivation(0.8f, 0.2f));
//            }
//            else
//            {
//                Color col = transparentMaterial.color;
//                col.a = 0;
//                transparentMaterial.color = col;
//                foreach (var sr in hexagonSpawner.GetActiveBehaviours())
//                {
//                    sr.gameObject.SetActive(false);
//                }
//            } 
//        }
//        private IEnumerator AnimateActivation(float delay, float duration)
//        {
//            yield return new WaitForSeconds(delay);
//            for (float i = 0; i < duration; i += Time.deltaTime)
//            {
//                Color col = transparentMaterial.color;
//                col.a = Mathf.Lerp(0f, finalAlpha, i / duration);
//                transparentMaterial.color = col;
//                yield return new WaitForEndOfFrame();
//            }
//        }
//        private IEnumerator AnimateDeactivation(float delay, float duration)
//        {
//            yield return new WaitForSeconds(delay);
//            for (float i = 0; i < duration; i += Time.deltaTime)
//            {
//                Color col = transparentMaterial.color;
//                col.a = Mathf.Lerp(finalAlpha, 0f, i / duration);
//                transparentMaterial.color = col;
//                yield return new WaitForEndOfFrame();
//            }
//            foreach (var sr in hexagonSpawner.GetActiveBehaviours())
//            {
//                sr.gameObject.SetActive(false);
//            }
//        }
//    }
//}