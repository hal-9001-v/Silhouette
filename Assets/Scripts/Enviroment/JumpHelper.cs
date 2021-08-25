using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JumpHelper : MonoBehaviour
{

    [Header("References")]
    [SerializeField] GameObject _platformPrefab;
    [SerializeField] JumpLink[] _links;

    [Header("Settings")]
    [SerializeField] Color _gizmosColor = Color.yellow;

    private void Start()
    {
        DisableRenderers();
    }

    [ContextMenu("All Scene/Turn on Renderers on All JumpHelpers")]
    public void EnableAllRenderersOnScene()
    {
        foreach (var helper in FindObjectsOfType<JumpHelper>())
        {
            helper.EnableRenderers();
        }

        EnableRenderers();
    }
    
    [ContextMenu("All Scene/Turn off Renderers on All JumpHelpers")]
    public void DisableAllRenderersOnScene()
    {
        foreach (var helper in FindObjectsOfType<JumpHelper>())
        {
            helper.DisableRenderers();
        }

        DisableRenderers();
    }

    [ContextMenu("Turn on Renderers")]
    public void EnableRenderers()
    {
        foreach (JumpHelper helper in GetComponentsInChildren<JumpHelper>())
        {
            if (helper != this)
            {
                foreach (Renderer renderer in helper.GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = true;
                }
            }
        }
    }

    [ContextMenu("Turn off Renderers")]
    public void DisableRenderers()
    {
        foreach (JumpHelper helper in GetComponentsInChildren<JumpHelper>())
        {
            if (helper != this)
            {
                foreach (Renderer renderer in helper.GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = false;
                }
            }
        }
    }

    [ContextMenu("Get OffMeshLinks in Children")]
    void GetChildrenOffMeshLinks()
    {

        EmptyLinks();

        var meshLinks = GetComponentsInChildren<OffMeshLink>();

        _links = new JumpLink[meshLinks.Length];

        for (int i = 0; i < meshLinks.Length; i++)
        {
            _links[i] = new JumpLink();

            _links[i].meshLink = meshLinks[i];

            _links[i].meshLink.name = "Segment " + (i + 1).ToString();

            if (_links[i].meshLink.startTransform != null && _links[i].meshLink.endTransform != null)
            {
                _links[i].meshLink.startTransform.parent = null;
                _links[i].meshLink.endTransform.parent = null;


                if (_links[i].meshLink.startTransform.position.y > _links[i].meshLink.endTransform.position.y)
                {
                    _links[i].meshLink.startTransform.name = "Up Point";
                    _links[i].meshLink.endTransform.name = "Down Point";

                    _links[i].meshLink.startTransform.parent = _links[i].meshLink.transform;
                    _links[i].meshLink.endTransform.parent = _links[i].meshLink.transform;
                }
                else
                {
                    _links[i].meshLink.startTransform.name = "Down Point";
                    _links[i].meshLink.endTransform.name = "Up Point";

                    _links[i].meshLink.endTransform.parent = _links[i].meshLink.transform;
                    _links[i].meshLink.startTransform.parent = _links[i].meshLink.transform;
                }
            }

            /*
            if (_platformPrefab != null)
            {
                GameObject surface = null;

                //Make pairs share Surface
                if (i % 2 == 0)
                {
                    surface = Instantiate(_platformPrefab);
                }

                _links[i].SetSurface(surface);
            }
            */
        }

    }

    void EmptyLinks()
    {
        if (_links != null)
        {
            foreach (JumpLink link in _links)
            {
                link.Free();
            }

            _links = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (_links != null)
        {
            foreach (JumpLink link in _links)
            {

                if (link.meshLink.startTransform != null && link.meshLink.endTransform != null)
                {
                    Gizmos.color = _gizmosColor;
                    Gizmos.DrawLine(link.meshLink.startTransform.position, link.meshLink.endTransform.position);
                }


            }
        }
    }

    [Serializable]
    public class JumpLink
    {
        public OffMeshLink meshLink;
        public GameObject surface { get; private set; }

        public void Free()
        {
            if (surface != null)
            {
                DestroyImmediate(surface);

                surface = null;
            }
        }

        public void SetSurface(GameObject newSurface)
        {
            Free();

            surface = newSurface;

            if (meshLink != null)
            {
                surface.transform.position = meshLink.transform.position;
                surface.transform.parent = meshLink.transform;
            }
        }

    }

}
